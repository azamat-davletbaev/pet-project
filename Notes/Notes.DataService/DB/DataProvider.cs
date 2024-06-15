using Microsoft.EntityFrameworkCore;
using Notes.Model;
using User = Notes.Model.User;
using Note = Notes.Model.Note;

namespace Notes.DataService
{
    public class DataProvider : IDataProvider
    {
        private PostgresContext ctx;
        public DataProvider(PostgresContext _ctx) => ctx = _ctx;
        
        public bool AddNote(Note note)
        {
            return default;
        }

        public bool AddUser(User user)
        {
            return default;
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
            return default;
        }

        public bool RemoveUser(int Id)
        {
            return default;
        }

        public bool UpdateNote(Note note)
        {
            return default;
        }
                
        public bool UpdateUser(User user)
        {
            return default;
        }
    }
}
