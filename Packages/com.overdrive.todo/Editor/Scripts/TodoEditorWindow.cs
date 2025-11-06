using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Overdrive.Todo
{
    public class TodoEditorWindow : EditorWindow
    {
        private ScrollView todoList;
        private ScrollView doneList;
        private VisualElement newTodoCreation;
        private TextField newTodoField;
        private Button addNewTodoButton;

        [MenuItem("Tools/Overdrive/Todo")]
        public static void ShowWindow()
        {
            var window = GetWindow<TodoEditorWindow>();
            window.titleContent = new GUIContent("Todo");
        }

        public void CreateGUI()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("UI/TodoEditor");
            visualTree.CloneTree(rootVisualElement);

            todoList = rootVisualElement.Q<ScrollView>("TodoList");
            doneList = rootVisualElement.Q<ScrollView>("DoneList");
            newTodoCreation = rootVisualElement.Q<VisualElement>("NewTodoCreation");
            newTodoField = rootVisualElement.Q<TextField>("NewTodoField");
            addNewTodoButton = rootVisualElement.Q<Button>("AddNewTodo");

            var cancelButton = rootVisualElement.Q<Button>("CancelNewTodo");

            addNewTodoButton.clicked += ShowNewTodoCreation;
            cancelButton.clicked += HideNewTodoCreation;

            newTodoField.RegisterCallback<KeyDownEvent>(OnNewTodoKeyDown);

            TodoUserPreferences.onPreferenceChanged += OnPreferenceChanged;
            EditorSceneManager.sceneOpened += OnSceneChanged;
            EditorSceneManager.sceneClosed += OnSceneClosed;
            EditorSceneManager.activeSceneChangedInEditMode += OnActiveSceneChanged;

            RefreshUI();
        }
        
        public void RequestRefresh()
        {
            RefreshUI();
        }

        private void OnPreferenceChanged(string preferenceName, bool value)
        {
            if (preferenceName == TodoUserPreferences.ShowDoneItemsPref)
            {
                RefreshUI();
            }
        }

        private void OnDestroy()
        {
            TodoUserPreferences.onPreferenceChanged -= OnPreferenceChanged;
            EditorSceneManager.sceneOpened -= OnSceneChanged;
            EditorSceneManager.sceneClosed -= OnSceneClosed;
            EditorSceneManager.activeSceneChangedInEditMode -= OnActiveSceneChanged;
        }

        private void OnSceneChanged(Scene scene, OpenSceneMode mode)
        {
            RefreshUI();
        }

        private void OnSceneClosed(Scene scene)
        {
            RefreshUI();
        }

        private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            RefreshUI();
        }

        private void ShowNewTodoCreation()
        {
            newTodoCreation.style.display = DisplayStyle.Flex;
            newTodoField.value = "";
            newTodoField.Focus();
        }

        private void HideNewTodoCreation()
        {
            newTodoCreation.style.display = DisplayStyle.None;
        }

        private void OnNewTodoKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
            {
                if (!string.IsNullOrWhiteSpace(newTodoField.value))
                {
                    AddTodoItem(newTodoField.value);
                    HideNewTodoCreation();
                }
                evt.StopPropagation();
            }
            else if (evt.keyCode == KeyCode.Escape)
            {
                HideNewTodoCreation();
                evt.StopPropagation();
            }
        }

        private void AddTodoItem(string text)
        {
            TodoData.Instance?.AddTodoItem(text);
            RefreshUI();
        }

        private void DeleteTodoItem(TodoItem item)
        {
            TodoData.Instance?.RemoveTodoItem(item);
            RefreshUI();
        }

        private void ToggleTodoItem(TodoItem item)
        {
            TodoData.Instance?.ToggleTodoItem(item);
            RefreshUI();
        }

        private void RefreshUI()
        {
            todoList.Clear();
            doneList.Clear();

            var todoData = TodoData.Instance;
            if (todoData == null) return;

            todoData.CleanupInvalidReferences();

            foreach (var item in todoData.items)
            {
                var itemElement = CreateTodoItemElement(item);
                if (item.isCompleted)
                {
                    if (TodoUserPreferences.GetShowDoneItems())
                    {
                        doneList.Add(itemElement);
                    }
                }
                else
                {
                    todoList.Add(itemElement);
                }
            }
        }

        private VisualElement CreateTodoItemElement(TodoItem item)
        {
            var itemTemplate = Resources.Load<VisualTreeAsset>("UI/TodoItem");
            var itemElement = itemTemplate.CloneTree();

            var toggle = itemElement.Q<Toggle>("TodoToggle");
            var label = itemElement.Q<Label>("TodoText");
            var assetsContainer = itemElement.Q<VisualElement>("AssetsContainer");
            var assetsList = itemElement.Q<ScrollView>("AssetsList");
            var dropZone = itemElement.Q<VisualElement>("DropZone");

            toggle.value = item.isCompleted;
            label.text = item.text;
            label.style.whiteSpace = WhiteSpace.Normal;

            if (item.isCompleted)
            {
                label.style.color = new StyleColor(Color.gray);
                label.style.unityFontStyleAndWeight = FontStyle.Italic;
            }

            toggle.RegisterValueChangedCallback(evt => ToggleTodoItem(item));

            SetupDragDrop(itemElement, item);
            SetupContextMenu(itemElement, item, label);
            RefreshAssetReferences(item, assetsContainer, assetsList);

            return itemElement;
        }


        private void SetupDragDrop(VisualElement itemElement, TodoItem item)
        {
            var dropZone = itemElement.Q<VisualElement>("DropZone");
            
            itemElement.RegisterCallback<DragEnterEvent>(evt =>
            {
                if (IsValidDrag())
                {
                    dropZone.style.display = DisplayStyle.Flex;
                    dropZone.style.backgroundColor = new StyleColor(new Color(0.2f, 0.6f, 0.2f, 0.3f));
                }
            });

            itemElement.RegisterCallback<DragLeaveEvent>(evt =>
            {
                dropZone.style.display = DisplayStyle.None;
                dropZone.style.backgroundColor = new StyleColor(new Color(0.4f, 0.4f, 0.4f, 0.2f));
            });

            itemElement.RegisterCallback<DragUpdatedEvent>(evt =>
            {
                if (IsValidDrag())
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                }
                else
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                }
            });

            itemElement.RegisterCallback<DragPerformEvent>(evt =>
            {
                if (IsValidDrag())
                {
                    DragAndDrop.AcceptDrag();
                    
                    foreach (var obj in DragAndDrop.objectReferences)
                    {
                        if (obj != null)
                        {
                            AddAssetToTodoItem(item, obj);
                        }
                    }
                    
                    dropZone.style.display = DisplayStyle.None;
                    dropZone.style.backgroundColor = new StyleColor(new Color(0.4f, 0.4f, 0.4f, 0.2f));
                    RefreshUI();
                }
            });
        }

        private void SetupContextMenu(VisualElement itemElement, TodoItem item, Label label)
        {
            itemElement.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button == 1) // Right click
                {
                    ShowTodoContextMenu(item, label);
                    evt.StopPropagation();
                }
            });
        }

        private void ShowTodoContextMenu(TodoItem item, Label label)
        {
            GenericMenu menu = new GenericMenu();
            
            menu.AddItem(new GUIContent("Rename"), false, () => StartRename(item, label));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Delete"), false, () => DeleteTodoItem(item));
            
            menu.ShowAsContext();
        }

        private void StartRename(TodoItem item, Label label)
        {
            // Hide the label and create a text field for editing
            var container = label.parent;
            label.style.display = DisplayStyle.None;
            
            var textField = new TextField
            {
                value = item.text,
                style = {
                    flexGrow = 1,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    marginLeft = 0,
                    marginRight = 0,
                    marginTop = 0,
                    marginBottom = 0
                }
            };
            
            container.Insert(container.IndexOf(label), textField);
            textField.Focus();
            textField.SelectAll();
            
            System.Action finishRename = () =>
            {
                if (!string.IsNullOrWhiteSpace(textField.value))
                {
                    item.text = textField.value;
                    label.text = item.text;
                    TodoData.Instance?.SaveData();
                }
                
                container.Remove(textField);
                label.style.display = DisplayStyle.Flex;
            };
            
            System.Action cancelRename = () =>
            {
                container.Remove(textField);
                label.style.display = DisplayStyle.Flex;
            };
            
            textField.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                {
                    finishRename();
                    evt.StopPropagation();
                }
                else if (evt.keyCode == KeyCode.Escape)
                {
                    cancelRename();
                    evt.StopPropagation();
                }
            });
            
            textField.RegisterCallback<BlurEvent>(evt =>
            {
                finishRename();
            });
        }

        private bool IsValidDrag()
        {
            if (DragAndDrop.objectReferences == null || DragAndDrop.objectReferences.Length == 0)
                return false;

            bool hasValidObjects = false;
            
            foreach (var obj in DragAndDrop.objectReferences)
            {
                if (obj == null) continue;
                
                if (obj is GameObject gameObject)
                {
                    // Scene GameObjects (not prefabs) have empty asset path
                    if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(gameObject)))
                    {
                        hasValidObjects = true; // Scene GameObjects are valid
                    }
                    else
                    {
                        hasValidObjects = true; // Prefabs are also valid
                    }
                }
                else
                {
                    // For non-GameObjects, check if they're valid assets
                    string assetPath = AssetDatabase.GetAssetPath(obj);
                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        hasValidObjects = true;
                    }
                }
            }
            
            return hasValidObjects;
        }

        private void AddAssetToTodoItem(TodoItem item, Object asset)
        {
            TodoData.Instance?.AddAssetToTodoItem(item, asset);
        }


        private void RefreshAssetReferences(TodoItem item, VisualElement assetsContainer, ScrollView assetsList)
        {
            assetsList.Clear();
            
            bool hasAnyReferences = item.assetGUIDs.Count > 0 || item.gameObjectGlobalIds.Count > 0;
            
            if (hasAnyReferences)
            {
                assetsContainer.style.display = DisplayStyle.Flex;
                
                // Add asset references
                foreach (var guid in item.assetGUIDs)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                        if (asset != null)
                        {
                            var assetElement = CreateAssetReferenceElement(asset, item, guid, false);
                            assetsList.Add(assetElement);
                        }
                    }
                }
                
                // Add GameObject references
                for (int i = 0; i < item.gameObjectGlobalIds.Count; i++)
                {
                    var globalId = item.gameObjectGlobalIds[i];
                    var gameObject = TodoData.Instance?.GetGameObjectFromGlobalId(globalId);
                    
                    if (gameObject != null)
                    {
                        var gameObjectElement = CreateGameObjectReferenceElement(gameObject, item, i);
                        assetsList.Add(gameObjectElement);
                    }
                    else if (i < item.gameObjectNames.Count)
                    {
                        // Show missing GameObject
                        var missingElement = CreateMissingGameObjectReferenceElement(item.gameObjectNames[i], item, i);
                        assetsList.Add(missingElement);
                    }
                }
            }
            else
            {
                assetsContainer.style.display = DisplayStyle.None;
            }
        }

        private VisualElement CreateAssetReferenceElement(Object asset, TodoItem item, string guid, bool isGameObject)
        {
            var assetTemplate = Resources.Load<VisualTreeAsset>("UI/AssetReference");
            var assetElement = assetTemplate.CloneTree();
            
            var button = assetElement.Q<Button>("AssetReference");
            var icon = assetElement.Q<VisualElement>("Icon");
            var nameLabel = assetElement.Q<Label>("Name");
            
            nameLabel.text = asset.name;
            button.tooltip = AssetDatabase.GetAssetPath(asset);
            
            var iconTexture = AssetPreview.GetMiniThumbnail(asset);
            if (iconTexture != null)
            {
                icon.style.backgroundImage = new StyleBackground(iconTexture);
            }
            
            button.clicked += () => SelectAsset(asset);
            SetupAssetContextMenu(button, item, guid, true);
            
            return assetElement;
        }

        private VisualElement CreateGameObjectReferenceElement(GameObject gameObject, TodoItem item, int index)
        {
            var assetTemplate = Resources.Load<VisualTreeAsset>("UI/AssetReference");
            var assetElement = assetTemplate.CloneTree();

            var button = assetElement.Q<Button>("AssetReference");
            var icon = assetElement.Q<VisualElement>("Icon");
            var nameLabel = assetElement.Q<Label>("Name");

            string sceneName = gameObject.scene.name;
            nameLabel.text = $"{gameObject.name} ({sceneName})";
            button.tooltip = $"Scene GameObject: {sceneName}";

            var iconTexture = GetGameObjectIcon(gameObject);
            if (iconTexture != null)
            {
                icon.style.backgroundImage = new StyleBackground(iconTexture);
            }

            button.clicked += () => SelectGameObject(gameObject);
            button.SetEnabled(true);
            SetupAssetContextMenu(button, item, index.ToString(), false);

            return assetElement;
        }

        private VisualElement CreateMissingGameObjectReferenceElement(string gameObjectName, TodoItem item, int index)
        {
            var assetTemplate = Resources.Load<VisualTreeAsset>("UI/AssetReference");
            var assetElement = assetTemplate.CloneTree();

            var button = assetElement.Q<Button>("AssetReference");
            var icon = assetElement.Q<VisualElement>("Icon");
            var nameLabel = assetElement.Q<Label>("Name");

            string sceneName = index < item.gameObjectScenePaths.Count ?
                System.IO.Path.GetFileNameWithoutExtension(item.gameObjectScenePaths[index]) :
                "Unknown";

            nameLabel.text = $"{gameObjectName} ({sceneName})";
            button.tooltip = "Scene not currently loaded or GameObject not available";

            button.SetEnabled(false);
            SetupAssetContextMenu(button, item, index.ToString(), false);

            return assetElement;
        }

        private Texture2D GetGameObjectIcon(GameObject gameObject)
        {
            if (gameObject == null) return null;
            
            var components = gameObject.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component == null) continue;
                if (component is Transform) continue;
                
                var icon = AssetPreview.GetMiniThumbnail(component);
                if (icon != null) return icon;
            }
            
            return AssetPreview.GetMiniThumbnail(gameObject);
        }

        private void SetupAssetContextMenu(Button assetButton, TodoItem item, string identifier, bool isAsset)
        {
            assetButton.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button == 1) // Right click
                {
                    ShowAssetContextMenu(item, identifier, isAsset);
                    evt.StopPropagation();
                }
            });
        }

        private void ShowAssetContextMenu(TodoItem item, string identifier, bool isAsset)
        {
            GenericMenu menu = new GenericMenu();
            
            menu.AddItem(new GUIContent("Remove from Todo"), false, () => 
            {
                if (isAsset)
                {
                    RemoveAssetFromTodoItem(item, identifier);
                }
                else
                {
                    RemoveGameObjectFromTodoItem(item, int.Parse(identifier));
                }
            });
            
            menu.ShowAsContext();
        }

        private void SelectAsset(Object asset)
        {
            if (asset is GameObject gameObject)
            {
                Selection.activeGameObject = gameObject;
            }
            else
            {
                Selection.activeObject = asset;
            }
            EditorGUIUtility.PingObject(asset);
        }

        private void SelectGameObject(GameObject gameObject)
        {
            Selection.activeGameObject = gameObject;
            EditorGUIUtility.PingObject(gameObject);
        }

        private void RemoveAssetFromTodoItem(TodoItem item, string guid)
        {
            TodoData.Instance?.RemoveAssetFromTodoItem(item, guid);
            RefreshUI();
        }

        private void RemoveGameObjectFromTodoItem(TodoItem item, int index)
        {
            TodoData.Instance?.RemoveGameObjectFromTodoItem(item, index);
            RefreshUI();
        }
    }
}