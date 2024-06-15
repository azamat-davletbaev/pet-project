using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.DI;
using WebAPI.Model;

namespace DataService.Data
{
    public class DataProvider : IDataProvider
    {
        private PostgresContext ctx;
        public DataProvider(PostgresContext _ctx)
        {
            ctx = _ctx;
        }

        public bool AddNote(Note note)
        {
            return false;   
        }

        public bool AddUser(User user)
        {
            return false;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return ctx.Users.Include(z => z.Notes).Select(u => new User 
            {
                Id = u.Id,
                Name = u.Name,
                Notes = u.Notes.Select(n => new Note 
                { 
                    Id = n.Id,
                    UserId = n.UserId,
                    Date = n.Date,
                    Text = n.Text,
                    User = new User { Id = u.Id, Name = u.Name }
                }).ToList()
            });
        }

        public IEnumerable<Note> GetNotesByUser(int UserId)
        {
            return default;
        }

        public User GetUser(int Id)
        {
            return default;
        }

        public bool RemoveNote(int Id)
        {
            return false;
        }

        public bool RemoveUser(int Id)
        {
            return false;
        }

        public bool UpdateNote(Note note)
        {
            return false;
        }

        public bool UpdateUser(User user)
        {
            return false;
        }
    }
}
