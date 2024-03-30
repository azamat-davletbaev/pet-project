using DataService.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Tables;

namespace DataService
{
    public class PostgresContext : DbContext
    {
        public DbSet<Users> USERS { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<CashExpenses> CashExpenses { get; set; }
        public DbSet<CachAccounts> CachAccounts { get; set; }

        public PostgresContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres;Pooling=true;SearchPath=cash;");
        }
    }
}
