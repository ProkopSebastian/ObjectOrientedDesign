using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projob_Projekt.ElementsOfGamestore
{
    // Reprezentacja podstawowa
    public class Mod : IMod // jesli juz mam gettery to chyba mozna wywalic to public tylko trzeba by to zrobic juz wszedzie 
    {
        public string name { get; }
        public string description { get; }
        public List<IUser> authors { get; }

        public Mod(string name, string description, List<IUser> authors = null)
        {
            this.name = name;
            this.description = description;
            this.authors = authors;
        }
        public override string ToString()
        {
            return name + ", opis: " + description;
        }
        public static Dictionary<string, object> GetAvailableFields()
        {
            return new Dictionary<string, object>
            {
                { "name", default(string) },
                { "description", default(string) },
            };
        }
    }

    // Reprezentacja alternatywna
    public class ModRep4
    {
        private readonly Dictionary<int, string> _myHashMap = new();
        private int _name;
        private int _description;

        public ModRep4(string name, string description, List<UserRep4>? authors = null, List<ModRep4>? compatibility = null)
        {
            SetName(name);
            SetDescription(description);
            Authors = authors!;
            Compatibility = compatibility!;
        }
        public string GetString(int i)
        {
            return _myHashMap[i];
        }

        public virtual void SetName(string name)
        {
            _name = name.GetHashCode();
            _myHashMap[_name] = name;
        }
        public virtual int GetName()
        {
            return _name;
        }

        public virtual void SetDescription(string description)
        {
            _description = description.GetHashCode();
            _myHashMap[_description] = description;
        }
        public virtual int GetDescription()
        {
            return _description;
        }

        private List<UserRep4> _authors;
        public virtual List<UserRep4> Authors
        {
            get => _authors;
            set => _authors = value ?? new List<UserRep4>();
        }

        private List<ModRep4> _compatibility;
        public virtual List<ModRep4> Compatibility
        {
            get => _compatibility;
            set => _compatibility = value ?? new List<ModRep4>();
        }
    }

    // Adapter z Rep4 do reprezentacji bazowej
    public class AdapterFromModRep4 : IMod
    {
        private readonly ModRep4 _mod4;
        public AdapterFromModRep4(ModRep4 mod4)
        {
            _mod4 = mod4;
        }
        public string name
        {
            get => _mod4.GetString(_mod4.GetName());
        }
        public string description
        {
            get => _mod4.GetString(_mod4.GetDescription());
        }
        public List<IUser> authors
        {
            get => _mod4.Authors.Select(autor => new AdapterFromUserRep4(autor)).Cast<IUser>().ToList();
        }
    }
}