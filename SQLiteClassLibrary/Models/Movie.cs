using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteClassLibrary.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int Year { get; set; }
        public string Country { get; set; }
        public double Rating { get; set; }
        public string Overview { get; set; }
        public string Director { get; set; }
        public string Screenwriter { get; set; }
        public string UrlLogo { get; set; }

        // Связи
        public ICollection<MovieActor> MovieActors { get; set; } = [];
        public ICollection<MovieGenre> MovieGenres { get; set; } = [];
    }
}
