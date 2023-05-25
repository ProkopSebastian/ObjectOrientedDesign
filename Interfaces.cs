using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace projob_Projekt
{
    public interface IGame
    {
        string name { get; }
        string genre { get; }
        List<IUser> authors { get; }
        List<IReview> reviews { get; }
        List<IMod> mods { get; } 
        string devices { get; }
        void wypisz();
    }
    public interface IReview
    {
        string text { get;}
        int rating { get; }
        IUser author { get; } // wyrzucilem set
    }
    public interface IMod
    {
        string name { get; }
        string description { get; }
        List<IUser> authors { get; }
        public string ToString();
    }
    public interface IUser
    {
        string nickname { get; }
        List<IGame> ownedGames { get; } // wyrzucilem set 
    }
    public interface ICommand : ISerializable
    {
        void Execute();
        void SerializeItem(string fileName, XmlSerializer serializer);
        string ToString();
    }
    [XmlInclude(typeof(ListCommand))]
    [XmlInclude(typeof(AddCommand))]
    [XmlInclude(typeof(FindCommand))]
    public abstract class CommandAbst
    {
        public abstract void Execute();
        //public abstract void SerializeItem(string fileName, XmlSerializer serializer);
    }
}
