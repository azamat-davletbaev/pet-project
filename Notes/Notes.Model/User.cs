﻿namespace Notes.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Note> Notes { get; set; }
    }
}
