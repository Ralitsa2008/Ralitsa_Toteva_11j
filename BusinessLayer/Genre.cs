using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Name can not be more than 20 symbols!")]
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public List<Game> Games { get; set; }
        public Genre()
        {
            Users = new List<User>();
            Games = new List<Game>();
        }
        public Genre(string name)
        {
            Name = name;
            Users = new List<User>();
            Games = new List<Game>();
        }
    }
}
