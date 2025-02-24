using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteClassLibrary.Models.DTO
{
    public class MovieCsvDTO
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }
        public double Rating { get; set; }
        public string Overview { get; set; }
        public string Director { get; set; }
        public string Screenwriter { get; set; }
        public string UrlLogo { get; set; }
        public string Actors { get; set; }
        public string Genres { get; set; }
    }
}
