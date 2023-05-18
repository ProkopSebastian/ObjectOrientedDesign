using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projob_Projekt
{
    public class console
    {
        private static Dictionary<string, Action<string[]>> commandDictionary;
        public GameStore storeMain; // reference to GameStore prepared in main

        public console(GameStore store)
        {
            // Initialize the command dictionary
            commandDictionary = new Dictionary<string, Action<string[]>>
            {
                { "list", ListCommand },
                { "find", FindCommand },
                { "add", AddCommand },
                { "exit", ExitCommand }
            };
            this.storeMain = store;
            string command;
            do
            {
                Console.Write("Enter a command: ");
                command = Console.ReadLine();
                ProcessCommand(command);
            } while (command != "exit");

        }

        private static void ProcessCommand(string command)
        {
            string[] commandTokens = command.Split(' ');

            if (commandTokens.Length > 0)
            {
                string commandName = commandTokens[0].ToLower();

                if (commandDictionary.ContainsKey(commandName))
                {
                    string[] commandArgs = new string[commandTokens.Length - 1];
                    Array.Copy(commandTokens, 1, commandArgs, 0, commandArgs.Length);

                    commandDictionary[commandName].Invoke(commandArgs);
                }
                else
                {
                    Console.WriteLine("Invalid command.");
                }
            }
        }

        private void ListCommand(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: list <name_of_the_class>");
                return;
            }

            string className = args[0];

            Console.WriteLine($"Executing list command for class '{className}'");
            // Your code to list objects of the specified class
            // ...
            storeMain.print(className);
        }

        private static void FindCommand(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: find <name_of_the_class> [<requirement> ...]");
                return;
            }

            // Perform the logic for the find command
            string className = args[0];
            // ...

            Console.WriteLine($"Executing find command for class '{className}'");
            // Your code to find objects based on requirements
            // ...
        }

        private static void AddCommand(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: add <name_of_the_class> base|secondary");
                return;
            }

            // Perform the logic for the add command
            string className = args[0];
            string representation = args[1];
            // ...

            Console.WriteLine($"Executing add command for class '{className}' with representation '{representation}'");
            // Your code to interactively create and add a new object
            // ...
        }

        private static void ExitCommand(string[] args)
        {
            Console.WriteLine("Exiting the application.");
            // Any additional cleanup or logic before exiting
            // ...
        }
    }
}

