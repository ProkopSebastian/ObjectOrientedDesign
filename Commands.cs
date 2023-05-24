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
    public class ListCommand : ICommand
    {
        private GameStore _store;
        private string _className;
        string[] args;
        public ListCommand(GameStore _gameStore, string _className, string[] args)
        {
            this._store = _gameStore;
            this._className = _className;
            this.args = args;
        }
        public void Execute() // This must be public to implement Interface Command
        {
            _store.print(_className);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public void SerializeItem(string fileName, XmlSerializer serializer)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(fileStream, _store);
                serializer.Serialize(fileStream, _className);
            }
        }

        public override string ToString() // It bothers me that from very beginning there are 2 references to this
        {
            return ($"List {_className}");
        }
    }
    public class AddCommand : ICommand
    {

        private GameStore storeMain;
        private string className;
        private object[] tab;
        private string representation;
        public AddCommand(GameStore store, string className, string representation, object[] args)
        {
            this.storeMain = store;
            this.className = className;
            this.tab = args;
            this.representation = representation;
        }

        public void Execute()
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public void SerializeItem(string fileName, XmlSerializer serializer)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(fileStream, storeMain);
                serializer.Serialize(fileStream, className);
                serializer.Serialize(fileStream, representation);
                serializer.Serialize(fileStream, tab);
            }
        }

        public override string ToString() 
        {
            return ($"Add {className}. Arguments: {tab.ToString()}");
        }
    }
    public class FindCommand : ICommand
    {
        private string _className;
        private string[] args;
        private GameStore store;
        public FindCommand(GameStore store, string className, string[] args)
        {
            _className = className;
            this.args = args;
            this.store = store;
        }
        public void Execute()
        {
            Console.WriteLine("Executing Find Command");
        }

        public void SerializeItem(string fileName, XmlSerializer serializer)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(fileStream, _className);
                serializer.Serialize(fileStream, args);
            }
        }

        public override string ToString() 
        {
            return ($"Find {_className}. Arguments: {args}");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("className", _className);
        }
    }

}
