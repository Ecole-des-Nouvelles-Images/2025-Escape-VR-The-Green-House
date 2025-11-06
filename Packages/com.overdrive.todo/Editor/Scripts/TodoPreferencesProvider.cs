using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

namespace Overdrive.Todo
{
    internal static class TodoPreferencesProvider
    {
        private static Toggle showDoneItemsToggle;
        private static Button moreInfoButton;

        [SettingsProvider]
        public static SettingsProvider CreateTodoPreferencesProvider()
        {
            SettingsProvider provider = new SettingsProvider("Preferences/Overdrive/Todo", SettingsScope.User)
            {
                label = "Todo",
                activateHandler = (searchContext, rootElement) =>
                {
                    const string pathToOverdrive = "Packages/com.overdrive.todo";
                    const string pathToUI = pathToOverdrive + "/Editor/Resources/UI";
                    VisualTreeAsset settings = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(pathToUI + "/Todo_UserPreferences.uxml");

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

                        // Setup show done items toggle
                        showDoneItemsToggle = settingsContainer.Q<Toggle>("showDoneItemsToggle");
                        if (showDoneItemsToggle != null)
                        {
                            showDoneItemsToggle.value = TodoUserPreferences.GetShowDoneItems();
                            showDoneItemsToggle.RegisterValueChangedCallback(evt =>
                            {
                                TodoUserPreferences.SetShowDoneItems(evt.newValue);
                            });
                        }

                        rootElement.Add(settingsContainer);
                    }
                    else
                    {
                        Debug.LogError("TodoPreferencesProvider: Could not load Todo_UserPreferences.uxml from " + pathToUI);
                        var errorLabel = new Label("Todo_UserPreferences.uxml not found");
                        errorLabel.style.color = Color.red;
                        rootElement.Add(errorLabel);
                    }
                },
                deactivateHandler = OnDeactivate,
                keywords = new HashSet<string>(new[] { "Todo", "done", "completed", "items", "show" })
            };

            return provider;
        }

        private static void OnDeactivate()
        {
            showDoneItemsToggle = null;
            moreInfoButton = null;
        }
        
    }
}