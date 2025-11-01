using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Data
{
    public class CineContext : DbContext
    {
        public CineContext(DbContextOptions<CineContext> options) : base(options) { }
    
        public DbSet<Person> Persons { get; set; }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Movie>().ToTable("Movie");
        }
    }
}
