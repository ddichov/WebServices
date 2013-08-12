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
    public class AlbumController : ApiController
    {
        private readonly IRepository<Album> dataAlbum;

        public AlbumController(IRepository<Album> data)
        {
            this.dataAlbum = data;
        }

        public AlbumController()
        {
            this.dataAlbum = new EfRepository<Album>(new MusicStore());
        }

        // GET api/album
        public IQueryable<AlbumModel> Get()
        {
            var album = this.dataAlbum.All().Select(AlbumModel.FromAlbum);
            return album;
        }

        public IQueryable<AlbumModel> Get(string albumTitle)
        {
            var album = this.dataAlbum.All().Where(x => x.Title == albumTitle).Select(AlbumModel.FromAlbum);
            return album;
        }

        public HttpResponseMessage Get(int id)
        {
            var album = this.dataAlbum.All().Where(x => x.AlbumId == id).Select(AlbumFullModel.FromFullAlbum).FirstOrDefault();
            if (album == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item not found!");
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, album);
        }

        // POST api/album
        public HttpResponseMessage Post([FromBody]AlbumModel value)
        {
            var album = value.CreateAlbum();

            var result = this.dataAlbum.Add(album);

            var message = this.Request.CreateResponse(HttpStatusCode.Created, result);
            message.Headers.Location = new Uri(this.Request.RequestUri + result.AlbumId.ToString(CultureInfo.InvariantCulture));
            return message;
        }

        // PUT api/album/5
        public HttpResponseMessage Put(int id, [FromBody]AlbumModel value)
        {
            var album = this.dataAlbum.Get(id);

            if (album == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item not found!");
            }

            var result = this.dataAlbum.Update(id, value.UpdateAlbum(album)); // YES!

            var message = this.Request.CreateResponse(HttpStatusCode.Accepted, result);
            message.Headers.Location = new Uri(this.Request.RequestUri + result.AlbumId.ToString(CultureInfo.InvariantCulture));
            return message;
        }

        // DELETE api/album/5
        public void Delete(int id)
        {
            this.dataAlbum.Delete(id);
        }
    }
}
