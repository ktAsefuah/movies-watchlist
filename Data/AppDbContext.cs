#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieWatchlist.Models;

namespace MovieWatchlist.Data
{
    /// <summary>
    /// EF Core database context.
    /// Extends IdentityDbContext so ASP.NET Core Identity tables
    /// (users, roles, claims) are included automatically.
    /// </summary>
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie>          Movies         { get; set; }
        public DbSet<WatchlistEntry> WatchlistEntries { get; set; }
        public DbSet<Review>         Reviews        { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Unique constraint: one entry per user per movie
            builder.Entity<WatchlistEntry>()
                .HasIndex(e => new { e.UserId, e.MovieId })
                .IsUnique();

            // Unique constraint: one review per user per movie
            builder.Entity<Review>()
                .HasIndex(r => new { r.UserId, r.MovieId })
                .IsUnique();

            // Seed demo movies
            builder.Entity<Movie>().HasData(
                new Movie { Id = 1, Title = "Inception",          ReleaseYear = 2010, Genre = "Sci-Fi / Thriller",   Description = "A thief who steals corporate secrets through dream-sharing technology is given the impossible task of planting an idea into the mind of a C.E.O." },
                new Movie { Id = 2, Title = "The Dark Knight",    ReleaseYear = 2008, Genre = "Action / Crime",       Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological tests of his ability to fight injustice." },
                new Movie { Id = 3, Title = "Interstellar",       ReleaseYear = 2014, Genre = "Sci-Fi / Adventure",   Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival." },
                new Movie { Id = 4, Title = "Parasite",           ReleaseYear = 2019, Genre = "Drama / Thriller",     Description = "Greed and class discrimination threaten the newly formed symbiotic relationship between two families." },
                new Movie { Id = 5, Title = "The Godfather",      ReleaseYear = 1972, Genre = "Crime / Drama",        Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son." },
                new Movie { Id = 6, Title = "The Matrix",         ReleaseYear = 1999, Genre = "Sci-Fi / Action",      Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers." },
                new Movie { Id = 7, Title = "Spirited Away",      ReleaseYear = 2001, Genre = "Animation / Fantasy",  Description = "A young girl becomes trapped in a magical spirit world after her parents are transformed into pigs." },
                new Movie { Id = 8, Title = "Pulp Fiction",       ReleaseYear = 1994, Genre = "Crime / Drama",        Description = "The lives of two mob hitmen, a boxer, and a pair of diner bandits intertwine in four tales of violence and redemption." }
            );
        }
    }
}
