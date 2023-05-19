using projob_Projekt.ElementsOfGamestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace projob_Projekt
{
    public class console
    {
        private static Dictionary<string, Action<string[]>> commandDictionary;
        private static Dictionary<string, Dictionary<string, object>> AvailableFields;
        public GameStore storeMain; // reference to GameStore prepared in main

        public console(GameStore store)
        {
            commandDictionary = new Dictionary<string, Action<string[]>>
            {
                { "list", ListCommand },
                { "find", FindCommand },
                { "add", AddCommand },
                { "exit", ExitCommand }
            };

            AvailableFields = new Dictionary<string, Dictionary<string, object>>
            {
                { "game", Game.GetAvailableFields() },
                { "mod", Mod.GetAvailableFields() },
                { "user", User.GetAvailableFields() },
                { "review", Review.GetAvailableFields() },
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
            // Skopiowane z internetu
            string[] commandTokens = Regex.Matches(command, @"[\""].+?[\""]|[^ ]+") 
                                          .Cast<Match>()
                                          .Select(m => m.Value)
                                          .ToArray();

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
            storeMain.print(className);
            // Każde polecenie powinno być oddzielnym obiektem, wtedy łatwiej serializować 
            // Wzorzec command, z interfejsem i metodą execute 
            // wzorzec np chain ... 
            // komendy obiektami 
        }

        private static void FindCommand(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: find <name_of_the_class> [<requirement> ...]");
                return;
            }

            string className = args[0];

            Console.WriteLine($"Executing find command for class '{className}'");

        }

        private void AddCommand(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: add <name_of_the_class> base|secondary");
                return;
            }

            string className = args[0];
            string representation = args[1];

            Console.WriteLine($"Executing add command for class '{className}' with representation '{representation}'");
            Dictionary<string, object> fields = new Dictionary<string, object>();
            if (AvailableFields.ContainsKey(className))
            {
                fields = AvailableFields[className];
            }
            Console.WriteLine($"Available fields: '{string.Join(", ", fields.Keys)}'");
            string input;
            input = Console.ReadLine();
            object[] tab = new object[fields.Count];
            int index = 0;
            do
            {
                string[] parts = input.Split('=');
                if (parts.Length == 2)
                {
                    string fieldName = parts[0].Trim();
                    string fieldValue = parts[1].Trim();

                    if (fields.ContainsKey(fieldName))
                    {
                        index = Array.IndexOf(fields.Keys.ToArray(), fieldName);
                        tab[index] = fieldValue;
                    }
                    else
                    {
                        Console.WriteLine($"Field '{fieldName}' does not exist.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input format. Please use 'fieldName=value' format.");
                }

                input = Console.ReadLine();
            } while (input != "DONE" && input != "EXIT");
            if (input == "EXIT")
                return;
            // GAME
            if (className == "game" && representation=="base")
            {
                Game nowa = new Game((string)tab[0], (string)tab[1], (string)tab[2]);
                storeMain.AddGame(nowa);
                return;
            }
            if (className == "game" && representation == "secondary")
            {
                GameRep4 nowa = new GameRep4((string)tab[0], (string)tab[1], (string)tab[2]);
                storeMain.AddGame(new AdapterGameFromRep4(nowa));
            }
            // MOD
            if (className == "mod" && representation == "base")
            {
                Mod nowa = new Mod((string)tab[0], (string)tab[1]);
                storeMain.AddMod(nowa);
                return;
            }
            if (className == "mod" && representation == "secondary")
            {
                ModRep4 nowa = new ModRep4((string)tab[0], (string)tab[1]);
                storeMain.AddMod(new AdapterFromModRep4(nowa));
            }
            // USER
            if (className == "user" && representation == "base")
            {
                User nowa = new User((string)tab[0]);
                storeMain.AddUser(nowa);
                return;
            }
            if (className == "user" && representation == "secondary")
            {
                UserRep4 nowa = new UserRep4((string)tab[0]);
                storeMain.AddUser(new AdapterFromUserRep4(nowa));
            }
            // REVIEW
            if (className == "review" && representation == "base")
            {
                Review nowa = new Review((string)tab[0], (int)tab[1]);
                storeMain.AddReview(nowa);
                return;
            }
            if (className == "review" && representation == "secondary")
            {
                ReviewRep4 nowa = new ReviewRep4((string)tab[0], (int)tab[1]);
                storeMain.AddReview(new AdapterFromReview4(nowa));
            }

        }

        private static void ExitCommand(string[] args)
        {
            Console.WriteLine("Exiting the application.");
        }
    }
}

