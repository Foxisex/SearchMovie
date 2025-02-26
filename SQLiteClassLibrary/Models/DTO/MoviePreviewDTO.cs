using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteClassLibrary.Models.DTO
{
    public class MoviePreviewDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
        public string Overview { get; set; }
        public string UrlLogo { get; set; }
        public List<string> Genres { get; set; }
        public List<string> Actors { get; set; }
    }
}
