using Cine_Ma.Models;
using Microsoft.EntityFrameworkCore;

namespace Cine_Ma.Data
{
    public class CineContext : DbContext
    {
        public CineContext(DbContextOptions<CineContext> options) : base(options) { }
    
        //public DbSet<Person> Persons { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Sex> Sexes { get; set; }
        //public DbSet<DirectorMovie> DirectorMovies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Chair> Chairs { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Session> Sessions { get; set; }
        //public DbSet<Studio> Studios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<SexMovie> SexMovies { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<CinemaRoom> CinemaRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Movie>().ToTable("Movie");
            modelBuilder.Entity<Sex>().ToTable("Sex");
            //modelBuilder.Entity<DirectorMovie>().ToTable("DirectorMovie");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Cinema>().ToTable("Cinema");
            modelBuilder.Entity<Chair>().ToTable("Chair");
            modelBuilder.Entity<Ticket>().ToTable("Ticket");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Session>().ToTable("Session");
            //modelBuilder.Entity<Studio>().ToTable("Studio");
            modelBuilder.Entity<SexMovie>().ToTable("SexMovie");
            modelBuilder.Entity<ProductOrder>().ToTable("ProductOrder");
            modelBuilder.Entity<CinemaRoom>().ToTable("CinemaRoom");

            // Definindo as chaves estrangeiras
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Session)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.SessionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Chair)
                .WithMany()
                .HasForeignKey(t => new { t.RoomId, t.Column, t.Row })
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasIndex(t => new { t.SessionId, t.Column, t.Row, t.RoomId })
                .IsUnique();

            modelBuilder.Entity<Chair>()
                .HasKey(c => new { c.RoomId, c.Column, c.Row });

            modelBuilder.Entity<Chair>()
                .HasOne(c => c.Room)
                .WithMany(r => r.Chairs)
                .HasForeignKey(c => c.RoomId);

            modelBuilder.Entity<Cinema>()
                .HasOne(c => c.Address)
                .WithMany()
                .HasForeignKey(c => c.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cinema>()
                .HasIndex(c => c.Cnpj)
                .IsUnique();

            modelBuilder.Entity<Cinema>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<CinemaRoom>()
                .HasOne(c => c.Cinema)
                .WithMany(c => c.Rooms)
                .HasForeignKey(c => c.CinemaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Client>()
                .HasOne(c => c.Address)
                .WithMany()
                .HasForeignKey(c => c.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Cpf)
                .IsUnique();

            modelBuilder.Entity<Language>()
                .HasIndex(l => l.Name)
                .IsUnique();

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Language)
                .WithMany()
                .HasForeignKey(m => m.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Movie>()
              .HasMany(m => m.SexMovies)
              .WithOne(sm => sm.Movie)
              .HasForeignKey(sm => sm.MovieId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.ProductOrders)
                .WithOne(po => po.Order)
                .HasForeignKey(po => po.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Tickets)
                .WithOne(t => t.Order)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductOrder>()
                .HasKey(po => new { po.OrderId, po.ProductId });

            modelBuilder.Entity<Product>()
              .HasMany(p => p.ProductOrders)
              .WithOne(po => po.Product)
              .HasForeignKey(po => po.ProductId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Movie)
                .WithMany()
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.LanguageAudio)
                .WithMany()
                .HasForeignKey(s => s.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.LanguageCaption)
                .WithMany()
                .HasForeignKey(s => s.CaptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.CinemaRoom)
                .WithMany()
                .HasForeignKey(s => s.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sex>()
              .HasMany(s => s.SexMovies)
              .WithOne(sm => sm.Sex)
              .HasForeignKey(sm => sm.SexId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SexMovie>()
                .HasKey(sm => new { sm.SexId, sm.MovieId });
        }
    }
}

