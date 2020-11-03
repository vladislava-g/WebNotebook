using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotebook.Models
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Notebook> Notebook { get; set; }
        public DbSet<DefaultImage> DefaultImage { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbContext(DbContextOptions<DbContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
    }
}
