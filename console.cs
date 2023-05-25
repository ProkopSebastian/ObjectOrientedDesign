using projob_Projekt.ElementsOfGamestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace projob_Projekt
{
    public class console
    {
        private static Dictionary<string, Action<string[]>> commandDictionary;
        private static Dictionary<string, Dictionary<string, object>> AvailableFields;
        public GameStore storeMain; // reference to GameStore prepared in main
        private List<CommandAbst> commands;

        public console(GameStore store)
        {
            commandDictionary = new Dictionary<string, Action<string[]>>
            {
                { "list", AddListCommand },
                { "find", AddFindCommand },
                { "add", AddAddCommand },
                { "exit", ExitCommand },
                { "queue print", QueuePrint },
                { "queue commit", QueueCommit },
                { "queue dismiss", QueueDismiss },
                { "queue export", QueueExport },
                { "queue load", QueueLoad }
            };

            AvailableFields = new Dictionary<string, Dictionary<string, object>>
            {
                { "game", Game.GetAvailableFields() },
                { "mod", Mod.GetAvailableFields() },
                { "user", User.GetAvailableFields() },
                { "review", Review.GetAvailableFields() },
            };

            this.storeMain = store;
            commands = new List<CommandAbst>();

            // Main loop of application is in constructor which is probably not the best solution
            string command;
            do
            {
                Console.Write("Enter a command: ");
                command = Console.ReadLine();
                ProcessCommand(command);
            } while (command != "exit");
        }
        // To do -- which and why should be static
        private static void ProcessCommand(string command)
        {
            string[] commandTokens = Regex.Matches(command, @"[\""].+?[\""]|[^ ]+")
                                          .Cast<Match>()
                                          .Select(m => m.Value)
                                          .ToArray();
            if (commandTokens.Length > 0)
            {
                string commandName = commandTokens[0].ToLower();

                bool b = false;
                // In case of print queue than contains space:
                if (commandName == "queue")
                {
                    if (commandTokens.Length > 1)
                    {
                        if (commandTokens[1].ToLower() == "print")
                        {
                            string[] commandArgs = new string[commandTokens.Length - 1];
                            Array.Copy(commandTokens, 1, commandArgs, 0, commandArgs.Length);

                            commandDictionary["queue print"].Invoke(commandArgs);
                        }
                        else if (commandTokens[1].ToLower() == "commit")
                        {
                            string[] commandArgs = new string[commandTokens.Length - 1];
                            Array.Copy(commandTokens, 1, commandArgs, 0, commandArgs.Length);

                            commandDictionary["queue commit"].Invoke(commandArgs);
                        }
                        else if (commandTokens[1].ToLower() == "dismiss")
                        {

                            string[] commandArgs = new string[commandTokens.Length - 1];
                            Array.Copy(commandTokens, 1, commandArgs, 0, commandArgs.Length);

                            commandDictionary["queue dismiss"].Invoke(commandArgs);
                        }
                        else if (commandTokens[1].ToLower() == "export")
                        {

                            string[] commandArgs = new string[commandTokens.Length - 2];
                            Array.Copy(commandTokens, 2, commandArgs, 0, commandArgs.Length);

                            commandDictionary["queue export"].Invoke(commandArgs);
                        }
                        else if (commandTokens[1].ToLower() == "load")
                        {

                            string[] commandArgs = new string[commandTokens.Length - 2];
                            Array.Copy(commandTokens, 2, commandArgs, 0, commandArgs.Length);

                            commandDictionary["queue load"].Invoke(commandArgs);
                        }
                        else
                        {
                            PrintPossibleCommands();
                        }
                    }
                    else
                    {
                        PrintPossibleCommands();
                    }
                }

                else if (commandDictionary.ContainsKey(commandName))
                {
                    string[] commandArgs = new string[commandTokens.Length - 1];
                    Array.Copy(commandTokens, 1, commandArgs, 0, commandArgs.Length);

                    commandDictionary[commandName].Invoke(commandArgs);
                }
                else
                {
                    PrintPossibleCommands();
                }
            }
        }
        public static void PrintPossibleCommands()
        {
            var keys = new List<string>();
            foreach (var key in commandDictionary.Keys)
            {
                keys.Add("[" + key.ToString() + "]");
            }
            Console.WriteLine("Possible commands: " + string.Join(", ", keys));
        }
        private void AddListCommand(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: list <name_of_the_class>");
                return;
            }

            string className = args[0];
            if (!AvailableFields.ContainsKey(className))
            {
                PrintPossibleCommands();
                return;
            }

            Console.WriteLine($"Executing list command for class '{className}'");
            CommandAbst command = new ListCommand(this.storeMain, className, args); // Arguments here make no sense though
            commands.Add(command);
            Console.WriteLine($"List command for {className} added to queue.");
            //storeMain.print(className);
            // Każde polecenie powinno być oddzielnym obiektem, wtedy łatwiej serializować 
            // Wzorzec command, z interfejsem i metodą execute 
            // wzorzec np chain ... 
            // komendy obiektami 
        }
        private void AddFindCommand(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: find <name_of_the_class> [<requirement> ...]");
                return;
            }

            string className = args[0];
            string[] commandArgs = new string[args.Length - 1];
            Array.Copy(args, 1, commandArgs, 0, commandArgs.Length);

            Console.WriteLine($"Executing find command for class '{className}'");
            CommandAbst command = new FindCommand(this.storeMain, className, args);
            commands.Add(command);
            Console.WriteLine("Find command added to command queue");
        }
        private void AddAddCommand(string[] args)
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

            AddCommand add = new AddCommand(storeMain, className, representation, tab);
            commands.Add(add);
            Console.WriteLine("Add command added to command queue");
        }
        private static void ExitCommand(string[] args)
        {
            Console.WriteLine("Exiting the application.");
        }
        private void QueuePrint(string[] args)
        {
            Console.WriteLine("Listing commands queue");
            foreach (var com in this.commands)
            {
                Console.WriteLine(com.ToString());
            }
        }
        private void QueueCommit(string[] args)
        {
            foreach(var com in this.commands)
            {
                com.Execute();
            }
            commands.Clear();
        }
        private void QueueDismiss(string[] args)
        {
            int len = commands.Count;
            commands.Clear();
            Console.WriteLine($"{len} commands cleared");
        }
        private void QueueExport(string[] args)
        {                
            string givenFileName = "default.xml";
            if (args.Length != 0)
                givenFileName = args[0];
            //string fileName = "XMLFiles\\" + givenFileName;
            string fileName = givenFileName;
            string path = Path.Combine(Environment.CurrentDirectory, fileName);

            // Create a new list to hold the commands
            //List<CommandAbst> commandList = new List<CommandAbst>(commands);

            XmlSerializer serializer = new XmlSerializer(typeof(List<CommandAbst>));
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, commands);
            }

            Console.WriteLine("Done");
        }
        private void QueueLoad(string[] args)
        {
            string fileName = args[0];

            // Determine the file format based on the file extension
            string fileExtension = Path.GetExtension(fileName);

            switch (fileExtension)
            {
                case ".xml":
                    LoadCommandsFromXml(fileName);
                    break;
                case ".txt":
                    //LoadCommandsFromPlainText(fileName);
                    break;
                default:
                    Console.WriteLine("Unsupported file format.");
                    break;
            }

            Console.WriteLine("Done");
        }

        private void LoadCommandsFromXml(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CommandAbst>));

            using (StreamReader reader = new StreamReader(fileName))
            {
                List<CommandAbst> loadedCommands = (List<CommandAbst>)serializer.Deserialize(reader);
                commands.AddRange(loadedCommands);
            }
        }
    }
}

