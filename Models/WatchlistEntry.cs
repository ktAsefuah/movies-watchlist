#nullable disable
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MovieWatchlist.Models
{
    public enum WatchStatus { Want, Watching, Watched, Dropped }

    /// <summary>
    /// Join table between ApplicationUser and Movie.
    /// Stores status, priority, and date added.
    /// </summary>
    public class WatchlistEntry
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        [Required]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public WatchStatus Status { get; set; } = WatchStatus.Want;

        [Range(1, 5)]
        public int PriorityLevel { get; set; } = 3;

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    }
}
