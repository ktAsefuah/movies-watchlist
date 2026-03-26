# CineTrack — ASP.NET Core MVC

**ITC6355 — Web Application Design and Development**
Team: Kwesi Asefuah & Priya Srivastava

## Setup & Run

Requirements: .NET 10 SDK (https://dot.net)

```bash
cd MovieWatchlist
dotnet run
```

Open https://localhost:5001 — the SQLite database and 8 seed movies are created automatically.

## Project Structure

```
MovieWatchlist/
├── MovieWatchlist.csproj
├── Program.cs                   ← Startup, services, pipeline
├── appsettings.json             ← Connection string
├── Models/
│   ├── Movie.cs                 ← Movie entity
│   ├── WatchlistEntry.cs        ← User ↔ Movie (status, priority)
│   ├── Review.cs                ← Rating + comment
│   └── ViewModels.cs            ← Login, Register, Dashboard, Detail, Search VMs
├── Data/
│   └── AppDbContext.cs          ← EF Core DbContext + seed data
├── Controllers/
│   ├── HomeController.cs        ← Dashboard
│   ├── MoviesController.cs      ← Search, Add, Detail, Delete, History
│   └── AccountController.cs    ← Register, Login, Logout, Profile
├── Views/
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   ├── Shared/_Layout.cshtml    ← Sidebar + topbar layout
│   ├── Home/Index.cshtml        ← Dashboard (Screen 1)
│   ├── Movies/
│   │   ├── Search.cshtml        ← Search + Add modal (Screen 2)
│   │   ├── Detail.cshtml        ← Edit / rate / delete (Screen 3)
│   │   ├── Delete.cshtml        ← Delete confirmation
│   │   └── History.cshtml       ← Watched history
│   └── Account/
│       ├── Login.cshtml
│       ├── Register.cshtml
│       └── Profile.cshtml
└── wwwroot/
    ├── css/site.css             ← External stylesheet
    └── js/site.js               ← External JavaScript
```

## Rubric Coverage

| Criteria | How it is met |
|---|---|
| HTML Layout | Semantic HTML5 Razor views |
| Navigation | Sidebar with active state + mobile hamburger |
| Graphics | Poster placeholders, star rating UI |
| Theme Consistency | CSS custom properties across all pages |
| External CSS | wwwroot/css/site.css linked in _Layout.cshtml |
| Internal CSS | ViewData["InlineStyle"] block set per view |
| Inline CSS | style="" attributes on specific elements |
| JS Validation | validateLoginForm(), validateRegisterForm(), validateDetailForm() |
| Alert/Prompt/Confirm | alert() for errors; confirm() for delete/watched; prompt() for notes |
| Database | SQLite via EF Core Identity + custom tables |
| MVC | Models / Views / Controllers folders |
| Reusability | _Layout.cshtml, shared CSS/JS, ViewModels |
| Responsiveness | CSS Grid + media queries at 1024/768/480px |
| Authentication | ASP.NET Core Identity, bcrypt, [Authorize] |
| Organisation | Separate folders, correct linking |
| Documentation | This README |
