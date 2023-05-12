using BusinessLayer;
using DataLayer;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TestingLayer
{
    [TestFixture]
    public class GenreContextTest
    {
        private GenresContext context = new(SetupFixture.dbContext);
        private Genre genre;
        private Game game;
        private User user;

        [SetUp]
        public void CreateGenre()
        {
            genre = new("Football");
            game = new("FIFA");
            user = new("Ralitsa", "Toteva", 17, "RalitsaToteva", "7865", "ralitsatoteva_zh19@schoolmath.eu");

            genre.Users.Add(user);
            genre.Games.Add(game);

            context.Create(genre);
        }

        [TearDown]
        public void DropGenre()
        {
            foreach (Genre item in SetupFixture.dbContext.Genres.ToList())
            {
                SetupFixture.dbContext.Genres.Remove(item);
            }
            SetupFixture.dbContext.SaveChanges();
        }

        [Test]
        public void Create()
        {
            Genre testGenre = new("Comedy");
            int genresBefore = SetupFixture.dbContext.Genres.Count();
            context.Create(testGenre);
            int genresAfter = SetupFixture.dbContext.Genres.Count();
            Assert.That(genresBefore + 1 == genresAfter, "Create() does not work!");
        }

        [Test]
        public void Read()
        {
            Genre readGenre = context.Read(genre.Id);
            Assert.AreEqual(genre, readGenre, "Read() does not return the same object!");
        }

        [Test]
        public void ReadWithNavigationalProperties()
        {
            Genre readGenre = context.Read(genre.Id, true);
            Assert.That(readGenre.Users.Contains(user), "User is not in the Users list!");
            Assert.That(readGenre.Games.Contains(game), "Game is not in the Games list!");
        }

        [Test]
        public void ReadAll()
        {
            List<Genre> genres = (List<Genre>)context.ReadAll();
            Assert.That(genres.Count != 0, "ReadAll() does not return genres!");
        }

        [Test]
        public void ReadAllWithNavigationalProperties()
        {
            Genre readGenre = new("Entertaining");
            Game genre1 = new("Minecraft");
            User user1 = new("Mariya", "Petrova", 17, "MariyaPetrova", "6703", "mariyapetrova_zh19@schoolmath.eu");
            SetupFixture.dbContext.Users.Add(user);
            SetupFixture.dbContext.Games.Add(genre1);
            SetupFixture.dbContext.Genres.Add(readGenre);

            List<Genre> games = (List<Genre>)context.ReadAll(true);
            Assert.That(games.Count != 0 && context.Read(readGenre.Id, true).Games.Count == 1
                && context.Read(readGenre.Id, true).Users.Count == 1, "ReadAll() does not return genres!");
        }

        [Test]
        public void Update()
        {
            Genre changedGenre = context.Read(genre.Id);

            changedGenre.Name = "Updated " + genre.Name;

            context.Update(changedGenre);
            genre = context.Read(genre.Id);

            Assert.AreEqual(changedGenre, genre, "Update() does not work!");
        }

        [Test]
        public void Delete()
        {
            int genresBefore = SetupFixture.dbContext.Genres.Count();
            context.Delete(genre.Id);
            int genresAfter = SetupFixture.dbContext.Genres.Count();
            Assert.IsTrue(genresBefore - 1 == genresAfter, "Delete() does not work!");
        }
    }
}
