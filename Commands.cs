using projob_Projekt.ElementsOfGamestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace projob_Projekt
{
    [Serializable]
    public class ListCommand : CommandAbst
    {
        [XmlIgnore]
        private GameStore? _store;
        [XmlElement]
        public string _className;
        [XmlElement]
        public string[] args;
        public ListCommand(GameStore _gameStore, string _className, string[] args)
        {
            this._store = _gameStore;
            this._className = _className;
            this.args = args;
        }
        public ListCommand() { }
        public override void Execute()
        {
            if (this._store == null)
            {
                _store = console.storeMain;
            }
            _store.print(_className);
        }
        public override string ToString() // It bothers me that from very beginning there are 2 references to this
        {
            return ($"List {_className}");
        }
    }
    [Serializable]
    public class AddCommand : CommandAbst
    {
        [XmlIgnore]
        private GameStore? storeMain;
        [XmlElement]
        public string className;
        [XmlElement]
        public object[] tab;
        [XmlElement]
        public string representation;
        public AddCommand(GameStore store, string className, string representation, object[] args)
        {
            this.storeMain = store;
            this.className = className;
            this.tab = args;
            this.representation = representation;
        }
        public AddCommand() { } // Parameterless constructor needed for serialization

        public override void Execute()
        {
            if (storeMain == null)
            {
                storeMain = console.storeMain;
            }
            Console.WriteLine("executing AddCommand");
            // GAME
            if (className == "game" && representation == "base")
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
        public override string ToString()
        {
            return ($"Add {className}. Arguments: {tab.ToString()}");
        }
    }
    [Serializable]
    public class FindCommand : CommandAbst
    {
        [XmlElement]
        public string _className { get; set; }
        [XmlElement]
        public string[] args { get; set; }
        [XmlIgnore]
        private GameStore? store { get; set; }
        public FindCommand(GameStore store, string className, string[] args)
        {
            _className = className;
            // argumenty trzeba oczyścić bo zerowy to nazwa klasy którą już zapisujemy w specjalnym do tego polu
            this._className = args[0];
            string[] commandArgs = new string[args.Length - 1];
            Array.Copy(args, 1, commandArgs, 0, commandArgs.Length);
            this.args = commandArgs;
            this.store = store;
        }
        public FindCommand() { } // Parameterless constructor to serialize
        public override void Execute()
        {
            if (store == null)
            {
                store = console.storeMain;
            }

            // Pobranie odpowiedniej tablicy obiektów na podstawie klasy
            object[] objects = null;
            switch (_className)
            {
                case "game":
                    objects = store.games;
                    break;
                case "mod":
                    objects = store.mods;
                    break;
                case "user":
                    objects = store.users;
                    break;
                case "review":
                    objects = store.reviews;
                    break;
                default:
                    Console.WriteLine($"Class {_className} is not supported for searching.");
                    return;
            }

            // Utworzenie słownika do przechowywania wymagań
            Dictionary<string, ComparisonRequirement> requirements = new Dictionary<string, ComparisonRequirement>();

            // Przetwarzanie poszczególnych argumentów
            foreach (string arg in args)
            {
                string[] parts = arg.Split(new[] { '=', '>', '<' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                {
                    Console.WriteLine($"Invalid requirement format: {arg}");
                    return;
                }

                string fieldName = parts[0];
                string operatorString = arg.Replace(fieldName, "").Trim();
                string requirement = parts[1];

                // Sprawdzenie, czy pole istnieje w klasie
                // ...


                // Konwersja operatora na odpowiedni enum ComparisonOperator
                ComparisonOperator comparisonOperator;
                switch (operatorString[0])
                {
                    case '=':
                        comparisonOperator = ComparisonOperator.Equal;
                        break;
                    case '>':
                        comparisonOperator = ComparisonOperator.GreaterThan;
                        break;
                    case '<':
                        comparisonOperator = ComparisonOperator.LessThan;
                        break;
                    default:
                        Console.WriteLine($"Invalid operator: {operatorString}");
                        return;
                }

                // Dodanie wymagania do słownika requirements
                requirements[fieldName] = new ComparisonRequirement(fieldName, comparisonOperator, requirement);

            }

            // Implementacja logiki wyszukiwania i wyświetlania pasujących obiektów
            // ...

            List<object> foundObjects = new List<object>();
            //bool fit = true;
            //if (_className == "game")
            //{
            //    if (requirements["name"] != null)
            //    {

            //    }
            //}
            //foreach (object obj in objects)
            //{
            //    bool satisfiesRequirements = true;
            //    foreach (var requirement in requirements)
            //    {
            //        // Sprawdzenie, czy obiekt spełnia wymaganie
            //        if (!requirement.Value.IsMatch(obj))
            //        {
            //            satisfiesRequirements = false;
            //            break;
            //        }
            //    }
            //    if (satisfiesRequirements)
            //    {
            //        foundObjects.Add(obj);
            //    }
            //}
        }
        public enum ComparisonOperator
        {
            Equal,
            GreaterThan,
            LessThan
        }


        public class ComparisonRequirement
        {
            public string FieldName { get; }
            public ComparisonOperator Operator { get; }
            public string Value { get; }

            public ComparisonRequirement(string fieldName, ComparisonOperator comparisonOperator, string value)
            {
                FieldName = fieldName;
                Operator = comparisonOperator;
                Value = value;
            }
        }

        private static bool CheckRequirement(object fieldValue, string requirement, string comparisonOperator)
        {
            switch (comparisonOperator)
            {
                case "=":
                    return fieldValue.ToString() == requirement;
                case "<":
                    return String.Compare(fieldValue.ToString(), requirement) < 0;
                case ">":
                    return String.Compare(fieldValue.ToString(), requirement) > 0;
                default:
                    Console.WriteLine($"Invalid comparison operator: {comparisonOperator}");
                    return false;
            }
        }

        public override string ToString()
        {
            return ($"Find {_className}. Arguments: {args}");
        }
    }
}
