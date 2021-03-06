﻿using System;
using System.Linq;

namespace CodeFirst.Models
{
    public class Song
    {
        public int SongId { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public int Year { get; set; }

        public int ArtistId { get; set; }

        public virtual Artist Artist { get; set; }

        public int? AlbumId { get; set; }

        public virtual Album Album { get; set; }
    }
}