#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWatchlist.Data;
using MovieWatchlist.Models;

namespace MovieWatchlist.Controllers
{
    /// <summary>
    /// Handles the dashboard (home) view.
    /// Requires the user to be authenticated.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext  _db;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(AppDbContext db, UserManager<IdentityUser> userManager)
        {
            _db          = db;
            _userManager = userManager;
        }

        // GET /
        // GET /Home/Index?filterStatus=watching
        public async Task<IActionResult> Index(string filterStatus = "all")
        {
            var userId = _userManager.GetUserId(User);

            var query = _db.WatchlistEntries
                           .Include(e => e.Movie)
                           .Where(e => e.UserId == userId);

            if (filterStatus != "all" && Enum.TryParse<WatchStatus>(filterStatus, true, out var status))
                query = query.Where(e => e.Status == status);

            var watchlist = await query
                .OrderBy(e => e.PriorityLevel)
                .ThenByDescending(e => e.DateAdded)
                .ToListAsync();

            // Count all entries (unfiltered) for stats cards
            var allEntries = await _db.WatchlistEntries
                .Where(e => e.UserId == userId)
                .ToListAsync();

            var vm = new DashboardViewModel
            {
                Watchlist      = watchlist,
                TotalCount     = allEntries.Count,
                WatchingCount  = allEntries.Count(e => e.Status == WatchStatus.Watching),
                WantCount      = allEntries.Count(e => e.Status == WatchStatus.Want),
                WatchedCount   = allEntries.Count(e => e.Status == WatchStatus.Watched),
                FilterStatus   = filterStatus
            };

            return View(vm);
        }
    }
}
