using System;
using System.Collections.Generic;
using System.Text;

namespace Company.UI
{
    public abstract class MenuItem
    {
        public string Name { get; }
        public Action? DisplayContext { get; }

        public MenuItem(string name, Action? displayContext = null)
        {
            Name = name;
            DisplayContext = displayContext;
        }

        public abstract void Execute(bool clearBefore = true, bool clearAfter = true);
    }
}
