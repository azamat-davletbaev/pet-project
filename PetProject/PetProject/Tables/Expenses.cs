using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Tables
{
    public class Expenses
    {
        [Column(TypeName = "ID")]
        public int ID { get; set; }

        [Column(TypeName = "Name")]
        public string Name { get; set; }
    }
}
