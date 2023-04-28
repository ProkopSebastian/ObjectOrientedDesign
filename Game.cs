using projob_Projekt;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projob_Projekt
{
    // Reprezentacja podstawowa
    public class Game : IGame
    {
        public string name { get; }
        public string genre { get; }
        public List<IUser> authors { get; }
        public List<IReview> reviews { get; }
        public List<IMod> mods { get; }
        public string devices { get; }


        public Game(string name, string genre, string devices)
        {
            this.name = name;
            this.genre = genre;
            authors = new List<IUser>();
            mods = new List<IMod>();
            reviews = new List<IReview>();
            this.devices = devices;
        }
        public void wypisz()
        {
            Console.WriteLine($"Gra - {name}, z gatunku {genre}, \n" +
                $"autorzy: {string.Join(", ", this.authors)}\n" + 
                $"recenzje: {string.Join(", ", reviews)}, \n" +
                $"mody: {string.Join(", ", mods)}, \nna platformy: {devices} \n");
        }
    }

    // Reprezentacja alternatywna
    public class GameRep4
    {
        private Dictionary<int, string> _myHashMap = new Dictionary<int, string>();
        private int _name;
        private int _genre;
        private int _devices;

        public GameRep4(string name, string genre, string devices, List<UserRep4>? authors = null,
            List<ReviewRep4>? reviews = null, List<ModRep4>? mods = null)
        {
            SetName(name);
            SetGenre(genre);
            SetDevices(devices);
            Authors = authors!;
            Reviews = reviews!;
            Mods = mods!;
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
        public virtual void SetGenre(string genre)
        {
            _genre = genre.GetHashCode();
            _myHashMap[_genre] = genre;
        }
        public virtual int GetGenre()
        {
            return _genre;
        }

        public virtual void SetDevices(string devices)
        {
            _devices = devices.GetHashCode();
            _myHashMap[_devices] = devices;
        }
        public virtual int GetDevices()
        {
            return _devices;
        }

        private List<UserRep4> _authors;
        public virtual List<UserRep4> Authors
        {
            get => _authors;
            set => _authors = value ?? new List<UserRep4>();
        }

        private List<ReviewRep4> _reviews;
        public virtual List<ReviewRep4> Reviews
        {
            get => _reviews;
            set => _reviews = value ?? new List<ReviewRep4>();
        }

        private List<ModRep4> _mods;
        public virtual List<ModRep4> Mods
        {
            get => _mods;
            set => _mods = value ?? new List<ModRep4>();
        }
    }

    // Adapter z Rep4 do Bazowej
    public class AdapterGameFromRep4 : IGame
    {
        private readonly GameRep4 _game4;
        public AdapterGameFromRep4(GameRep4 game4)
        {
            _game4 = game4;
        }
        public string name 
        { 
            get => _game4.GetString(_game4.GetName()); // bo GetName zwraca prywatnego int-a odpowiadającego stringowi name
            set => _game4.SetName(value);
        }
        public string genre
        {
            get => _game4.GetString(_game4.GetGenre());
        }
        public string devices
        {
            get => _game4.GetString(_game4.GetDevices());
        }
        public List<IUser> authors
        {
            get => _game4.Authors.Select(gAut => new AdapterFromUserRep4(gAut)).Cast<IUser>().ToList();
            // set; 
        }
        public List<IReview> reviews
        {
            get => _game4.Reviews.Select(g => new AdapterFromReview4(g)).Cast<IReview>().ToList();
        }
        public List<IMod> mods // co sie zmienia jesli tu zaimplementuje Mod zamiast IMod
        {
            get => _game4.Mods.Select(mod => new AdapterFromModRep4(mod)).Cast<IMod>().ToList();
        }
        public void wypisz()
        {
            // Czy to na pewno dobrze, ze tak to robie?
            Console.WriteLine($"Gra - {name}, z gatunku {genre}, \n" +
            $"autorzy: {string.Join(", ", this.authors)}\n" +
            $"recenzje: {string.Join(", ", reviews)}, \n" +
            $"mody: {string.Join(", ", mods)}, \nna platformy: {devices} \n");
        }

    }
}