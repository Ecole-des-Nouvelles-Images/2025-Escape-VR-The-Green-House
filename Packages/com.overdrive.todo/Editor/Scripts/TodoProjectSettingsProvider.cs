using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

namespace Overdrive.Todo
{
    internal static class TodoProjectSettingsProvider
    {
        private static Button clearAllButton;
        private static Button moreInfoButton;

        [SettingsProvider]
        public static SettingsProvider CreateTodoSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider("Project/Overdrive/Todo", SettingsScope.Project)
            {
                label = "Todo",
                activateHandler = (searchContext, rootElement) =>
                {
                    const string pathToOverdrive = "Packages/com.overdrive.todo";
                    const string pathToUI = pathToOverdrive + "/Editor/Resources/UI";
                    VisualTreeAsset settings = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(pathToUI + "/Todo_ProjectSettings.uxml");

                    if (settings != null)
                    {
                        TemplateContainer settingsContainer = settings.Instantiate();

                        // Setup more info button
                        moreInfoButton = settingsContainer.Q<Button>("Btn_Todo-Web");
                        if (moreInfoButton != null)
                        {
                            moreInfoButton.clicked += () =>
                            {
                                Application.OpenURL("https://www.overdrivetoolset.com/todo");
                            };
                        }

                        // Setup clear all button
                        clearAllButton = settingsContainer.Q<Button>("clearAllButton");
                        if (clearAllButton != null)
                        {
                            clearAllButton.clicked += () =>
                            {
                                if (EditorUtility.DisplayDialog("Clear All Todo Items", 
                                    "Are you sure you want to delete all todo items? This action cannot be undone.", 
                                    "Delete All", "Cancel"))
                                {
                                    TodoData.Instance?.ClearAll();
                                    RefreshAllTodoWindows();
                                    Debug.Log("[Todo] All todo items have been cleared.");
                                }
                            };
                        }

                        rootElement.Add(settingsContainer);
                    }
                    else
                    {
                        Debug.LogError("TodoProjectSettingsProvider: Could not load Todo_ProjectSettings.uxml from " + pathToUI);
                        var errorLabel = new Label("Todo_ProjectSettings.uxml not found");
                        errorLabel.style.color = Color.red;
                        rootElement.Add(errorLabel);
                    }
                },
                deactivateHandler = OnDeactivate,
                keywords = new HashSet<string>(new[] { "Todo", "clear", "delete", "items" })
            };

            return provider;
        }

        private static void OnDeactivate()
        {
            clearAllButton = null;
            moreInfoButton = null;
        }
        
        private static void RefreshAllTodoWindows()
        {
            var windows = Resources.FindObjectsOfTypeAll<TodoEditorWindow>();
            foreach (var window in windows)
            {
                if (window != null)
                {
                    window.RequestRefresh();
                }
            }
        }
    }
}