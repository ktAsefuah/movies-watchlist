#nullable disable
using System.ComponentModel.DataAnnotations;

namespace MovieWatchlist.Models
{
    /// <summary>
    /// Represents a movie in the catalogue.
    /// </summary>
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, MinimumLength = 1)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Release year is required.")]
        [Range(1888, 2100, ErrorMessage = "Enter a valid release year.")]
        public int ReleaseYear { get; set; }

        [StringLength(120)]
        public string Genre { get; set; }

        public string Description { get; set; }

        public string PosterUrl { get; set; }

        // Navigation
        public ICollection<WatchlistEntry> WatchlistEntries { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
