using System;
using System.Linq;
using System.Linq.Expressions;
using CodeFirst.Models;

namespace HW_WS_02.Models
{
    public class SongModel
    {
        public static Expression<Func<Song, SongModel>> FromSong
        {
            get
            {
                return x => new SongModel 
                {
                    Id = x.SongId,
                    Title = x.Title,
                    Genre = x.Genre,
                    Year = x.Year,
                    ArtistId = x.ArtistId,
                    AlbumId = x.AlbumId
                };
            }
        }

        public int Id { get; set; }

        public int ArtistId { get; set; }

        public int? AlbumId { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public int Year { get; set; }

        public Song CreateSong()
        {
            return new Song 
            {
                Title = this.Title,
                Genre = this.Genre,
                Year = this.Year,
                ArtistId = this.ArtistId,
                AlbumId = this.AlbumId
            };
        }

        public Song UpdateSong(Song song)
        {
            if (this.Title != null)
            {
                song.Title = this.Title;
            }

            if (this.Genre != null)
            {
                song.Genre = this.Genre;
            }

            if (this.ArtistId != 0)
            {
                song.ArtistId = this.ArtistId;
            }

            if (this.AlbumId != 0)
            {
                song.AlbumId = this.AlbumId;
            }

            if (this.Year != 0)
            {
                song.Year = this.Year;
            }

            return song;
        }
    }
}