using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.Models
{
    public class Album
    {
        private ICollection<Artist> artist;
        private ICollection<Song> song;

        public Album()
        {
            this.artist = new HashSet<Artist>();
            this.song = new HashSet<Song>();
        }

        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Producer { get; set; }
        public int Year { get; set; }

        public virtual ICollection<Artist> Artist 
        { 
            get
            {
                return this.artist;
            }

            set 
            { 
                this.artist=value;
            }
        }

        public virtual ICollection<Song> Song
        {
            get
            {
                return this.song;
            }

            set
            {
                this.song = value;
            }
        }

    
    }
}
