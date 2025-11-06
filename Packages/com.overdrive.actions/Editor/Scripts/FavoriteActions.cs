using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Overdrive.Actions
{
    /// <summary>
    /// Manages favorite actions using EditorPrefs.
    /// </summary>
    public static class FavoriteActions
    {
        public const string FavoritesKey = "FavoriteActions";

        private static readonly List<string> favoriteActions = new List<string>();

        static FavoriteActions()
        {
            LoadFavorites();
        }

        public static void AddFavorite(string actionPath)
        {
            if (!favoriteActions.Contains(actionPath))
            {
                favoriteActions.Add(actionPath);
                SaveFavorites();
            }
        }

        public static List<string> GetFavorites() => favoriteActions;

        public static void LoadFavorites()
        {
            // Ensure the default JSON is a valid object.
            string json = EditorPrefs.GetString(FavoritesKey, "{\"favorites\":[]}");
            var wrapper = JsonUtility.FromJson<FavoriteListWrapper>(json);

            favoriteActions.Clear();
            if (wrapper != null)
            {
                favoriteActions.AddRange(wrapper.favorites);
            }
        }

        public static void RemoveFavorite(string actionPath)
        {
            if (favoriteActions.Contains(actionPath))
            {
                favoriteActions.Remove(actionPath);
                SaveFavorites();
            }
        }

        public static void SaveFavorites()
        {
            var wrapper = new FavoriteListWrapper { favorites = favoriteActions };
            string json = JsonUtility.ToJson(wrapper);
            EditorPrefs.SetString(FavoritesKey, json);
        }

        public static void ToggleFavorite(string actionPath)
        {
            if (favoriteActions.Contains(actionPath))
                favoriteActions.Remove(actionPath);
            else
                favoriteActions.Add(actionPath);

            SaveFavorites();
        }

        [Serializable]
        private sealed class FavoriteListWrapper
        {
            public List<string> favorites = new List<string>();
        }
    }
}
