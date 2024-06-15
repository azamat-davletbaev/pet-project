using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Notes.DataService.DB
{
    /// <summary>
    /// Таблица "Пользователи"
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Note> Notes { get; set; }
    }
}
