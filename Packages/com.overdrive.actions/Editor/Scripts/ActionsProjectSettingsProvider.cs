using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Overdrive.Actions
{
    /// <summary>
    /// Project Settings page (Edit > Project Settings > Overdrive > Actions) for project-wide settings.
    /// </summary>
    internal static class ActionsProjectSettingsProvider
    {
        private static Button moreInfoButton;

        [SettingsProvider]
        public static SettingsProvider CreateActionsProjectSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider("Project/Overdrive/Actions", SettingsScope.Project)
            {
                label = "Actions",
                activateHandler = (searchContext, rootElement) =>
                {
                    const string pathToPackage = "Packages/com.overdrive.actions";
                    const string pathToUI = pathToPackage + "/Editor/Resources/UI";
                    VisualTreeAsset settings = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(pathToUI + "/Actions_ProjectSettings.uxml");

                    if (settings != null)
                    {
                        TemplateContainer settingsContainer = settings.Instantiate();

                        moreInfoButton = settingsContainer.Q<Button>("Btn_Actions-Web");
                        if (moreInfoButton != null)
                        {
                            moreInfoButton.clicked += () =>
                            {
                                Application.OpenURL("https://www.overdrivetoolset.com/actions");
                            };
                        }

                        rootElement.Add(settingsContainer);
                    }
                    else
                    {
                        Debug.LogError("ActionsProjectSettingsProvider: Could not load Actions_ProjectSettings.uxml from " + pathToUI);
                        var errorLabel = new Label("Actions_ProjectSettings.uxml not found");
                        errorLabel.style.color = Color.red;
                        rootElement.Add(errorLabel);
                    }
                },
                deactivateHandler = OnDeactivate,
                keywords = new HashSet<string>(new[] { "Actions", "Palette", "Commands" })
            };

            return provider;
        }

        private static void OnDeactivate()
        {
            moreInfoButton = null;
        }
    }
}
