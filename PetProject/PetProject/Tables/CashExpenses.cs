using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Tables
{
    public class CashExpenses
    {
        [Column(TypeName = "ID")]
        public int ID { get; set; }

        [Column(TypeName = "UserID")]
        public string UserID { get; set; }

        [Column(TypeName = "ExpenseID")]
        public int ExpenseID { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "Cash")]
        public double Cash { get; set; }
    }
}
