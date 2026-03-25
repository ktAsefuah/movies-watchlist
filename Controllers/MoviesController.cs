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
    /// Handles all movie and watchlist operations:
    /// Search, Add to Watchlist, Detail/Edit, Delete, History.
    /// </summary>
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public MoviesController(AppDbContext db, UserManager<IdentityUser> userManager)
        {
            _db          = db;
            _userManager = userManager;
        }

        // ── SEARCH ────────────────────────────────────────────────────────────
        // GET /Movies/Search?query=inception
        public async Task<IActionResult> Search(string query)
        {
            var vm = new SearchViewModel { Query = query };

            if (!string.IsNullOrWhiteSpace(query))
            {
                var q = query.Trim().ToLower();
                vm.Results = await _db.Movies
                    .Where(m => m.Title.ToLower().Contains(q) || m.Genre.ToLower().Contains(q))
                    .OrderBy(m => m.Title)
                    .ToListAsync();
            }

            return View(vm);
        }

        // ── ADD TO WATCHLIST ──────────────────────────────────────────────────
        // POST /Movies/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int movieId, WatchStatus status)
        {
            var userId = _userManager.GetUserId(User);

            var existing = await _db.WatchlistEntries
                .FirstOrDefaultAsync(e => e.UserId == userId && e.MovieId == movieId);

            if (existing != null)
            {
                existing.Status = status;
            }
            else
            {
                _db.WatchlistEntries.Add(new WatchlistEntry
                {
                    UserId  = userId,
                    MovieId = movieId,
                    Status  = status
                });
            }

            await _db.SaveChangesAsync();
            TempData["Success"] = "Movie added to your watchlist!";
            return RedirectToAction("Index", "Home");
        }

        // ── DETAIL / EDIT ─────────────────────────────────────────────────────
        // GET /Movies/Detail/5
        public async Task<IActionResult> Detail(int id)
        {
            var userId = _userManager.GetUserId(User);

            var entry = await _db.WatchlistEntries
                .Include(e => e.Movie)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entry == null) return NotFound();

            var review = await _db.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == entry.MovieId);

            var vm = new DetailViewModel
            {
                Entry         = entry,
                Review        = review,
                Status        = entry.Status,
                PriorityLevel = entry.PriorityLevel,
                Rating        = review?.Rating ?? 0,
                Comment       = review?.Comment ?? ""
            };

            return View(vm);
        }

        // POST /Movies/Detail/5  (Save or MarkWatched)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(int id, DetailViewModel vm, string action)
        {
            var userId = _userManager.GetUserId(User);

            var entry = await _db.WatchlistEntries
                .Include(e => e.Movie)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entry == null) return NotFound();

            // Update watchlist entry
            entry.Status        = action == "markWatched" ? WatchStatus.Watched : vm.Status;
            entry.PriorityLevel = vm.PriorityLevel;
            await _db.SaveChangesAsync();

            // Upsert review
            if (vm.Rating > 0)
            {
                var review = await _db.Reviews
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == entry.MovieId);

                if (review == null)
                {
                    _db.Reviews.Add(new Review
                    {
                        UserId  = userId,
                        MovieId = entry.MovieId,
                        Rating  = vm.Rating,
                        Comment = vm.Comment ?? ""
                    });
                }
                else
                {
                    review.Rating  = vm.Rating;
                    review.Comment = vm.Comment ?? "";
                }
                await _db.SaveChangesAsync();
            }

            TempData["Success"] = action == "markWatched"
                ? $"\"{entry.Movie.Title}\" marked as watched!"
                : "Changes saved successfully!";

            return RedirectToAction("Detail", new { id });
        }

        // ── DELETE ────────────────────────────────────────────────────────────
        // GET /Movies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var entry  = await _db.WatchlistEntries
                .Include(e => e.Movie)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entry == null) return NotFound();
            return View(entry);
        }

        // POST /Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var entry  = await _db.WatchlistEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entry != null)
            {
                // Also remove linked review
                var review = await _db.Reviews
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == entry.MovieId);
                if (review != null) _db.Reviews.Remove(review);

                _db.WatchlistEntries.Remove(entry);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Movie removed from your watchlist.";
            }

            return RedirectToAction("Index", "Home");
        }

        // ── HISTORY ───────────────────────────────────────────────────────────
        // GET /Movies/History
        public async Task<IActionResult> History()
        {
            var userId = _userManager.GetUserId(User);

            var history = await _db.WatchlistEntries
                .Include(e => e.Movie)
                .Where(e => e.UserId == userId && e.Status == WatchStatus.Watched)
                .OrderByDescending(e => e.DateAdded)
                .ToListAsync();

            // Join reviews
            var movieIds = history.Select(e => e.MovieId).ToList();
            var reviews  = await _db.Reviews
                .Where(r => r.UserId == userId && movieIds.Contains(r.MovieId))
                .ToDictionaryAsync(r => r.MovieId);

            ViewBag.Reviews = reviews;
            return View(history);
        }
    }
}
