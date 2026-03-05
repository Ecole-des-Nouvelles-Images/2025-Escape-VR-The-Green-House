using System;
using UnityEditor;

namespace Overdrive.Todo
{
    public static class TodoUserPreferences
    {
        public static event Action<string, bool> onPreferenceChanged;

        public const string ShowDoneItemsPref = "Overdrive.Todo.ShowDoneItems";
        
        public static bool GetShowDoneItems()
        {
            return EditorPrefs.GetBool(ShowDoneItemsPref, true);
        }
        
        public static void SetShowDoneItems(bool value)
        {
            if (EditorPrefs.GetBool(ShowDoneItemsPref, true) == value)
                return;
            EditorPrefs.SetBool(ShowDoneItemsPref, value);
            onPreferenceChanged?.Invoke(ShowDoneItemsPref, value);
        }
    }
}