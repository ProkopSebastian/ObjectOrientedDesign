using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;
using System.ComponentModel.Design;
using System.Reflection.Metadata;
using System.Text;

namespace projob_Projekt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Testy zadania punktowanego nr 1:
            //Test1();

            Console.WriteLine("------- Iterator --------");
            IteratorTest1();
            Console.WriteLine("-------Usuwanie---------");
            IteratorTest2();
            Console.WriteLine("------ z uzyciem FOREACH");
            IteratorTest3();
            Console.WriteLine("-----Count if -----------");
            IteratorTest4();
            Console.WriteLine("-------KONIEC----------");
        }

        // Funkcje do wypisywania:
        static void PrintGame(IGame game)
        {
            Console.Write($"Gra o tytule: {game.name} \n" +
                $"Autorzy: ");
            foreach(var a in game.authors)
            {
                PrintAuthor(a);
            }
            Console.WriteLine("");
            Console.WriteLine("Recenzje gry: ");
            foreach(var b in game.reviews)
            {
                PrintReview(b);
            }
            Console.Write("Mody do tej gry: ");
            foreach (var c in game.mods)
            {
                PrintMods(c);
            }
            Console.WriteLine('\n');
        }
        static void PrintAuthor(IUser user)
        {
            Console.Write(user.nickname + ", ");
        }
        static void PrintReview(IReview review)
        {
            Console.WriteLine(review.text);
        }
        static void PrintMods(IMod mod)
        {
            Console.WriteLine(mod.description);
        }

        static void Test1()
        {
            GameStore gameStore = new GameStore();
            gameStore.fill();

            GameStore gameStore4 = new GameStore();
            gameStore4.fillRep4();

            Console.WriteLine("\n----------- From Rep0 ----------- \n");
            foreach (var game in gameStore.games)
            {
                PrintGame(game);
            }

            Console.WriteLine("\n----------- From Rep4 ----------- \n");
            PrintGame(gameStore4.games[0]);

            Console.WriteLine("\n----------- Zadanie ----------- \n");
            int s = 0, i = 0;
            foreach (Game game1 in gameStore.games)
            {
                foreach (Review review in game1.reviews)
                {
                    s += review.rating;
                    i++;
                }
                float mean = -1;
                if (i != 0)
                    mean = s / i;
                if (mean > 10)
                {
                    Console.WriteLine("--- Udało się! ---");
                    Console.WriteLine("Średnia: " + mean);
                    PrintGame(game1);
                }
            }
        }
        // Funkcje do iteratorów
         static void IteratorTest1()
        {
            SquareArray<int> array = new SquareArray<int>();
            array.Insert(0);
            array.Insert(1);
            array.Insert(2);
            array.Insert(3);
            array.Insert(4);

            IIterator<int> forwardIterator = array.GetForwardIterator();
            Console.WriteLine("Forward iterator:");
            foreach (int value in forwardIterator.GetValues())
            {
                Console.Write(value + " ");
            }

            IIterator<int> reverseIterator = array.GetReverseIterator();
            Console.WriteLine("\nReverse iterator:");
            foreach (int value in reverseIterator.GetValues())
            {
                Console.Write(value + " ");
            }
        }
        static void IteratorTest2()
        {
            SquareArray<int> array = new SquareArray<int>();
            array.Insert(0);
            array.Insert(1);
            array.Insert(2);
            array.Insert(3);
            array.Insert(4);

            IIterator<int> forwardIterator = array.GetForwardIterator();
            Console.WriteLine("Forward iterator:");
            foreach (int value in forwardIterator.GetValues())
            {
                Console.Write(value + " ");
            }
            array.Usun();
            Console.WriteLine("\nremoving");
            IIterator<int> forwardIterator2 = array.GetForwardIterator();
            Console.WriteLine("Forward iterator:");
            foreach (int value in forwardIterator2.GetValues())
            {
                Console.Write(value + " ");
            }
            Console.WriteLine("");
        }

        static void IteratorTest3()
        {
            SquareArray<int> array = new SquareArray<int>();
            array.Insert(0);
            array.Insert(1);
            array.Insert(2);
            array.Insert(3);
            array.Insert(4);
            foreach(var v in array)
            {
                Console.WriteLine(v + " ");
            }
        }

        static void IteratorTest4()
        {
            SquareArray<int> array = new SquareArray<int>();
            array.Insert(0);
            array.Insert(1);
            array.Insert(2);
            array.Insert(3);
            array.Insert(4);
            Func<int, bool> predicate = (value) => value > 3;
            int count = array.CountIf(predicate);
            Console.WriteLine("Wiekszych od 3 jest:" + count);
        }
    }
}

// TO DO
// -- przerzucić wypisywacze do oddzielnej klasy wypisującej żeby to nie siedziało tutaj w Mainie lamersko 
// -- Ująć to co jest teraz w mainie w metode klasy zadanie1, może to być metoda 'działaj' żeby też lamersko w mainie nie siedziało 

// Get enumerator żeby to po ludzku działało 