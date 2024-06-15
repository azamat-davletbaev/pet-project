using Notes.DataService.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Reflection.Emit;

namespace Notes.DataService
{
    public class PostgresContext : DbContext
    {
        private static string connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres;";

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }        

        public PostgresContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().HasKey(p => p.Id);
            modelBuilder.Entity<User>().Property(p => p.Id).UseSerialColumn();
            modelBuilder.Entity<User>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<Note>().ToTable("Notes");
            modelBuilder.Entity<Note>().HasKey(p => p.Id);
            modelBuilder.Entity<Note>().Property(p => p.Id).UseSerialColumn();
            modelBuilder.Entity<Note>().Property(p => p.Date).IsRequired();
            modelBuilder.Entity<Note>().Property(p => p.Text).IsRequired();
            modelBuilder.Entity<Note>().HasOne(x => x.User).WithMany(s=>s.Notes).HasForeignKey(x => x.UserId);
                        
            base.OnModelCreating(modelBuilder);
        }
    }
}
