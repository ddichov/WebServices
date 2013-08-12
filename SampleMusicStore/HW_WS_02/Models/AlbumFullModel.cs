using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CodeFirst.Models;

namespace HW_WS_02.Models
{
    public class AlbumFullModel : AlbumModel
    {
        public IEnumerable<SongModel> Songs { get; set; }

        public static Expression<Func<Album, AlbumFullModel>> FromFullAlbum
        {
            get
            {
                return x => new AlbumFullModel
                {
                    Id = x.AlbumId,
                    Title = x.Title,
                    Producer = x.Producer,
                    Year = x.Year,
                    SongsCount = x.Song.Count,
                    Songs = x.Song.Select(y => new SongModel()
                    {
                        Id = y.SongId,
                        Title = y.Title,
                        Genre = y.Genre,
                        Year = y.Year,
                        ArtistId = y.ArtistId,
                        AlbumId = y.AlbumId
                    })
                };
            }
        }
    }
}