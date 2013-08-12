using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeFirst.Data;
using CodeFirst.Models;
using HW_WS_02.Models;

namespace HW_WS_02.Controllers
{
    public class SongController : ApiController
    {
        private readonly IRepository<Song> dataSong;

        public SongController()
        {
            this.dataSong = new EfRepository<Song>(new MusicStore());
        }

        public SongController(IRepository<Song> data)
        {
            this.dataSong = data;
        }

        // GET api/song
        public IQueryable<SongModel> Get()
        {
            var songs = this.dataSong.All().Select(SongModel.FromSong);
            return songs;
        }

        // GET api/song/5
        public SongModel Get(int id)
        {
            var song = this.dataSong.All().Where(x => x.SongId == id).Select(SongModel.FromSong).FirstOrDefault();
            return song;
        }

        // GET 
        public IQueryable<SongModel> Get(string title)
        {
            var song = this.dataSong.All().Where(x => x.Title == title).Select(SongModel.FromSong);
            return song;
        }

        // POST api/song
        public HttpResponseMessage Post([FromBody]
                                        SongModel value)
        {
            var song = value.CreateSong();

            var model = this.dataSong.Add(song);
           
            var message = this.Request.CreateResponse(HttpStatusCode.Created, model);
            //message.Headers.Location = new Uri(this.Request.RequestUri + song.SongId.ToString(CultureInfo.InvariantCulture));
            message.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = model.SongId }));
            return message;
        }

        // PUT api/song/5
        public HttpResponseMessage Put(int id, [FromBody]
                                       SongModel value)
        {
            var song = this.dataSong.Get(id);
            if (song == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Item not found!");
            }

            var model = this.dataSong.Update(id, value.UpdateSong(song));

            var message = this.Request.CreateResponse(HttpStatusCode.Created, model);
            //message.Headers.Location = new Uri(this.Request.RequestUri + song.SongId.ToString(CultureInfo.InvariantCulture));
            message.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = model.SongId }));
            return message;
        }

        // DELETE api/song/5
        public void Delete(int id)
        {
            this.dataSong.Delete(id);
        }
    }
}