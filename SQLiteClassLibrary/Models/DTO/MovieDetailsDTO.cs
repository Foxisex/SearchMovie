using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteClassLibrary.Models.DTO
{
    public class MovieDetailsDTO
    {
        public required string Title { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }
        public double Rating { get; set; }
        public string Overview { get; set; }
        public string Director { get; set; }
        public string Screenwriter { get; set; }
        public string UrlLogo { get; set; }
        public List<string> Genres { get; set; }
        public List<string> Actors { get; set; }

    }
}
