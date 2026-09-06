using Company.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.UI
{
    public class SubMenuItem : MenuItem
    {
        public List<MenuItem> Items { get; init; }

        public SubMenuItem(string name, List<MenuItem> items, Action? displayContext = null) : base(name, displayContext)
        {
            Items = items;
        }

        public override void Execute(bool clearBefore = true, bool clearAfter = true)
        {
            if (clearBefore)
            {
                Console.Clear();
            }

            bool running = true;
            while (running)
            {
                DisplayContext?.Invoke();
                Console.WriteLine(Name);
                Console.WriteLine("0. Exit");
                for (int i = 0; i < Items.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {Items[i].Name}");
                }


                int choice = ConsoleInput.Read<int>("Choice: ");
                if (choice == 0)
                {
                    running = false;
                    continue;
                }
                if (choice < 0 || choice > Items.Count)
                {
                    Console.WriteLine("Invalid choice. Try again.");
                    continue;
                }

                var selectedItem = Items[choice - 1];
                selectedItem.Execute();
            }

            if (clearAfter)
            {
                Console.Clear();
            }
        }
    }
}
