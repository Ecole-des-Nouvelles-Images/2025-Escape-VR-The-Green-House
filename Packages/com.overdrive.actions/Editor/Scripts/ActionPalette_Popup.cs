using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UIElements;
using Overdrive.Framework;

namespace Overdrive.Actions
{
    /// <summary>
    /// Displays action palette in a popup at mouse position.
    /// Lists available actions (menu items and shortcuts) and allows filtering and execution.
    /// </summary>
    [InitializeOnLoad]
    public static class ActionPalette_Popup
    {
        private static ActionPaletteInstance currentInstance;

        static ActionPalette_Popup()
        {
            OverdrivePopup.RegisterContent("ActionPalette", CreatePopupContent, GetSearchFieldForFocus);
        }

        /// <summary>
        /// Raised when favorites are updated.
        /// </summary>
        public static event Action FavoritesUpdated;

        public static TextField GetSearchField()
        {
            return currentInstance?.GetSearchFieldInternal();
        }

        [Shortcut("Overdrive/Toggle QuickActions Palette", KeyCode.A, ShortcutModifiers.None)]
        public static void TogglePopup(ShortcutArguments args)
        {
            Event.current?.Use();

            if (OverdrivePopup.IsActive("ActionPalette"))
            {
                OverdrivePopup.Close("ActionPalette");
                return;
            }

            if (SceneView.lastActiveSceneView != null && EditorWindow.focusedWindow != SceneView.lastActiveSceneView)
            {
                SceneView.lastActiveSceneView.Focus();
            }

            Vector2 mousePos = Event.current != null ? Event.current.mousePosition : new Vector2(100, 100);
            OverdrivePopup.Show("ActionPalette", mousePos);
        }

        private static VisualElement CreatePopupContent()
        {
            currentInstance = new ActionPaletteInstance();
            return currentInstance.CreateContent();
        }

        private static VisualElement GetSearchFieldForFocus()
        {
            return currentInstance?.GetSearchFieldInternal();
        }

        private sealed class ActionPaletteInstance
        {
            private VisualElement _root;
            private ListView actionListView;
            private List<MenuCommandEntry> filteredCommands = new List<MenuCommandEntry>();
            private VisualTreeAsset listItemTemplate;
            private List<MenuCommandEntry> menuCommands = new List<MenuCommandEntry>();
            private VisualTreeAsset overlayTemplate;
            private TextField searchField;
            public VisualElement CreateContent()
            {
                //// var t1 = System.Diagnostics.Stopwatch.GetTimestamp();
              
                _root = new VisualElement();

                // Try to load the normal UI, if it fails show placeholder
                if (!TryInitializeUI())
                {
                    CreatePlaceholderUI();
                }
                else
                {
                    // Set up global keyboard handling for auto-typing to search field
                    SetupGlobalKeyboardHandling();
                }

                //// var t2 = System.Diagnostics.Stopwatch.GetTimestamp();
                //// Debug.LogFormat("ActionPalette: CreateContent takes {0}ms", TimeSpan.FromTicks(t2 - t1).TotalMilliseconds);
                return _root;
            }

            public TextField GetSearchFieldInternal()
            {
                return searchField;
            }

            private static bool ContainsWholeWord(string text, string word)
            {
                // Use regex to match whole words only
                return System.Text.RegularExpressions.Regex.IsMatch(
                    text,
                    @"\b" + System.Text.RegularExpressions.Regex.Escape(word) + @"\b",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
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

            private void CreatePlaceholderUI()
            {
                var container = new VisualElement();
                container.style.paddingTop = 10;
                container.style.paddingLeft = 10;
                container.style.paddingRight = 10;
                container.style.paddingBottom = 10;
                container.style.minWidth = 200;

                var titleLabel = new Label("Action Palette");
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

            private void FilterActions(string search)
            {
                bool hasSelection = Selection.activeGameObject != null;

                IEnumerable<MenuCommandEntry> filtered = menuCommands;
                if (!hasSelection)
                {
                    filtered = filtered.Where(cmd =>
                        !ContainsWholeWord(cmd.Label, "selected")
                        && !ContainsWholeWord(cmd.Label, "child")
                        && !ContainsWholeWord(cmd.Label, "parent")
                    );
                }

                if (!string.IsNullOrEmpty(search))
                {
                    filtered = filtered
                        .Where(cmd => FuzzySearchHelper.FuzzyMatch(search, cmd.Label))
                        .OrderByDescending(cmd => FuzzySearchHelper.FuzzyMatchScore(search, cmd.Label));
                }

                filteredCommands = filtered.ToList();
                actionListView.itemsSource = filteredCommands;
                actionListView.RefreshItems();

                // Always select first item when filter changes
                if (filteredCommands.Count > 0)
                {
                    SetListSelection(0);
                }
                else
                {
                    actionListView.selectedIndex = -1;
                }
            }

            private void InitializeUIElements(VisualElement root)
            {
                /*
                // Hide the overlay header
                root.RegisterCallback<GeometryChangedEvent>(evt =>
                {
                    // Walk up to find #overlay-content
                    VisualElement current = root;
                    while (current != null)
                    {
                        if (current.name == "overlay-content")
                        {
                            // Found overlay-content, now find its sibling with class overlay-header
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

                // Setup search field callbacks
                searchField.focusable = true;
                searchField.RegisterValueChangedCallback(evt => FilterActions(evt.newValue));
                searchField.RegisterCallback<KeyDownEvent>(OnSearchFieldKeyDown);

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
                                FavoritesUpdated?.Invoke();
                                actionListView.Rebuild();
                            }
                        };

                        return actionRow;
                    }

                    // Fail out if template not found
                    Debug.LogError("Failed to load ActionListItem.uxml");
                    var container = new VisualElement { name = "fail" };
                    var label = new Label { text = "Error loading item template" };
                    container.Add(label);
                    return container;
                };
                actionListView.bindItem = (element, index) =>
                {
                    var pathLabel = element.Q<Label>("PathLabel");
                    var actionLabel = element.Q<Label>("ActionLabel");
                    var favoriteToggle = element.Q<Button>("FavoriteToggle");
                    var command = filteredCommands[index];
                    string fullLabel = command.Label;

                    if (fullLabel.Contains(" > "))
                    {
                        string[] parts = fullLabel.Split(new string[] { " > " }, StringSplitOptions.None);
                        if (parts.Length >= 2)
                        {
                            string actionName = parts[parts.Length - 1];
                            string pathPortion = string.Join(" > ", parts.Take(parts.Length - 1));
                            pathLabel.text = pathPortion + " > ";
                            actionLabel.text = actionName;
                        }
                        else
                        {
                            pathLabel.text = "";
                            actionLabel.text = fullLabel;
                        }
                    }
                    else
                    {
                        pathLabel.text = "";
                        actionLabel.text = fullLabel;
                    }

                    bool isFavorite = FavoriteActions.GetFavorites().Contains(command.Path);
                    favoriteToggle.RemoveFromClassList("favorite-on");
                    favoriteToggle.RemoveFromClassList("favorite-off");
                    favoriteToggle.AddToClassList(isFavorite ? "favorite-on" : "favorite-off");

                    // Store command path in userData to access in click handler
                    favoriteToggle.userData = command.Path;
                };

                actionListView.unbindItem = (element, index) =>
                {
                    var favoriteToggle = element.Q<Button>("FavoriteToggle");
                    favoriteToggle.userData = null;
                };
                actionListView.selectionType = SelectionType.Single;
                actionListView.RegisterCallback<KeyDownEvent>(OnListKeyDown);
                actionListView.RegisterCallback<MouseDownEvent>(evt =>
                {
                    if (actionListView.selectedIndex >= 0)
                    {
                        ExecuteMenuItem(filteredCommands[actionListView.selectedIndex]);
                        OverdrivePopup.Close("ActionPalette");
                    }
                });

                LoadMenuActions();
                FilterActions("");
            }

            private bool IsTypingCharacter(KeyDownEvent evt)
            {
                // Check if this is a typing character (letter, number, space, backspace, etc.)
                KeyCode key = evt.keyCode;

                // Letters and numbers
                if ((key >= KeyCode.A && key <= KeyCode.Z) ||
                    (key >= KeyCode.Alpha0 && key <= KeyCode.Alpha9) ||
                    (key >= KeyCode.Keypad0 && key <= KeyCode.Keypad9))
                {
                    return true;
                }

                // Common typing characters
                switch (key)
                {
                    case KeyCode.Space:
                    case KeyCode.Backspace:
                    case KeyCode.Delete:
                    case KeyCode.Period:
                    case KeyCode.Comma:
                    case KeyCode.Semicolon:
                    case KeyCode.Quote:
                    case KeyCode.Minus:
                    case KeyCode.Plus:
                    case KeyCode.Equals:
                    case KeyCode.LeftBracket:
                    case KeyCode.RightBracket:
                    case KeyCode.Backslash:
                    case KeyCode.Slash:
                        return true;
                }

                return false;
            }

            private void LoadMenuActions()
            {
                menuCommands.Clear();

                string[] gameObjectMenus = Unsupported.GetSubmenus("GameObject");
                foreach (var menuItem in gameObjectMenus)
                {
                    menuCommands.Add(new MenuCommandEntry(menuItem, menuItem.Replace("/", " > "), false));
                }

                string[] editMenus = Unsupported.GetSubmenus("Edit");
                foreach (var menuItem in editMenus)
                {
                    if (menuItem.Contains("/Find") ||
                        menuItem.Contains("/Search/") ||
                        menuItem.Contains("/Sign in") ||
                        menuItem.Contains("/Sign out") ||
                        menuItem.Contains("/Graphics Tier/") ||
                        menuItem.Contains("/Rendering/") ||
                        menuItem.StartsWith("Edit/Selection/", StringComparison.Ordinal))
                    {
                        continue;
                    }
                    menuCommands.Add(new MenuCommandEntry(menuItem, menuItem.Replace("/", " > "), false));
                }

                string[] overdriveMenus = Unsupported.GetSubmenus("Tools/Overdrive Actions");
                foreach (var menuItem in overdriveMenus)
                {
                    menuCommands.Add(new MenuCommandEntry(menuItem, menuItem.Replace("/", " > "), false));
                }

                var allowedPrefixes = new[]
                {
                    "Scene View/", "Overlays/", "3D Viewport/", "Grid/", "Hierarchy View/",
                    "ParticleSystem/", "Scene Picking/", "Scene Visibility/", "Snap/", "Splines/",
                    "Stage/", "Terrain/", "Tools/", "TrailRenderer/", "Transform/", "Window/", "Overdrive/",
                    "Main Menu/Component/Add"
                };

                var shortcutIds = ShortcutManager.instance.GetAvailableShortcutIds();

                foreach (var shortcutId in shortcutIds)
                {
                    foreach (var prefix in allowedPrefixes)
                    {
                        if (shortcutId.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        {
                            menuCommands.Add(new MenuCommandEntry(shortcutId, shortcutId.Replace("/", " > "), true));
                            break;
                        }
                    }
                }

                menuCommands = menuCommands.Distinct(new MenuCommandEntryComparer()).ToList();

                // Deduplicate by command name (ignore path), prefer menu items over shortcuts
                var commandsByName = new Dictionary<string, MenuCommandEntry>();
                foreach (var command in menuCommands)
                {
                    // Extract the final command name from the label (after last " > ")
                    var lastIndex = command.Label.LastIndexOf(" > ");
                    string commandName = lastIndex != -1
                        ? command.Label.Substring(lastIndex + 3)
                        : command.Label;

                    if (commandsByName.TryGetValue(commandName, out var existingValue))
                    {
                        // Prefer menu items (IsShortcut = false) over shortcuts (IsShortcut = true)
                        if (existingValue.IsShortcut && !command.IsShortcut)
                        {
                            commandsByName[commandName] = command;
                        }
                        // If both are the same type, keep the first one found
                    }
                    else
                    {
                        commandsByName[commandName] = command;
                    }
                }

                menuCommands = commandsByName.Values.ToList();

        
            }

            private void OnGlobalKeyDown(KeyDownEvent evt)
            {
                if (searchField == null || actionListView == null)
                    return;

                // Handle special keys that should always work regardless of focus
                if (evt.keyCode == KeyCode.Escape)
                {
                    OverdrivePopup.Close("ActionPalette");
                    evt.StopPropagation();
                    return;
                }

                if (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter)
                {
                    // Enter should always execute the selected item
                    if (filteredCommands.Count > 0 && actionListView.selectedIndex >= 0)
                    {
                        ExecuteMenuItem(filteredCommands[actionListView.selectedIndex]);
                        OverdrivePopup.Close("ActionPalette");
                        evt.StopPropagation();
                    }
                    return;
                }

                // Handle arrow keys to control list navigation regardless of focus
                if (evt.keyCode == KeyCode.DownArrow)
                {
                    if (filteredCommands.Count > 0)
                    {
                        int newIndex = actionListView.selectedIndex < 0 ? 0 :
                                      Mathf.Min(actionListView.selectedIndex + 1, filteredCommands.Count - 1);
                        SetListSelection(newIndex);
                    }
                    evt.StopPropagation();
                    return;
                }

                if (evt.keyCode == KeyCode.UpArrow)
                {
                    if (filteredCommands.Count > 0)
                    {
                        int newIndex = actionListView.selectedIndex <= 0 ? 0 : actionListView.selectedIndex - 1;
                        SetListSelection(newIndex);
                    }
                    evt.StopPropagation();
                    return;
                }

                // For typing characters, auto-focus search field and let the character through
                if (IsTypingCharacter(evt))
                {
                    if (searchField.focusController?.focusedElement != searchField)
                    {
                        searchField.Focus();

                        // If it's a regular character (not backspace/delete), clear selection 
                        // so new typing replaces any existing text
                        if (evt.keyCode != KeyCode.Backspace && evt.keyCode != KeyCode.Delete)
                        {
                            searchField.schedule.Execute(() =>
                            {
                                if (string.IsNullOrEmpty(searchField.text))
                                {
                                    // Field is empty, cursor will be at start naturally
                                }
                                else
                                {
                                    // Select all so typing will replace
                                    searchField.SelectAll();
                                }
                            });
                        }
                        // Don't stop propagation - let the character be typed into the search field
                    }
                }
            }

            private void OnListKeyDown(KeyDownEvent evt)
            {
                // Global key handler takes care of navigation, execution, and escape
                // List view can focus on list-specific behavior if needed

                // Let global handler manage all keyboard input
            }

            private void OnSearchFieldKeyDown(KeyDownEvent evt)
            {
                // Global key handler takes care of navigation, execution, and escape
                // This handler can focus on search-specific behavior if needed

                // Let global handler manage arrow keys, enter, and escape
                // Search field just needs to handle its text input naturally
            }

            private void SetListSelection(int index)
            {
                if (actionListView == null || filteredCommands == null) return;

                index = Mathf.Clamp(index, 0, filteredCommands.Count - 1);
                actionListView.selectedIndex = index;

                // Ensure the selected item is visible
                if (index >= 0 && index < filteredCommands.Count)
                {
                    actionListView.ScrollToItem(index);
                }
            }

            private void SetupGlobalKeyboardHandling()
            {
                // Add global key handler to root element to catch all typing
                _root.RegisterCallback<KeyDownEvent>(OnGlobalKeyDown, TrickleDown.TrickleDown);
            }

            private bool TryInitializeUI()
            {
                // Load templates
                overlayTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Packages/com.overdrive.actions/Editor/Resources/UI/ActionPalette_Popup.uxml");
                listItemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Packages/com.overdrive.actions/Editor/Resources/UI/ActionListItem.uxml");

                if (overlayTemplate == null || listItemTemplate == null)
                {
                    return false; // Assets not ready yet
                }

                VisualElement root = overlayTemplate.Instantiate();
                VisualElement rootContainer = root.Q<VisualElement>("RootContainer");

                if (rootContainer == null)
                {
                    return false;
                }

                // Get references from UXML
                searchField = rootContainer.Q<TextField>("SearchField");
                actionListView = rootContainer.Q<ListView>("ActionListView");

                if (searchField == null || actionListView == null)
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
