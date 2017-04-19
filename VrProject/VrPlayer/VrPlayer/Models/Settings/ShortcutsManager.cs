using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace VrPlayer.Models.Settings
{
    public class ShortcutsManager
    {
        private readonly IDictionary<Key, ICommand> _shortcuts;

        public ShortcutsManager()
        {
            _shortcuts = new Dictionary<Key, ICommand>();
        }

        public void Register(Key key, ICommand command)
        {
            foreach (var item in _shortcuts.Where(kvp => kvp.Value == command).ToList())
            {
                _shortcuts.Remove(item.Key);
            }

            if (_shortcuts.ContainsKey(key))
                _shortcuts.Remove(key);

            _shortcuts.Add(key, command);
        }

        public void Execute(Key key)
        {
            if (!_shortcuts.ContainsKey(key)) return;
            if (_shortcuts[key] != null)
                _shortcuts[key].Execute(null);
        }

        public bool Contains(Key key)
        {
            return _shortcuts.Keys.Contains(key);
        }

        public void Clear()
        {
            _shortcuts.Clear();
        }
    }
}