using System;
using System.Linq;
using System.Linq.Expressions;
using CodeFirst.Models;

namespace HW_WS_02.Models
{
    public class AlbumModel
    {
        public static Expression<Func<Album, AlbumModel>> FromAlbum
        {
            get
            {
                return x => new AlbumModel 
                {
                    Id = x.AlbumId,
                    Title = x.Title,
                    Producer = x.Producer,
                    Year = x.Year,
                    SongsCount = x.Song.Count
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Producer { get; set; }

        public int Year { get; set; }

        public int SongsCount { get; set; }

        public Album CreateAlbum()
        {
            return new Album { Title = this.Title, Producer = this.Producer, Year = this.Year };
        }

        public Album UpdateAlbum(Album album)
        {
            if (this.Title != null)
            {
                album.Title = this.Title;
            }

            if (this.Producer != null)
            {
                album.Producer = this.Producer;
            }

            if (this.Year != null)
            {
                album.Year = this.Year;
            }

            return album;
        }
    }
}