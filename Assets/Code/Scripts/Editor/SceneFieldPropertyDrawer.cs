using Code.Scripts.Source.Types;
using Code.Scripts.Utils;
using UnityEditor;
using UnityEngine;

namespace Code.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(SceneField))]
    public class SceneFieldPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty sceneAssetProperty = property.FindPropertyRelative("_sceneAsset");

            EditorGUI.PropertyField(position, sceneAssetProperty, new GUIContent { text = label.text });
        }
    }
}
