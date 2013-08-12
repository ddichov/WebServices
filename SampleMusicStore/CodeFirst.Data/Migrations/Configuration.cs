namespace CodeFirst.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CodeFirst.Models;

    public sealed class Configuration : DbMigrationsConfiguration<MusicStore>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CodeFirst.Data.MusicStore context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var firstAlbum = new Album { Producer = "Sony Music", Title= "Rock my body", Year= 2013 };

            var secondAlbum = new Album { Producer = "Sony Music", Title = "All in one", Year = 2013 };

             var artists = new Artist[] 
                    { 
                        new Artist { Name = "Andrew Peters", Country = "USA", DateOfBirth = new DateTime(1965,5,23) },
                        new Artist { Name = "Brice Lambson", Country = "FR", DateOfBirth = new DateTime(1960,7,28) },
                        new Artist { Name = "G Miller", Country = "UK", DateOfBirth = new DateTime(1986,7,13) }
                    };

            var songs = new Song[] 
                    { 
                        new Song { Title = "Rock", Genre = "Rock and Roll", Year = 2013, Artist = artists[0], },
                        new Song { Title = "Move", Genre = "Hip-hop" , Year = 2012, Artist = artists[0] },
                        new Song { Title = "Sunny so sunny", Genre = "Pop", Year = 2011, Artist = artists[2] }
                    };

            for (int i = 0; i < artists.Length; i++)
            {
                context.Artists.AddOrUpdate(
                 a => a.Name,
                  artists[i]
                );

                context.Songs.AddOrUpdate(
                 s => s.Title,
                 songs[i]
               );

                secondAlbum.Song.Add(songs[i]);
            }

            context.Albums.AddOrUpdate(
                al => al.Title,
                secondAlbum,
                 firstAlbum
              );

        }
    }
}
