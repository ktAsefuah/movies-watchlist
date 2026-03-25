#nullable disable
using System.ComponentModel.DataAnnotations;

namespace MovieWatchlist.Models
{
    // ── Login ─────────────────────────────────────────────────
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    // ── Register ──────────────────────────────────────────────
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(120)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    // ── Dashboard ─────────────────────────────────────────────
    public class DashboardViewModel
    {
        public List<WatchlistEntry> Watchlist { get; set; } = new();
        public int TotalCount     { get; set; }
        public int WatchingCount  { get; set; }
        public int WantCount      { get; set; }
        public int WatchedCount   { get; set; }
        public string FilterStatus { get; set; } = "all";
    }

    // ── Movie Detail / Edit ────────────────────────────────────
    public class DetailViewModel
    {
        public WatchlistEntry Entry  { get; set; }
        public Review         Review { get; set; }

        // Editable fields posted back
        public WatchStatus Status        { get; set; }
        public int         PriorityLevel { get; set; }
        public int         Rating        { get; set; }
        public string      Comment       { get; set; }
    }

    // ── Search ────────────────────────────────────────────────
    public class SearchViewModel
    {
        public string        Query   { get; set; }
        public List<Movie>   Results { get; set; } = new();
        public WatchStatus   AddStatus { get; set; } = WatchStatus.Want;
    }
}
