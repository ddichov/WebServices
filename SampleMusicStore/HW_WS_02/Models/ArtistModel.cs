using System;
using System.Linq;
using System.Linq.Expressions;
using CodeFirst.Models;

namespace HW_WS_02.Models
{
    public class ArtistModel
    {
        public static Expression<Func<Artist, ArtistModel>> FromAlbum
        {
            get
            {
                return x => new ArtistModel 
                {
                    ArtistId = x.ArtistId,
                    Name = x.Name,
                    Country = x.Country,
                    DateOfBirth = x.DateOfBirth
                };
            }
        }

        public int ArtistId { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Artist CreateArtist()
        {
            return new Artist 
            {
                Name = this.Name,
                Country = this.Country,
                DateOfBirth = this.DateOfBirth,
                ArtistId = this.ArtistId
            };
        }

        public Artist UpdateArtist(Artist artist)
        {
            if (this.Name != null)
            {
                artist.Name = this.Name;
            }

            if (this.Country != null)
            {
                artist.Country = this.Country;
            }

            if (this.DateOfBirth != new DateTime())
            {
                artist.DateOfBirth = this.DateOfBirth;
            }

            return artist;
        }
    }
}