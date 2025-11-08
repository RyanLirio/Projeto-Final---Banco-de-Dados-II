using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Data
{
    public class CineContext : DbContext
    {
        public CineContext(DbContextOptions<CineContext> options) : base(options) { }
    
        public DbSet<Person> Persons { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<DirectorMovie> DirectorMovies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Chair> Chairs { get; set; }
        public DbSet<Address> Addresses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Movie>().ToTable("Movie");
            modelBuilder.Entity<Gender>().ToTable("Gender");
            modelBuilder.Entity<DirectorMovie>().ToTable("DirectorMovie");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Cinema>().ToTable("Cinema");
            modelBuilder.Entity<Chair>().ToTable("Chair");
            modelBuilder.Entity<Address>().ToTable("Address");
        }
    }
}

