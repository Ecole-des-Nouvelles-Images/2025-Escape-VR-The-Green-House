using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace Overdrive.Todo
{
    [System.Serializable]
    public class TodoItem
    {
        public string text;
        public bool isCompleted;
        public List<string> assetGUIDs = new List<string>();
        public List<string> gameObjectGlobalIds = new List<string>();
        public List<string> gameObjectNames = new List<string>();
        public List<string> gameObjectScenePaths = new List<string>();
    }

    [CreateAssetMenu(fileName = "TodoData", menuName = "Overdrive/Todo Data")]
    public class TodoData : ScriptableObject
    {
        [Header("Todo Items")]
        public List<TodoItem> items = new List<TodoItem>();
        
        private static TodoData _instance;
        private const string DataPath = "Packages/com.overdrive.todo/ProjectData/TodoData.asset";
        
        public static TodoData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = AssetDatabase.LoadAssetAtPath<TodoData>(DataPath);
                    if (_instance == null && !EditorApplication.isCompiling)
                    {
                        CreateDataAsset();
                    }
                }
                return _instance;
            }
        }
        
        public static void CreateDataAsset()
        {
            if (_instance != null) return;
            
            _instance = CreateInstance<TodoData>();
            
            string directory = Path.GetDirectoryName(DataPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            AssetDatabase.CreateAsset(_instance, DataPath);
            AssetDatabase.SaveAssets();
        }
        
        [InitializeOnLoadMethod]
        private static void InitializeAfterRecompilation()
        {
            _instance = AssetDatabase.LoadAssetAtPath<TodoData>(DataPath);
        }
        
        public void AddTodoItem(string text)
        {
            var item = new TodoItem
            {
                text = text,
                isCompleted = false,
                assetGUIDs = new List<string>(),
                gameObjectGlobalIds = new List<string>(),
                gameObjectNames = new List<string>(),
                gameObjectScenePaths = new List<string>()
            };
            items.Add(item);
            SaveData();
        }
        
        public void RemoveTodoItem(TodoItem item)
        {
            items.Remove(item);
            SaveData();
        }
        
        public void ToggleTodoItem(TodoItem item)
        {
            item.isCompleted = !item.isCompleted;
            SaveData();
        }
        
        public void AddAssetToTodoItem(TodoItem item, Object asset)
        {
            if (asset is GameObject gameObject)
            {
                string assetPath = AssetDatabase.GetAssetPath(gameObject);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    string guid = AssetDatabase.AssetPathToGUID(assetPath);
                    if (!string.IsNullOrEmpty(guid) && !item.assetGUIDs.Contains(guid))
                    {
                        item.assetGUIDs.Add(guid);
                    }
                }
                else
                {
                    string globalId = GetGlobalObjectId(gameObject);
                    if (!string.IsNullOrEmpty(globalId) && !item.gameObjectGlobalIds.Contains(globalId))
                    {
                        item.gameObjectGlobalIds.Add(globalId);
                        item.gameObjectNames.Add(gameObject.name);
                        item.gameObjectScenePaths.Add(gameObject.scene.path);
                    }
                }
            }
            else
            {
                string assetPath = AssetDatabase.GetAssetPath(asset);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    string guid = AssetDatabase.AssetPathToGUID(assetPath);
                    if (!string.IsNullOrEmpty(guid) && !item.assetGUIDs.Contains(guid))
                    {
                        item.assetGUIDs.Add(guid);
                    }
                }
            }
            SaveData();
        }
        
        public void RemoveAssetFromTodoItem(TodoItem item, string guid)
        {
            item.assetGUIDs.Remove(guid);
            SaveData();
        }
        
        public void RemoveGameObjectFromTodoItem(TodoItem item, int index)
        {
            if (index >= 0 && index < item.gameObjectGlobalIds.Count)
            {
                item.gameObjectGlobalIds.RemoveAt(index);
                if (index < item.gameObjectNames.Count)
                    item.gameObjectNames.RemoveAt(index);
                if (index < item.gameObjectScenePaths.Count)
                    item.gameObjectScenePaths.RemoveAt(index);
                SaveData();
            }
        }
        
        public void ClearAll()
        {
            items.Clear();
            SaveData();
        }
        
        public string GetGlobalObjectId(GameObject gameObject)
        {
            if (gameObject == null)
            {
                Debug.LogWarning("[Todo] Cannot get GlobalObjectId for null GameObject");
                return null;
            }
            
            try
            {
                GlobalObjectId globalId = GlobalObjectId.GetGlobalObjectIdSlow(gameObject);
                if (globalId.identifierType == 0)
                {
                    Debug.LogWarning($"[Todo] Invalid GlobalObjectId for GameObject '{gameObject.name}' - identifierType is 0");
                    return null;
                }
                return globalId.ToString();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[Todo] Failed to get GlobalObjectId for GameObject '{gameObject.name}': {ex.Message}");
                return null;
            }
        }
        
        public GameObject GetGameObjectFromGlobalId(string globalIdString, bool logWarnings = false)
        {
            if (string.IsNullOrEmpty(globalIdString))
            {
                if (logWarnings)
                    Debug.LogWarning("[Todo] Cannot resolve null or empty GlobalObjectId");
                return null;
            }

            if (!globalIdString.StartsWith("GlobalObjectId"))
            {
                if (logWarnings)
                    Debug.LogWarning($"[Todo] Invalid GlobalObjectId format: '{globalIdString}' - doesn't start with 'GlobalObjectId'");
                return null;
            }

            if (!GlobalObjectId.TryParse(globalIdString, out GlobalObjectId id))
            {
                if (logWarnings)
                    Debug.LogWarning($"[Todo] Failed to parse GlobalObjectId: '{globalIdString}'");
                return null;
            }

            // Check if this is a scene object (identifierType 2)
            // and if so, check if the scene is loaded before attempting to resolve
            if (id.identifierType == 2)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(id.assetGUID.ToString());
                if (!string.IsNullOrEmpty(scenePath))
                {
                    var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByPath(scenePath);
                    if (!scene.isLoaded)
                    {
                        // Scene not loaded, can't resolve GameObject
                        return null;
                    }
                }
            }

            try
            {
                UnityEngine.Object obj = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id);
                if (obj == null)
                {
                    if (logWarnings)
                        Debug.LogWarning($"[Todo] GlobalObjectId '{globalIdString}' resolved to null - GameObject may be from unopened scene or deleted");
                    return null;
                }

                var gameObject = obj as GameObject;
                if (gameObject == null)
                {
                    if (logWarnings)
                        Debug.LogWarning($"[Todo] GlobalObjectId '{globalIdString}' resolved to non-GameObject object of type {obj.GetType().Name}");
                    return null;
                }

                return gameObject;
            }
            catch (System.Exception ex)
            {
                if (logWarnings)
                    Debug.LogError($"[Todo] Failed to resolve GlobalObjectId '{globalIdString}': {ex.Message}");
                return null;
            }
        }
        
        public GameObject GetGameObjectFromGlobalId(string globalIdString)
        {
            return GetGameObjectFromGlobalId(globalIdString, logWarnings: false);
        }
        
        public void CleanupInvalidReferences()
        {
            bool hasChanges = false;
            
            foreach (var item in items)
            {
                var validAssetGUIDs = new List<string>();
                foreach (var guid in item.assetGUIDs)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        validAssetGUIDs.Add(guid);
                    }
                    else
                    {
                        hasChanges = true;
                    }
                }
                item.assetGUIDs = validAssetGUIDs;
                
                var toRemove = new List<int>();
                for (int i = 0; i < item.gameObjectGlobalIds.Count; i++)
                {
                    if (ShouldRemoveInvalidGameObjectReference(item, i))
                    {
                        toRemove.Add(i);
                        hasChanges = true;
                    }
                }
                
                for (int i = toRemove.Count - 1; i >= 0; i--)
                {
                    int index = toRemove[i];
                    item.gameObjectGlobalIds.RemoveAt(index);
                    if (index < item.gameObjectNames.Count)
                        item.gameObjectNames.RemoveAt(index);
                    if (index < item.gameObjectScenePaths.Count)
                        item.gameObjectScenePaths.RemoveAt(index);
                }
            }
            
            if (hasChanges)
            {
                SaveData();
            }
        }
        
        private bool ShouldRemoveInvalidGameObjectReference(TodoItem item, int index)
        {
            if (index >= item.gameObjectGlobalIds.Count)
                return true;
                
            string globalId = item.gameObjectGlobalIds[index];
            if (string.IsNullOrEmpty(globalId))
                return true;
            
            string scenePath = index < item.gameObjectScenePaths.Count ? item.gameObjectScenePaths[index] : null;
            
            if (string.IsNullOrEmpty(scenePath))
            {
                return true;
            }
            
            if (!System.IO.File.Exists(scenePath))
            {
                return true;
            }
            
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByPath(scenePath);
            if (scene.isLoaded)
            {
                var gameObject = GetGameObjectFromGlobalId(globalId, logWarnings: false);
                if (gameObject == null)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        public void SaveData()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}