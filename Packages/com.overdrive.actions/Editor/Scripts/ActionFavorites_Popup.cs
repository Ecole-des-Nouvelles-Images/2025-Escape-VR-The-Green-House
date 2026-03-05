using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UIElements;
using Overdrive.Framework;

namespace Overdrive.Actions
{
    /// <summary>
    /// Displays favorite actions in a popup at mouse position.
    /// </summary>
    [InitializeOnLoad]
    public static class ActionFavorites_Popup
    {
        private static ActionFavoritesInstance currentInstance;

        static ActionFavorites_Popup()
        {
            OverdrivePopup.RegisterContent("ActionFavorites", CreatePopupContent, GetListViewForFocus);
        }

        [Shortcut("Overdrive/Toggle QuickActions Favorites", typeof(SceneView), KeyCode.Mouse1, ShortcutModifiers.Alt)]
        public static void TogglePopup(ShortcutArguments args)
        {
            Event.current?.Use();

            if (OverdrivePopup.IsActive("ActionFavorites"))
            {
                OverdrivePopup.Close("ActionFavorites");
                return;
            }

            if (SceneView.lastActiveSceneView != null && EditorWindow.focusedWindow != SceneView.lastActiveSceneView)
            {
                SceneView.lastActiveSceneView.Focus();
            }

            Vector2 mousePos = Event.current != null ? Event.current.mousePosition : new Vector2(100, 100);
            OverdrivePopup.Show("ActionFavorites", mousePos);
        }

        private static VisualElement CreatePopupContent()
        {
            currentInstance = new ActionFavoritesInstance();
            return currentInstance.CreateContent();
        }

        private static VisualElement GetListViewForFocus()
        {
            return currentInstance?.GetRootElementInternal();
        }

        private sealed class ActionFavoritesInstance
        {
            private readonly List<MenuCommandEntry> favoriteCommands = new List<MenuCommandEntry>();
            private VisualElement _root;
            private ListView actionListView;
            private VisualTreeAsset listItemTemplate;
            private VisualTreeAsset overlayTemplate;

            public VisualElement CreateContent()
            {
                _root = new VisualElement();

                if (!TryInitializeUI())
                {
                    CreatePlaceholderUI();
                }
                else
                {
                    SetupGlobalKeyboardHandling();
                }

                return _root;
            }

            public ListView GetListViewInternal()
            {
                return actionListView;
            }

            public VisualElement GetRootElementInternal()
            {
                return _root;
            }

            private void CreatePlaceholderUI()
            {
                var container = new VisualElement();
                container.style.paddingTop = 10;
                container.style.paddingLeft = 10;
                container.style.paddingRight = 10;
                container.style.paddingBottom = 10;
                container.style.minWidth = 200;

                var titleLabel = new Label("Action Favorites");
                titleLabel.style.fontSize = 14;
                titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                titleLabel.style.marginBottom = 8;
                container.Add(titleLabel);

                var infoLabel = new Label("UXML assets not yet loaded.\nClick 'Ready!' when Unity finishes importing.");
                infoLabel.style.whiteSpace = WhiteSpace.Normal;
                infoLabel.style.marginBottom = 12;
                container.Add(infoLabel);

                var readyButton = new Button(() =>
                {
                    _root.Clear();
                    if (TryInitializeUI())
                    {
                        _root.MarkDirtyRepaint();
                        _root.schedule.Execute(() =>
                        {
                            _root.MarkDirtyRepaint();
                        }).ExecuteLater(1);
                    }
                    else
                    {
                        CreatePlaceholderUI();
                    }
                });
                readyButton.text = "Ready!";
                readyButton.style.backgroundColor = new StyleColor(new Color(0.2f, 0.6f, 0.2f, 1f));
                container.Add(readyButton);

                _root.Add(container);
            }

            private static void ExecuteMenuItem(MenuCommandEntry command)
            {
                if (!command.IsShortcut)
                {
                    EditorApplication.delayCall += () => EditorApplication.ExecuteMenuItem(command.Path);
                }
                else
                {
                    EditorApplication.delayCall += () => ShortcutSimulator.SimulateShortcut(command.Path);
                }
            }

            private void InitializeUIElements(VisualElement root)
            {
                /*
                // Hide the overlay header
                    root.RegisterCallback<GeometryChangedEvent>(evt =>
                {
                    VisualElement current = root;
                    while (current != null)
                    {
                        if (current.name == "overlay-content")
                        {
                            var parent = current.parent;
                            if (parent != null)
                            {
                                var header = parent.Q(className: "overlay-header");
                                if (header != null)
                                {
                                    header.style.display = DisplayStyle.None;
                                }
                            }
                            break;
                        }
                        current = current.parent;
                    }
                }, TrickleDown.TrickleDown);
                */

                // Focus lost is handled by OverdrivePopup framework

                // Setup list view
                actionListView.makeItem = () =>
                {
                    if (listItemTemplate != null)
                    {
                        var item = listItemTemplate.Instantiate();
                        var actionRow = item.Q<VisualElement>("ActionRow");
                        var favoriteToggle = actionRow.Q<Button>("FavoriteToggle");

                        favoriteToggle.clicked += () =>
                        {
                            string commandPath = favoriteToggle.userData as string;
                            if (!string.IsNullOrEmpty(commandPath))
                            {
                                FavoriteActions.ToggleFavorite(commandPath);
                                LoadFavorites();
                            }
                        };

                        return actionRow;
                    }

                    Debug.LogError("Failed to load FavoritesListItem.uxml");
                    var container = new VisualElement { name = "fail" };
                    var label = new Label { text = "Error loading item template" };
                    container.Add(label);
                    return container;
                };

                actionListView.bindItem = (element, index) =>
                {
                    var actionLabel = element.Q<Label>("ActionLabel");
                    var favoriteToggle = element.Q<Button>("FavoriteToggle");
                    var command = favoriteCommands[index];
                    string fullLabel = command.Label;

                    if (fullLabel.Contains(" > "))
                    {
                        string[] parts = fullLabel.Split(new string[] { " > " }, System.StringSplitOptions.None);
                        if (parts.Length >= 2)
                        {
                            string actionName = parts[parts.Length - 1];
                            actionLabel.text = actionName;
                        }
                        else
                        {
                            actionLabel.text = fullLabel;
                        }
                    }
                    else
                    {
                        actionLabel.text = fullLabel;
                    }

                    favoriteToggle.userData = command.Path;
                };

                actionListView.unbindItem = (element, index) =>
                {
                    var favoriteToggle = element.Q<Button>("FavoriteToggle");
                    favoriteToggle.userData = null;
                };

                actionListView.selectionType = SelectionType.Single;
                actionListView.focusable = false;
                actionListView.RegisterCallback<MouseDownEvent>(evt =>
                {
                    if (actionListView.selectedIndex >= 0)
                    {
                        ExecuteMenuItem(favoriteCommands[actionListView.selectedIndex]);
                        OverdrivePopup.Close("ActionFavorites");
                    }
                });

                LoadFavorites();
            }

            private void LoadFavorites()
            {
                favoriteCommands.Clear();
                List<string> favorites = FavoriteActions.GetFavorites();

                foreach (var actionPath in favorites)
                {
                    string label = actionPath.Replace("/", " > ");
                    bool isShortcut = !actionPath.StartsWith("GameObject") && !actionPath.StartsWith("Edit") && !actionPath.StartsWith("Tools");
                    favoriteCommands.Add(new MenuCommandEntry(actionPath, label, isShortcut));
                }

                actionListView.itemsSource = favoriteCommands;
                actionListView.RefreshItems();
                if (favoriteCommands.Count > 0)
                {
                    SetListSelection(0);
                }
            }

            private void OnGlobalKeyDown(KeyDownEvent evt)
            {
                if (actionListView == null || favoriteCommands == null)
                    return;

                // Handle special keys regardless of focus
                if (evt.keyCode == KeyCode.Escape)
                {
                    OverdrivePopup.Close("ActionFavorites");
                    evt.StopPropagation();
                    return;
                }

                if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                {
                    if (favoriteCommands.Count > 0 && actionListView.selectedIndex >= 0)
                    {
                        ExecuteMenuItem(favoriteCommands[actionListView.selectedIndex]);
                        OverdrivePopup.Close("ActionFavorites");
                        evt.StopPropagation();
                    }
                    return;
                }

                // Handle arrow keys for navigation
                if (evt.keyCode == KeyCode.DownArrow)
                {
                    if (favoriteCommands.Count > 0)
                    {
                        int currentIndex = actionListView.selectedIndex;
                        int newIndex = Mathf.Min(currentIndex + 1, favoriteCommands.Count - 1);
                        SetListSelection(newIndex);
                    }
                    evt.StopPropagation();
                    return;
                }

                if (evt.keyCode == KeyCode.UpArrow)
                {
                    if (favoriteCommands.Count > 0)
                    {
                        int currentIndex = actionListView.selectedIndex;
                        int newIndex = Mathf.Max(currentIndex - 1, 0);
                        SetListSelection(newIndex);
                    }
                    evt.StopPropagation();
                    return;
                }
            }

            private void SetListSelection(int index)
            {
                if (actionListView == null || favoriteCommands == null) return;

                index = Mathf.Clamp(index, 0, favoriteCommands.Count - 1);
                actionListView.selectedIndex = index;

                if (index >= 0 && index < favoriteCommands.Count)
                {
                    actionListView.ScrollToItem(index);
                }
            }

            private void SetupGlobalKeyboardHandling()
            {
                _root.focusable = true;
                _root.RegisterCallback<KeyDownEvent>(OnGlobalKeyDown, TrickleDown.TrickleDown);
            }

            private bool TryInitializeUI()
            {
                overlayTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Packages/com.overdrive.actions/Editor/Resources/UI/ActionFavorites_Popup.uxml");
                listItemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Packages/com.overdrive.actions/Editor/Resources/UI/FavoritesListItem.uxml");

                if (overlayTemplate == null || listItemTemplate == null)
                {
                    return false;
                }

                VisualElement root = overlayTemplate.Instantiate();

                actionListView = root.Q<ListView>("ActionListView");
                if (actionListView == null)
                {
                    return false;
                }

                _root.Add(root);
                InitializeUIElements(root);
                return true;
            }
        }
    }
}
