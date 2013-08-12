using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Models
{
    public class Artist
    {
        //private ICollection<Song> song;

        public int ArtistId { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public DateTime DateOfBirth { get; set; }

        //public virtual ICollection<Song> Song
        //{
        //    get
        //    {
        //        return this.song;
        //    }
        //    set
        //    {
        //        this.song = value;
        //    }
        //}

        //public Artist()
        //{
        //    this.song = new HashSet<Song>();
        //}
    }
}