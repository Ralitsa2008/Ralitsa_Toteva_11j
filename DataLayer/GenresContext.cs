using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class GenresContext : IDb<Genre, int>
    {
        private readonly GamesDBContext dbContext;

        public GenresContext(GamesDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(Genre item)
        {
            try
            {
                List<User> users = new();
                foreach (User user in item.Users)
                {
                    User userFromDb = dbContext.Users.Find(user.Id);
                    if (userFromDb != null)
                    {
                        users.Add(userFromDb);
                    }
                    else
                    {
                        users.Add(user);
                    }
                }
                item.Users = users;

                List<Game> games = new();
                foreach (Game game in item.Games)
                {
                    Game gameFromDb = dbContext.Games.Find(game.Id);
                    if (gameFromDb != null)
                    {
                        games.Add(gameFromDb);
                    }
                    else
                    {
                        games.Add(game);
                    }
                }
                item.Games = games;

                dbContext.Genres.Add(item);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Genre Read(int key, bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<Genre> query = dbContext.Genres;

                if (useNavigationalProperties)
                {
                    query = query.Include(g => g.Users).Include(g => g.Games);
                }

                return query.FirstOrDefault(g => g.Id == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Genre> ReadAll(bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<Genre> query = dbContext.Genres;

                if (useNavigationalProperties)
                {
                    query = query.Include(g => g.Users).Include(g => g.Games);
                }

                return query.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(Genre item, bool useNavigationalProperties = false)
        {
            try
            {
                Genre genreFromDb = Read(item.Id, useNavigationalProperties);

                if (genreFromDb == null)
                {
                    Create(item);
                    return;
                }
                genreFromDb.Name = item.Name;

                if (useNavigationalProperties)
                {

                    List<User> users = new List<User>();

                    foreach (User u in item.Users)
                    {
                        User uFromDb = dbContext.Users.Find(u.Id);

                        if (uFromDb != null)
                        {
                            users.Add(uFromDb);
                        }
                        else
                        {
                            users.Add(u);
                        }
                    }
                    genreFromDb.Users = users;

                    List<Game> games = new List<Game>();

                    foreach (Game g in item.Games)
                    {
                        Game gFromDb = dbContext.Games.Find(g.Id);

                        if (gFromDb != null)
                        {
                            games.Add(gFromDb);
                        }
                        else
                        {
                            games.Add(g);
                        }
                    }
                    genreFromDb.Games = games;
                }

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(int key)
        {
            try
            {
                Genre genreFromDb = Read(key);

                if (genreFromDb != null)
                {
                    dbContext.Genres.Remove(genreFromDb);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Genre with that id does not exist!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
