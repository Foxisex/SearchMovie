﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteClassLibrary.Models
{
    public class MovieActor
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
