using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projob_Projekt.ElementsOfGamestore
{
    // Reprezentacja podstawowa
    public class User : IUser
    {
        public string nickname { get; }
        public List<IGame> ownedGames { get; set; } // rozumiem że ten set zostaje domyślny, skoro to działa?

        public User(string nickname)
        {
            this.nickname = nickname;
            ownedGames = new List<IGame>();
        }
        public override string ToString()
        {
            return nickname;
            // return "Tu dziala"; // -- tu faktycznie działa
        }
        public static Dictionary<string, object> GetAvailableFields()
        {
            return new Dictionary<string, object>
            {
                { "nickname", default(string) }
            };
        }
    }

    // Repreentacja alternatywna
    public class UserRep4
    {
        private readonly Dictionary<int, string> _myHashMap = new Dictionary<int, string>();
        private int _nickname;

        public UserRep4(string nickname, List<GameRep4>? ownedGames = null)
        {
            SetNickname(nickname);
            OwnedGames = ownedGames!;
        }

        public string GetString(int i)
        {
            return _myHashMap[i];
        }

        public virtual void SetNickname(string nickname)
        {
            _nickname = nickname.GetHashCode();
            _myHashMap[_nickname] = nickname;
        }
        public virtual int GetNickname()
        {
            return _nickname;
        }

        private List<GameRep4> _ownedGames;
        public virtual List<GameRep4> OwnedGames
        {
            get => _ownedGames;
            set => _ownedGames = value ?? new List<GameRep4>();
        }
        public override string ToString()
        {
            //return this.GetString(this.GetNickname());
            return "jestem user";
        }
        public string wypisz()
        {
            return "a tu?";
        }
    }

    // Adapter z Rep4 do bazowej
    public class AdapterFromUserRep4 : IUser
    {
        private readonly UserRep4 _userRep4;
        public AdapterFromUserRep4(UserRep4 userRep4)
        {
            _userRep4 = userRep4;
        }
        public string nickname
        {
            // get => _userRep4.GetNickname(); // GetNickname zwraca string
            get => _userRep4.GetString(_userRep4.GetNickname());
        }
        public List<IGame> ownedGames
        {
            get => _userRep4.OwnedGames.Select(game => new AdapterGameFromRep4(game)).Cast<IGame>().ToList();
        }
    }
}