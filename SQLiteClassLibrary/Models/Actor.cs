using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteClassLibrary.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        // Связи
        public ICollection<MovieActor> MovieActors { get; set; } = [];
    }
}
