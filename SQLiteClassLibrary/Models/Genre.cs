using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteClassLibrary.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        // Связи
        public ICollection<MovieGenre> MovieGenres { get; set; } = [];
    }
}
