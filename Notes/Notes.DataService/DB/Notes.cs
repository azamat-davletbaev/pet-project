using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.DataService.DB
{
    /// <summary>
    /// Таблица Notes
    /// </summary>
    public class Note
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }        
        public string Text { get; set; }
        public User User { get; set; }
    }
}
