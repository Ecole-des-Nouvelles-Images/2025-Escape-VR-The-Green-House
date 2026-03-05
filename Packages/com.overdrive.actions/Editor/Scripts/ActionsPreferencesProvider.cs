using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Overdrive.Actions
{
    /// <summary>
    /// A Preferences page (Edit > Preferences > Overdrive > Action Palette) that shows current favorites
    /// and allows managing them.
    /// </summary>
    internal static class ActionsPreferencesProvider
    {
        private static ScrollView favoritesListContainer;
        private static Button moreInfoButton;
        [SettingsProvider]
        public static SettingsProvider CreateActionsPreferencesProvider()
        {
            SettingsProvider provider = new SettingsProvider("Preferences/Overdrive/Actions", SettingsScope.User)
            {
                label = "Actions",
                activateHandler = (searchContext, rootElement) =>
                {
                    const string pathToPackage = "Packages/com.overdrive.actions";
                    const string pathToUI = pathToPackage + "/Editor/Resources/UI";
                    VisualTreeAsset settings = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(pathToUI + "/Actions_UserPreferences.uxml");

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

                        favoritesListContainer = settingsContainer.Q<ScrollView>("FavoritesListContainer");

                        UpdateFavoritesList();

                        rootElement.Add(settingsContainer);
                    }
                    else
                    {
                        Debug.LogError("ActionsPreferencesProvider: Could not load Actions_UserPreferences.uxml from " + pathToUI);
                        var errorLabel = new Label("Actions_UserPreferences.uxml not found");
                        errorLabel.style.color = Color.red;
                        rootElement.Add(errorLabel);
                    }
                },
                deactivateHandler = OnDeactivate,
                keywords = new HashSet<string>(new[] { "Actions", "Favorite", "Palette", "Commands" })
            };

            return provider;
        }

        private static void OnDeactivate()
        {
            moreInfoButton = null;
            favoritesListContainer = null;
        }

        private static void UpdateFavoritesList()
        {
            if (favoritesListContainer == null) return;

            favoritesListContainer.Clear();
            List<string> favorites = FavoriteActions.GetFavorites() ?? new List<string>();

            if (favorites.Count == 0)
            {
                var noFavoritesLabel = new Label("No favorite actions yet.");
                noFavoritesLabel.style.color = new Color(0.6f, 0.6f, 0.6f);
                noFavoritesLabel.style.marginTop = 4;
                noFavoritesLabel.style.marginBottom = 4;
                favoritesListContainer.Add(noFavoritesLabel);
                return;
            }

            foreach (var favorite in favorites)
            {
                var row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                row.style.marginTop = 2;
                row.style.marginBottom = 2;

                var label = new Label(favorite);
                label.style.flexGrow = 1;

                var removeButton = new Button(() =>
                {
                    FavoriteActions.RemoveFavorite(favorite);
                    UpdateFavoritesList();
                });
                removeButton.text = "Remove";
                removeButton.style.width = 60;

                row.Add(label);
                row.Add(removeButton);
                favoritesListContainer.Add(row);
            }
        }
    }
}
