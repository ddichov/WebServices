using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using HW_WS_02.Models;

namespace WebApi
{
    /*
    Id Title           Producer        Year  SongsCount
    1 Album1          PROD 1          1234  3
    2 Album2          Prod 2          1111  2
    3 NewAlbum 1      New Producer 1  1990  0
    4 updated         New Producer 2  2020  1
    5 NewAlbum 1      New Producer 1  1990  0
    6 NewAlbum 2      New Producer 2  1990  0
    8 NewAlbum 2      New Producer 2  1990  0
    9 Best Album      Mitko Pionera   2010  1
     * 
    4 Artist1    BG         10.10.2012 г. 00:00:00 ч.
    5 Artist2    BG         1.1.2011 г. 00:00:00 ч.
    6 Ivanco     G          4.10.1980 г. 00:00:00 ч.
    7 Pepa XXL   UK         4.10.1970 г. 00:00:00 ч.
    8 VanKo      Fr         4.10.1970 г. 00:00:00 ч.
     * 
    Id Title           Genre           Year  ArtistId AlbumId
    1 song1           xxx             1234  4        1
    2 song2           xxx             1321  5        1
    3 NewSong 1       New Gener G4    1992  5
    4 NewSong 1       New Gener G4    1992  5
    5 TTTTT           New Gener G4    1992  4        1
    6 NewSong 1       New Gener G4    1992  5
    7 NewSong 1       New Gener G4    1992  5
    8 UpdatedSong     New Gener G4    1968  6        2
    10 pesen           pop             1988  6
    11 pesen           Folk            1988  6
    12 pesen           Folk            1988  6        4
    13 pesen           Folk            1988  8
    14 pesen           Folk            1988  8        2
    15 Rap pesen       Rap             1987  7        9
    */
    class Program
    {
        private static readonly HttpClient Client = new HttpClient { BaseAddress = new Uri("http://localhost:61050/") };

        static void Main(string[] args)
        {
            /*
            * The GetAsync method sends an HTTP GET request. As the name implies, GetAsyc is asynchronous.
            * It returns immediately, without waiting for a response from the server.
            * The return value is a Task object that represents the asynchronous operation.
            * When the operation completes, the Task.Result property contains the HTTP response.
            */
            // Add an Accept header for JSON format.
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine();
            // AddNewAlbum(new AlbumModel { Title="Best Album", Producer="Mitko Pionera", Year=2010 });
            // UpdateAlbum(4, new AlbumModel { Title = "updated", Year=2020 });
            // DeleteAlbum(7);
            PrintAlbums(GetAllAlbums());

            Console.WriteLine();
            //AddNewArtist(new ArtistModel { Country = "CH", Name = "ToBeDeleted", DateOfBirth = new DateTime(1943, 10, 4) });
            //UpdateArtist(7, new ArtistModel { Country="UK", Name="Pepa XXL" });
            //DeleteArtist(9);
            PrintArtists(GetAllArtists());

            Console.WriteLine();
            //AddNewSong(new SongModel { Title="Rap pesen", Genre= "Rap", Year = 1987, ArtistId = 7, AlbumId = 9});
            //UpdateSong(8, new SongModel { Title="UpdatedSong", ArtistId=6, Year=1968 });
            //DeleteSong(9);
            PrintSongs(GetAllSongs());

            Console.WriteLine();
            PrintAlbumDetails(GetAlbum(1));
           
            Console.WriteLine(GetArtist(6).Name);
            Console.WriteLine(GetSong(5).Title);
        }

        internal static void PrintAlbums(IEnumerable<AlbumModel> data)
        {
            Console.WriteLine("{0,4} {1,-15} {2,-15} {3,-5} {4,-8}",
                "Id", "Title", "Producer", "Year", "SongsCount");
            foreach (var a in data)
            {
                Console.WriteLine("{0,4} {1,-15} {2,-15} {3,-5} {4,-8}",
                    a.Id, a.Title, a.Producer, a.Year, a.SongsCount);
            }
        }

        internal static void PrintAlbumDetails(AlbumFullModel album)
        {
            Console.WriteLine(album.Title);
            Console.WriteLine("{0,4} {1,-15} {2,-15} {3,-5} {4,-8} {5,-8}",
                "Id", "Title", "Genre", "Year", "ArtistId", "AlbumId");
            foreach (var s in album.Songs)
            {
                Console.WriteLine("{0,4} {1,-15} {2,-15} {3,-5} {4,-8} {5,-8}",
                    s.Id, s.Title, s.Genre, s.Year, s.ArtistId, s.AlbumId);
            }
        }

        internal static IEnumerable<AlbumModel> GetAllAlbums()
        {
            HttpResponseMessage responseAM = Client.GetAsync("api/album").Result; // Blocking call!

            if (responseAM.IsSuccessStatusCode)
            {
                return responseAM.Content.ReadAsAsync<IEnumerable<AlbumModel>>().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)responseAM.StatusCode, responseAM.ReasonPhrase); // ?
            }
            return null;
        }

        internal static AlbumFullModel GetAlbum(int id)
        {
            string url = "api/album/" + id;
            HttpResponseMessage responseAM = Client.GetAsync(url).Result;

            if (responseAM.IsSuccessStatusCode)
            {
                return responseAM.Content.ReadAsAsync<AlbumFullModel>().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)responseAM.StatusCode, responseAM.ReasonPhrase); // ?
            }
            return null;
        }

        internal static void AddNewAlbum(AlbumModel album)
        {
            var response = Client.PostAsJsonAsync("api/album", album).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Album added!");
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        internal static void UpdateAlbum(int id, AlbumModel album)
        {
            string url = "api/album/" + id;
            var response = Client.PutAsJsonAsync(url, album).Result;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Album updated!");
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        internal static void DeleteAlbum(int id)
        {
            string url = "api/album/" + id;
            HttpResponseMessage response = Client.DeleteAsync(url).Result;
           
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Album with Id={0} deleted!", id);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        internal static void DeleteAlbumByTitle(string title)
        {
            string url = "api/album?albumTitle=" + title;
            HttpResponseMessage response = Client.GetAsync(url).Result;
            
            if (response.IsSuccessStatusCode)
            {
                var albums = response.Content.ReadAsAsync<IEnumerable<AlbumModel>>().Result;
                foreach (var a in albums)
                {
                    if (a.Title == title)
                    {
                        DeleteAlbum(a.Id);
                    }
                }
            }
        }

        // Song

        internal static void PrintSongs(IEnumerable<SongModel> data)
        {
            Console.WriteLine("{0,4} {1,-15} {2,-15} {3,-5} {4,-8} {5,-8}",
                "Id", "Title", "Genre", "Year", "ArtistId", "AlbumId");
            foreach (var s in data)
            {
                Console.WriteLine("{0,4} {1,-15} {2,-15} {3,-5} {4,-8} {5,-8}",
                    s.Id, s.Title, s.Genre, s.Year, s.ArtistId, s.AlbumId);
            }
        }

        internal static IEnumerable<SongModel> GetAllSongs()
        {
            HttpResponseMessage responseAM = Client.GetAsync("api/song").Result; // Blocking call!

            if (responseAM.IsSuccessStatusCode)
            {
                return responseAM.Content.ReadAsAsync<IEnumerable<SongModel>>().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)responseAM.StatusCode, responseAM.ReasonPhrase); // ?
            }
            return null;
        }

        internal static SongModel GetSong(int id)
        {
            string url = "api/song/" + id;
            HttpResponseMessage responseSong = Client.GetAsync(url).Result;

            if (responseSong.IsSuccessStatusCode)
            {
                return responseSong.Content.ReadAsAsync<SongModel>().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)responseSong.StatusCode, responseSong.ReasonPhrase); // ?
            }
            return null;
        }

        internal static void AddNewSong(SongModel song)
        {
            var response = Client.PostAsJsonAsync("api/song", song).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Song added!");
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        internal static void UpdateSong(int id, SongModel song)
        {
            var response = Client.PutAsJsonAsync("api/song/" + id, song).Result;
           
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Song updated!");
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        internal static void DeleteSong(int id)
        {
            string url = "api/song/" + id;
            HttpResponseMessage response = Client.DeleteAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Song with Id={0} deleted!", id);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        internal static void DeleteSongByTitle(string title)
        {
            string url = "api/song?songTitle=" + title;
            HttpResponseMessage response = Client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var songs = response.Content.ReadAsAsync<IEnumerable<SongModel>>().Result;
                foreach (var s in songs)
                {
                    if (s.Title == title)
                    {
                        DeleteSong(s.Id);
                    }
                }
            }
        }

        // Artist

        internal static void PrintArtists(IEnumerable<ArtistModel> data)
        {
            foreach (var a in data)
            {
                Console.WriteLine("{0,4} {1,-10} {2,-10} {3}",
                    a.ArtistId, a.Name, a.Country, a.DateOfBirth);
            }
        }

        internal static IEnumerable<ArtistModel> GetAllArtists()
        {
            HttpResponseMessage responseAM = Client.GetAsync("api/artist").Result; // Blocking call!

            if (responseAM.IsSuccessStatusCode)
            {
                return responseAM.Content.ReadAsAsync<IEnumerable<ArtistModel>>().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)responseAM.StatusCode, responseAM.ReasonPhrase); // ?
            }
            return null;
        }

        internal static ArtistModel GetArtist(int id)
        {
            string url = "api/artist/" + id;
            HttpResponseMessage responseSong = Client.GetAsync(url).Result;

            if (responseSong.IsSuccessStatusCode)
            {
                return responseSong.Content.ReadAsAsync<ArtistModel>().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)responseSong.StatusCode, responseSong.ReasonPhrase); // ?
            }
            return null;
        }

        internal static void AddNewArtist(ArtistModel artist)
        {
            //var song = new SongModel { Title = title, Genre = gener, Year = year, ArtistId = artistId };
            var response = Client.PostAsJsonAsync("api/artist", artist).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Artist added!");
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        internal static void UpdateArtist(int id, ArtistModel artist)
        {
            string url = "api/artist/" + id;
            var response = Client.PutAsJsonAsync(url, artist).Result;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Artist updated!");
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        internal static void DeleteArtist(int id)
        {
            string url = "api/artist/" + id;
            HttpResponseMessage response = Client.DeleteAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Artist with Id={0} deleted!", id);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }

        internal static void DeleteArtistByName(string name)
        {
            string url = "api/artist?artistName=" + name;
            HttpResponseMessage response = Client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var songs = response.Content.ReadAsAsync<IEnumerable<ArtistModel>>().Result;
                foreach (var s in songs)
                {
                    if (s.Name == name)
                    {
                        DeleteArtist(s.ArtistId);
                    }
                }
            }
        }
    }
}