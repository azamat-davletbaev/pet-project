﻿
namespace Notes.Model
{
    public interface IDataProvider
    {
        public IEnumerable<User> GetAllUsers();
        public User GetUser(int Id);
        public IEnumerable<Note> GetNotesByUser(int Id);
        
        public bool AddUser(User user);
        public bool AddNote(Note note);

        public bool RemoveUser(int Id);
        public bool RemoveNote(int Id);

        public bool UpdateUser(User user);
        public bool UpdateNote(Note note);
    }
}
