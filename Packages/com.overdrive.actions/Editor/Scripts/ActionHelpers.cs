using System.Collections.Generic;

namespace Overdrive.Actions
{
    /// <summary>
    /// Represents a menu or shortcut command.
    /// </summary>
    public sealed class MenuCommandEntry
    {
        public string Path { get; }

        public string Label { get; }

        public bool IsShortcut { get; }

        public MenuCommandEntry(string path, string label, bool isShortcut)
        {
            this.Path = path ?? throw new System.ArgumentNullException(nameof(path));
            Label = label ?? throw new System.ArgumentNullException(nameof(label));
            IsShortcut = isShortcut;
        }
    }

    /// <summary>
    /// Compares MenuCommandEntry objects based on their Path.<br/>
    /// Has no fields, is not stored. No need to be a class.
    /// </summary>
    public struct MenuCommandEntryComparer : IEqualityComparer<MenuCommandEntry>
    {
        public readonly bool Equals(MenuCommandEntry x, MenuCommandEntry y) => x.Path == y.Path;

        public readonly int GetHashCode(MenuCommandEntry obj)
        {
            return obj.Path.GetHashCode();
        }
    }
}
