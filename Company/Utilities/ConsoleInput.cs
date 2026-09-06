using System;

namespace Company.Utilities
{
    public static class ConsoleInput
    {
        public static T Read<T>(string prompt)
            where T : IParsable<T>
        {
            Console.Write(prompt);

            while (true)
            {
                string? input = Console.ReadLine();

                if (T.TryParse(input, null, out T value))
                {
                    return value;
                }

                Console.Write("Invalid input type. Try again: ");
            }
        }

        public static T? ReadOptional<T>(string prompt)
            where T : IParsable<T>
        {
            Console.Write(prompt);

            while (true)
            {
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    return default;
                }

                if (T.TryParse(input, null, out T value))
                {
                    return value;
                }

                Console.Write("Invalid input type. Try again: ");
            }
        }

        public static T Select<T>(List<T> options)
        {
            int choice;
            do
            {
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {options[i]}");
                }

                choice = Read<int>("Choice: ")!;
            } while (choice < 1 || choice > options.Count);

            return options[choice - 1];
        }

        public static T? OptionalSelect<T>(List<T> options)
        {
            int choice;
            do
            {
                Console.WriteLine("0. Exit");
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {options[i]}");
                }

                choice = Read<int>("Choice: ")!;
            } while (choice < 0 || choice > options.Count);

            return choice == 0 ? default : options[choice - 1];
        }

        public static List<T>? OptionalSelectMultiple<T>(List<T> options)
        {
            var selectedOptions = new HashSet<T>();
            int choice;

            while (true)
            {
                Console.WriteLine("0. Exit");
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {options[i]}");
                }

                choice = Read<int>("Choice: ")!;
                if (choice == 0)
                {
                    break;
                }
                if (choice > 0 && choice <= options.Count)
                {
                    if (selectedOptions.Contains(options[choice - 1]))
                    {
                        Console.WriteLine("Option already selected. Please choose another.");
                        continue;
                    }
                    selectedOptions.Add(options[choice - 1]);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }

            return selectedOptions.Count > 0 ? selectedOptions.ToList() : null;
        }
    }
}