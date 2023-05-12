using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class GamesContext : IDb<Game, int>
    {
        private readonly GamesDBContext dbContext;
        public GamesContext(GamesDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Create(Game item)
        {
            try
            {
                List<Genre> genres = new();
                foreach (Genre genre in item.Genres)
                {
                    Genre genreFromDb = dbContext.Genres.Find(genre.Id);
                    if (genreFromDb != null)
                    {
                        genres.Add(genreFromDb);
                    }
                    else
                    {
                        genres.Add(genre);
                    }
                }
                item.Genres = genres;

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

                dbContext.Games.Add(item);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Game Read(int key, bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<Game> query = dbContext.Games;

                if (useNavigationalProperties)
                {
                    query = query.Include(g => g.Users).Include(g => g.Genres);
                }

                return query.FirstOrDefault(g => g.Id == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Game> ReadAll(bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<Game> query = dbContext.Games;

                if (useNavigationalProperties)
                {
                    query = query.Include(g => g.Users).Include(g => g.Genres);
                }

                return query.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(Game item, bool useNavigationalProperties = false)
        {
            try
            {
                Game gameFromDb = Read(item.Id, useNavigationalProperties);

                if (gameFromDb == null)
                {
                    Create(item);
                    return;
                }
                gameFromDb.Name = item.Name;

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
                    gameFromDb.Users = users;

                    List<Genre> genres = new List<Genre>();

                    foreach (Genre g in item.Genres)
                    {
                        Genre gFromDb = dbContext.Genres.Find(g.Id);

                        if (gFromDb != null)
                        {
                            genres.Add(gFromDb);
                        }
                        else
                        {
                            genres.Add(g);
                        }
                    }
                    gameFromDb.Genres = genres;
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
                Game gameFromDb = Read(key);

                if (gameFromDb != null)
                {
                    dbContext.Games.Remove(gameFromDb);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Game with that id does not exist!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
