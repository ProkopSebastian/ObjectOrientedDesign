using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projob_Projekt.ElementsOfGamestore
{
    // Reprezentacja podstawowa
    public class Review : IReview
    {
        public string text { get; }
        public int rating { get; }
        public IUser author { get; set; }

        public Review(string text, int rating, IUser author=null)
        {
            this.text = text;
            this.rating = rating;
            this.author = author;
        }
        public override string ToString()
        {
            return text;
        }
        public static Dictionary<string, object> GetAvailableFields()
        {
            return new Dictionary<string, object>
            {
                { "text", default(string) },
                { "rating", default(int) }
            };
        }
    }

    // Reprezentacja alternatywna
    public class ReviewRep4
    {
        private readonly Dictionary<int, string> _myHashMap = new();
        private int _text;
        private int _rating;

        public ReviewRep4(string text, int rating, UserRep4? author = null)
        {
            SetText(text);
            SetRating(rating);
            Author = author!;
        }
        public string GetString(int i)
        {
            return _myHashMap[i];
        }
        public virtual void SetText(string text)
        {
            _text = text.GetHashCode();
            _myHashMap[_text] = text;
        }
        public virtual int GetText()
        {
            return _text;
        }

        public virtual void SetRating(int rating)
        {
            string stringRating = rating.ToString();
            _rating = stringRating.GetHashCode();
            _myHashMap[_rating] = stringRating;
        }
        public virtual int GetRating()
        {
            return _rating;
        }

        private UserRep4 _author;
        public virtual UserRep4 Author
        {
            get => _author;
            set => _author = value ?? new UserRep4("");
        }
    }

    // Adapter z Rep4 do bazowej 
    public class AdapterFromReview4 : IReview
    {
        private readonly ReviewRep4 _review4;
        public AdapterFromReview4(ReviewRep4 review4)
        {
            _review4 = review4;
        }
        public string text
        {
            get => _review4.GetString(_review4.GetText()); // bo getText zwraca private int ktory powstaje w procesie hashowania
        }
        public int rating
        {
            //get => int.Parse(_review4.GetString(_review4.GetText()));
            // no i właśnie nie wiem na ile mam udawać to hashowanie a na ile moge po prostu uzyc metody ponizej:
            get => _review4.GetRating(); // ale to pole chyba tam nie powinno istnieć...
        }
        public IUser author
        {
            get => new AdapterFromUserRep4(_review4.Author);
        }
    }
}