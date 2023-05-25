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
        private GameStore _store;
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
        private GameStore storeMain;
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
        private GameStore store { get; set; } // nie wiem 
        public FindCommand(GameStore store, string className, string[] args)
        {
            _className = className;
            this.args = args;
            this.store = store;
        }
        public FindCommand() { } // Parameterless constructor to serialize
        public override void Execute()
        {
            Console.WriteLine("Executing Find Command");
        }
        public override string ToString() 
        {
            return ($"Find {_className}. Arguments: {args}");
        }
    }
}
