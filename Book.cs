using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WillisfordProg260CourseProject
{
    class Book
    {
        // For each book the Title, Author, Rating(1 poor, to 5 great) and PubYear(year published) will be stored 
        // and retrievable by using the books ISBN number.
        public string Title { get; set; }
        public String Author { get; set; }
        public Byte Rating { get; set; }
        public int PubYear { get; set; }
        public int ISBN { get; set; }

        // Constructor
        public Book(string _Title = "No Such Book", string _Author = "", byte _Rating = 0, int _Year = 0, int _ISBN = 0)
        {
            // Is it best practice to have input validation here in the class or in the call?  
            // Basic research seems to indicate that it completely depends on the program and the language.
            Title = _Title;
            Author = _Author;
            Rating = _Rating;
            PubYear = _Year;
            ISBN = _ISBN;
        }
    }
}
