using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeFirst.Data;
using CodeFirst.Models;
using HW_WS_02.Models;

namespace HW_WS_02.Controllers
{
    public class ArtistController : ApiController
    {
        private readonly IRepository<Artist> dataArtist;

        public ArtistController()
        {
            this.dataArtist = new EfRepository<Artist>(new MusicStore());
        }

        public ArtistController(IRepository<Artist> data)
        {
            this.dataArtist = data;
        }

        // GET api/Artist
        public IQueryable<ArtistModel> Get()
        {
            var artist = this.dataArtist.All().Select(ArtistModel.FromAlbum);
            return artist;
        }

        public IQueryable<ArtistModel> Get(string artistName)
        {
            var artist = this.dataArtist.All().Where(x => x.Name == artistName).Select(ArtistModel.FromAlbum);
            return artist;
        }

        // GET api/Artist/5
        public HttpResponseMessage Get(int id)
        {
            var artist = this.dataArtist.All().Where(x => x.ArtistId == id).Select(ArtistModel.FromAlbum).FirstOrDefault();
            if (artist == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item not found!");
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, artist);
        }
        
        // PUT api/Artist/5
        public HttpResponseMessage Put(int id, [FromBody] ArtistModel value)
        {
            var artist = this.dataArtist.Get(id);

            if (artist == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item not found!");
            }

            var result = this.dataArtist.Update(id, value.UpdateArtist(artist)); // YES!

            var message = this.Request.CreateResponse(HttpStatusCode.Accepted, result);
            message.Headers.Location = new Uri(this.Request.RequestUri + result.ArtistId.ToString(CultureInfo.InvariantCulture));
            return message;
        }

        // POST api/Artist
        public HttpResponseMessage Post([FromBody] ArtistModel value)
        {
            var artist = value.CreateArtist();

            var result = this.dataArtist.Add(artist);

            var message = this.Request.CreateResponse(HttpStatusCode.Created, result);
            message.Headers.Location = new Uri(this.Request.RequestUri + result.ArtistId.ToString(CultureInfo.InvariantCulture));
            return message;
        }

        // DELETE api/Artist/5
        public void Delete(int id)
        {
            this.dataArtist.Delete(id);
        }
    }
}