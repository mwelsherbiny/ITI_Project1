using System;
using System.Collections.Generic;
using System.Text;

namespace Company.UI
{
    public class ActionMenuItem : MenuItem
    {
        public Action? DisplayContext { get; }
        public Action Action { get; }

        public ActionMenuItem(string name, Action action, Action? displayContext = null) : base(name, displayContext)
        {
            ArgumentNullException.ThrowIfNull(action);
            Action = action;
            DisplayContext = displayContext;
        }

        public override void Execute(bool clearBefore = true, bool clearAfter = true)
        {
            if (clearBefore)
            {
                Console.Clear();
            }
            DisplayContext?.Invoke();
            Action();
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
            if (clearAfter)
            {
                Console.Clear();
            }
        }
    }
}
