using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using projob_Projekt.ElementsOfGamestore;

namespace projob_Projekt
{
    public class GameStore
    {
        public IGame[] games;
        public IUser[] users;
        public IReview[] reviews;
        public IMod[] mods;

        private static Dictionary<string, Action> listDictionary;
        public GameStore()
        {
            this.games = new IGame[10];
            this.users = new IUser[8];
            this.reviews = new IReview[5];
            this.mods = new IMod[8];

            // Initialize the command dictionary
            listDictionary = new Dictionary<string, Action>
            {
                { "game", ListGame },
                { "mod", ListMod },
                { "user", ListUser},
                { "review", ListReview}
            };
        }

        public void AddGame(IGame game)
        {
            // find first null in game array
            int idx = 0;
            while (games[idx]!=null && idx<games.Length) { idx++; }
            games[idx] = game;
        }
        public void AddMod(IMod mod)
        {
            // find first null in game array
            int idx = 0;
            while (mods[idx] != null && idx < mods.Length) { idx++; }
            mods[idx] = mod;
        }
        public void AddReview(IReview rev)
        {
            // find first null in game array
            int idx = 0;
            while (reviews[idx] != null && idx < reviews.Length) { idx++; }
            reviews[idx] = rev;
        }
        public void AddUser(IUser user)
        {
            // find first null in game array
            int idx = 0;
            while (users[idx] != null && idx < users.Length) { idx++; }
            users[idx] = user;
        }

        public void print(string klasa)
        {
            listDictionary[klasa].Invoke();
        }
        private void ListReview()
        {
            foreach(var rev in reviews)
            {
                if(rev!= null)
                    Console.WriteLine(rev);
            }
        }

        private void ListUser()
        {
            foreach(var use in users)
            {
                Console.WriteLine(use);
            }    
        }

        private void ListMod()
        {
            foreach (var mod in mods)
            { Console.WriteLine(mod); }
        }

        private void ListGame()
        {
            foreach (var gam in games)
                if (gam != null)
                    gam.wypisz();
        }

        // Inicjalizacja przykładowych wartości w reprezentacji podstawowej
        public void fill()
        {
            // Przykładowe gry
            games[0] = new Game("Garbage Collector", "simulation", "PC");
            games[1] = new Game("Universe of Technology", "4X", "bitnix");
            games[2] = new Game("Moo", "rogue-like", "bitstation");
            games[3] = new Game("Tickets Please", "platformer", "bitbox");
            games[4] = new Game("Cosmic", "MOBA", "cross platform");

            // Przykładowi użytkownicy
            users[0] = new User("Szredor");
            users[0].ownedGames.Add(games[0]);
            users[0].ownedGames.Add(games[1]);
            users[0].ownedGames.Add(games[2]);
            users[0].ownedGames.Add(games[3]);
            users[0].ownedGames.Add(games[4]);

            users[1] = new User("Driver");
            users[1].ownedGames.Add(games[0]);
            users[1].ownedGames.Add(games[1]);
            users[1].ownedGames.Add(games[2]);
            users[1].ownedGames.Add(games[3]);
            users[1].ownedGames.Add(games[4]);

            users[2] = new User("Pek");
            users[2].ownedGames.Add(games[0]);
            users[2].ownedGames.Add(games[1]);
            users[2].ownedGames.Add(games[2]);
            users[2].ownedGames.Add(games[3]);
            users[2].ownedGames.Add(games[4]);

            users[3] = new User("Commander Shepard");
            users[3].ownedGames.Add(games[0]);
            users[3].ownedGames.Add(games[1]);
            users[3].ownedGames.Add(games[3]);

            users[4] = new User("MLG");
            users[4].ownedGames.Add(games[0]);
            users[4].ownedGames.Add(games[4]);

            users[5] = new User("Rondo");
            users[5].ownedGames.Add(games[0]);

            users[6] = new User("lemon");
            users[6].ownedGames.Add(games[2]);
            users[6].ownedGames.Add(games[3]);

            users[7] = new User("Bonet");
            users[7].ownedGames.Add(games[1]);

            // Przykładowe recenzje
            reviews[0] = new Review("I’m Commander Shepard and this is my favorite game on Smoke", 10, users[3]);
            reviews[1] = new Review("The Moo remake sets a new standard for the future of the survival horror series⁠, even if it isn't the sequel I've been pining for.", 12, users[1]);
            reviews[2] = new Review("Universe of Technology is a spectacular 4X game, that manages to shine even when the main campaign doesn't.", 15, users[7]);
            reviews[3] = new Review("Moo’s interesting art design can't save it from its glitches, bugs, and myriad terrible game design decisions, but I love its sound design", 2, users[6]);
            reviews[4] = new Review("I've played this game for years nonstop. Over 8k hours logged (not even joking). And I'm gonna tell you: at this point, the game's just not worth playing anymore. I think it hasn't been worth playing for a year or two now, but I'm only just starting to realize it. It breaks my heart to say that, but that's just the truth of the matter.", 5, users[0]);

            // Połączenie tego
            games[0].authors.Add(users[0]);
            games[1].authors.Add(users[1]);
            games[1].authors.Add(users[2]);
            games[2].authors.Add(users[2]);
            games[2].reviews.Add(reviews[1]);
            games[3].reviews.Add(reviews[0]);
            games[4].reviews.Add(reviews[4]);

            // Jeszcze Mody - wcześniej o nich zapomniałem
            mods[0] = new Mod("Clouds", "Super clouds", new List<IUser> { users[2] });
            mods[0] = new Mod("Clouds", "Super clouds", new List<IUser> { users[0] });
            mods[1] = new Mod("T-pose", "Cow are now T-posing", new List<IUser> { users[1] });
            mods[2] = new Mod("Commander Shepard", "I’m Commander Shepard and this is my favorite mod on Smoke", new List<IUser> { users[2] });
            mods[3] = new Mod("BTM", "You can now play in BTM’s trains and bytebuses", new List<IUser> { users[3], users[4] });
            mods[4] = new Mod("Cosmic - black hole edition", "Adds REALISTIC black holes", new List<IUser> { users[5] });

            games[0].mods.Add(mods[0]);
            games[1].mods.Add(mods[1]);
            games[2].mods.Add(mods[2]);
            games[3].mods.Add(mods[3]);

        }

        // Inicjalizacja przykładowych danych w reprezentacji alternatywnej
        public void fillRep4()
        {
            UserRep4 u1 = new UserRep4("Example");
            users[0] = new AdapterFromUserRep4(u1);

            ReviewRep4 r1 = new ReviewRep4("I’m Commander Shepard and this is my favorite game on Smoke", 10, u1);
            reviews[0] = new AdapterFromReview4(r1);

            ModRep4 m4 = new ModRep4("Clouds", "Super clouds", new List<UserRep4> { u1 });
            mods[0] = new AdapterFromModRep4(m4); // -- teraz już nie wiem czy to jest w końcu konieczne

            GameRep4 g1 = new GameRep4("Garbage Collector", "simulation", "PC", new List<UserRep4> { u1 }, new List<ReviewRep4> { r1 }, new List<ModRep4> { m4 });
            GameRep4 g2 = new GameRep4("Universe of Technology", "4X", "bitnix");
            games[0] = new AdapterGameFromRep4(g1);
            games[1] = new AdapterGameFromRep4(g2);            
        }

        //public void print()
        //{
        //    foreach (var us in games) { if (us != null) us.wypisz(); }
        //}
        //public void list_games()
        //{
        //    foreach (var game in games)
        //    {
        //        game.wypisz();
        //    }
        //}

    }
}
