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
                new Movie { Id = 8,  Title = "Pulp Fiction",              ReleaseYear = 1994, Genre = "Crime / Drama",          Description = "The lives of two mob hitmen, a boxer, and a pair of diner bandits intertwine in four tales of violence and redemption." },
                new Movie { Id = 9,  Title = "The Shawshank Redemption",  ReleaseYear = 1994, Genre = "Drama",                   Description = "Two imprisoned men bond over years, finding solace and eventual redemption through acts of common decency." },
                new Movie { Id = 10, Title = "Forrest Gump",              ReleaseYear = 1994, Genre = "Drama / Romance",          Description = "The presidencies of Kennedy and Johnson, Vietnam, Watergate, and other history unfold through the perspective of an Alabama man with a low IQ." },
                new Movie { Id = 11, Title = "The Lion King",             ReleaseYear = 1994, Genre = "Animation / Adventure",    Description = "Lion prince Simba flees his kingdom after his father's murder, only to return years later to reclaim his throne." },
                new Movie { Id = 12, Title = "Gladiator",                 ReleaseYear = 2000, Genre = "Action / Drama",           Description = "A Roman general is betrayed and his family murdered by a corrupt prince, who must then fight as a gladiator to avenge them." },
                new Movie { Id = 13, Title = "The Silence of the Lambs",  ReleaseYear = 1991, Genre = "Crime / Thriller",         Description = "A young FBI cadet seeks the counsel of imprisoned cannibal Dr. Hannibal Lecter to catch a serial killer known as Buffalo Bill." },
                new Movie { Id = 14, Title = "Schindler's List",          ReleaseYear = 1993, Genre = "Biography / Drama",        Description = "In German-occupied Poland, industrialist Oskar Schindler saves more than a thousand Jewish refugees from the Holocaust." },
                new Movie { Id = 15, Title = "Goodfellas",                ReleaseYear = 1990, Genre = "Crime / Drama",            Description = "The story of Henry Hill and his life in the mob, covering his career from young messenger to petty criminal to drug dealer and informant." },
                new Movie { Id = 16, Title = "The Avengers",              ReleaseYear = 2012, Genre = "Action / Sci-Fi",          Description = "Earth's mightiest heroes must come together to stop Loki and his alien army from enslaving humanity." },
                new Movie { Id = 17, Title = "Avatar",                    ReleaseYear = 2009, Genre = "Sci-Fi / Adventure",       Description = "A paraplegic marine dispatched to Pandora must choose between following orders and protecting the alien world he calls home." },
                new Movie { Id = 18, Title = "Titanic",                   ReleaseYear = 1997, Genre = "Drama / Romance",          Description = "A seventeen-year-old aristocrat falls in love with a kind but poor artist aboard the ill-fated R.M.S. Titanic." },
                new Movie { Id = 19, Title = "Jurassic Park",             ReleaseYear = 1993, Genre = "Sci-Fi / Adventure",       Description = "A wealthy entrepreneur secretly creates a theme park featuring living dinosaurs, but things go catastrophically wrong." },
                new Movie { Id = 20, Title = "The Truman Show",           ReleaseYear = 1998, Genre = "Drama / Sci-Fi",           Description = "An insurance salesman discovers his entire life is actually a television show and begins his quest to escape it." },
                new Movie { Id = 21, Title = "Fight Club",                ReleaseYear = 1999, Genre = "Drama / Thriller",         Description = "An insomniac office worker and a devil-may-care soap maker form an underground fight club that evolves into something more." },
                new Movie { Id = 22, Title = "Whiplash",                  ReleaseYear = 2014, Genre = "Drama / Music",            Description = "A promising young drummer enrolls at a cut-throat music conservatory where his teacher will stop at nothing to realise a student's potential." },
                new Movie { Id = 23, Title = "La La Land",                ReleaseYear = 2016, Genre = "Drama / Musical",          Description = "While navigating their careers in Los Angeles, a pianist and an aspiring actress fall in love while attempting to reconcile their aspirations for the future." },
                new Movie { Id = 24, Title = "Get Out",                   ReleaseYear = 2017, Genre = "Horror / Thriller",        Description = "A young African-American visits his white girlfriend's parents for the weekend, where his worst nightmares slowly materialize." },
                new Movie { Id = 25, Title = "Everything Everywhere All at Once", ReleaseYear = 2022, Genre = "Sci-Fi / Comedy",  Description = "A middle-aged Chinese immigrant is swept up in an outrageous adventure where she alone can save the world by exploring other universes connecting with lives she could have led." },
                new Movie { Id = 26, Title = "Oppenheimer",               ReleaseYear = 2023, Genre = "Biography / Drama",        Description = "The story of J. Robert Oppenheimer's role in the development of the atomic bomb during World War II." },
                new Movie { Id = 27, Title = "Dune",                      ReleaseYear = 2021, Genre = "Sci-Fi / Adventure",       Description = "A noble family becomes embroiled in a war for control over the galaxy's most valuable asset on a desert planet." },
                new Movie { Id = 28, Title = "Coco",                      ReleaseYear = 2017, Genre = "Animation / Adventure",    Description = "Aspiring musician Miguel enters the Land of the Dead to find his great-great-grandfather and lift a curse on his family." },
                new Movie { Id = 29, Title = "The Grand Budapest Hotel",   ReleaseYear = 2014, Genre = "Comedy / Drama",           Description = "A writer encounters the owner of an aging European hotel between the wars and discovers his adventures with a young lobby boy." },
                new Movie { Id = 30, Title = "Mad Max: Fury Road",        ReleaseYear = 2015, Genre = "Action / Sci-Fi",          Description = "In a post-apocalyptic wasteland, Max teams with Furiosa to flee from a cult leader and his army in a high-octane chase across the desert." }
            );
        }
    }
}
