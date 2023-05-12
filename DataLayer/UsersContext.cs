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
    public class UsersContext : IDb<User, int>
    {
        private readonly GamesDBContext dbContext;
        public UsersContext(GamesDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Create(User item)
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

                dbContext.Users.Add(item);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public User Read(int key, bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<User> query = dbContext.Users;

                if (useNavigationalProperties)
                {
                    query = query.Include(u => u.Games).Include(u => u.Genres).Include(u => u.Friends);
                }

                return query.FirstOrDefault(g => g.Id == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<User> ReadAll(bool useNavigationalProperties = false)
        {
            try
            {
                IQueryable<User> query = dbContext.Users;

                if (useNavigationalProperties)
                {
                    query = query.Include(u => u.Games).Include(u => u.Genres).Include(u => u.Friends);
                }

                return query.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(User item, bool useNavigationalProperties = false)
        {
            try
            {
                User userFromDb = Read(item.Id, useNavigationalProperties);

                if (userFromDb == null)
                {
                    Create(item);
                    return;
                }
                userFromDb.FirstName = item.FirstName;
                userFromDb.LastName = item.LastName;
                userFromDb.Age = item.Age;
                userFromDb.UserName = item.UserName;
                userFromDb.Password = item.Password;
                userFromDb.Email = item.Email;

                if (useNavigationalProperties)
                {
                    List<User> friends = new List<User>();
                    foreach (User f in item.Friends)
                    {
                        User fFromDb = dbContext.Users.Find(f.Id);

                        if (fFromDb != null)
                        {
                            friends.Add(fFromDb);
                        }
                        else
                        {
                            friends.Add(f);
                        }
                    }
                    userFromDb.Friends = friends;

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
                    userFromDb.Games = games;

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
                    userFromDb.Genres = genres;
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
                User userFromDb = Read(key);
                if (userFromDb != null)
                {
                    dbContext.Users.Remove(userFromDb);
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("User with that id does not exist!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
