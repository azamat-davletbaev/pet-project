using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Tables
{
    public class CachAccounts
    {
        [Column(TypeName = "ID")]
        public int ID { get; set; }

        [Column(TypeName = "UserID")]
        public string UserID { get; set; }

        [Column(TypeName = "Cash")]
        public double Cash { get; set; }
    }
}
