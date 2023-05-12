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
    public class GamesContextTest
    {
        private GamesContext context = new(SetupFixture.dbContext);
        private Game game;
        private User user;
        private Genre genre;

        [SetUp]
        public void CreateGame()
        {
            game = new("FIFA");
            genre = new("Football");
            user = new("Raitsa", "Toteva", 17, "RalitsaToteva", "7865", "ralitsatoteva_zh19@schoolmath.eu");

            game.Genres.Add(genre);
            game.Users.Add(user);

            context.Create(game);
        }

        [TearDown]
        public void DropGame()
        {
            foreach (Game item in SetupFixture.dbContext.Games.ToList())
            {
                SetupFixture.dbContext.Games.Remove(item);
            }

            SetupFixture.dbContext.SaveChanges();
        }

        [Test]
        public void Create()
        {
            Game testGame = new("Minecraft");
            int gamesBefore = SetupFixture.dbContext.Games.Count();
            context.Create(testGame);
            int gamesAfter = SetupFixture.dbContext.Games.Count();
            Assert.That(gamesBefore + 1 == gamesAfter, "Create() does not work!");
        }

        [Test]
        public void Read()
        {
            Game readGame = context.Read(game.Id);
            Assert.AreEqual(game, readGame, "Read() does not return the same object!");
        }

        [Test]
        public void ReadWithNavigationalProperties()
        {
            Game readGame = context.Read(game.Id, true);
            Assert.That(readGame.Users.Contains(user), "User is not in the Users list!");
            Assert.That(readGame.Genres.Contains(genre), "Genre is not in the Genres list!");
        }

        [Test]
        public void ReadAll()
        {
            List<Game> games = (List<Game>)context.ReadAll();
            Assert.That(games.Count != 0, "ReadAll() does not return games!");
        }

        [Test]
        public void ReadAllWithNavigationalProperties()
        {
            Game readGame = new("Minecraft");
            Genre genre1 = new("Entertaining");
            User user1 = new("Mariya", "Petrova", 17, "MariyaPetrova", "6703", "mariyapetrova_zh19@schoolmath.eu");
            SetupFixture.dbContext.Users.Add(user);
            SetupFixture.dbContext.Genres.Add(genre1);
            SetupFixture.dbContext.Games.Add(readGame);

            List<Game> games = (List<Game>)context.ReadAll(true);
            Assert.That(games.Count != 0 && context.Read(readGame.Id, true).Genres.Count == 1
                && context.Read(readGame.Id, true).Users.Count == 1, "ReadAll() does not return games!");
        }

        [Test]
        public void Update()
        {
            Game changedGame = context.Read(game.Id);

            changedGame.Name = "Updated " + game.Name;

            context.Update(changedGame);
            game = context.Read(game.Id);

            Assert.AreEqual(changedGame, game, "Update() does not work!");
        }

        [Test]
        public void Delete()
        {
            int gamesBefore = SetupFixture.dbContext.Games.Count();
            context.Delete(game.Id);
            int gamesAfter = SetupFixture.dbContext.Games.Count();
            Assert.IsTrue(gamesBefore - 1 == gamesAfter, "Delete() does not work!");
        }
    }
}
