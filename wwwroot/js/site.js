/* ============================================================
   MovieWatchlist — wwwroot/js/site.js  (External JavaScript)
   Covers: sidebar toggle, star rating, alert/prompt/confirm,
           form validation, search modal, theme
   ============================================================ */

/* ── Sidebar toggle (mobile) ─────────────────────────────────── */
document.addEventListener('DOMContentLoaded', () => {
  const hamburger = document.getElementById('hamburger');
  const sidebar   = document.getElementById('sidebar');

  if (hamburger && sidebar) {
    hamburger.addEventListener('click', () => sidebar.classList.toggle('open'));
  }

  /* Active nav item highlight */
  const path = window.location.pathname.toLowerCase();
  document.querySelectorAll('.nav-item[data-href]').forEach(item => {
    if (path.includes(item.dataset.href.toLowerCase())) item.classList.add('active');
  });

  /* Topbar search → redirect to Search page */
  const topSearch = document.getElementById('topbarSearch');
  if (topSearch) {
    topSearch.addEventListener('keydown', e => {
      if (e.key === 'Enter' && topSearch.value.trim()) {
        window.location = '/Movies/Search?query=' + encodeURIComponent(topSearch.value.trim());
      }
    });
  }
});

/* ── Star rating input ───────────────────────────────────────── */
function initStarInput(containerId, hiddenId) {
  const container = document.getElementById(containerId);
  const hidden    = document.getElementById(hiddenId);
  if (!container || !hidden) return;

  const stars = container.querySelectorAll('span');

  function paint(n) {
    stars.forEach((s, i) => s.classList.toggle('active', i < n));
  }

  stars.forEach((star, i) => {
    star.addEventListener('click',     () => { hidden.value = i + 1; paint(i + 1); });
    star.addEventListener('mouseover', () => paint(i + 1));
  });
  container.addEventListener('mouseleave', () => paint(parseInt(hidden.value) || 0));

  // Set initial state from hidden input
  paint(parseInt(hidden.value) || 0);
}

/* ── JS form validation (client-side layer) ──────────────────── */

// Login validation
function validateLoginForm() {
  const email = document.getElementById('loginEmail')?.value.trim();
  const pass  = document.getElementById('loginPass')?.value.trim();
  if (!email) { alert('Email address is required.'); return false; }
  if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) { alert('Please enter a valid email address.'); return false; }
  if (!pass)  { alert('Password is required.'); return false; }
  if (pass.length < 6) { alert('Password must be at least 6 characters.'); return false; }
  return true;
}

// Register validation
function validateRegisterForm() {
  const name    = document.getElementById('regName')?.value.trim();
  const email   = document.getElementById('regEmail')?.value.trim();
  const pass    = document.getElementById('regPass')?.value;
  const confirm = document.getElementById('regConfirm')?.value;

  if (!name)  { alert('Full name is required.'); return false; }
  if (!email) { alert('Email address is required.'); return false; }
  if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) { alert('Please enter a valid email address.'); return false; }
  if (!pass || pass.length < 8) { alert('Password must be at least 8 characters.'); return false; }
  if (!/[A-Z]/.test(pass)) { alert('Password must contain at least one uppercase letter.'); return false; }
  if (!/[0-9]/.test(pass)) { alert('Password must contain at least one number.'); return false; }
  if (pass !== confirm) { alert('Passwords do not match.'); return false; }

  alert(`Welcome, ${name}! Your account is being created…`);
  return true;
}

// Detail page validation
function validateDetailForm() {
  const rating = parseInt(document.getElementById('ratingValue')?.value || '0');
  const status = document.getElementById('statusSelect')?.value;
  if (!status) { alert('Please select a status for this movie.'); return false; }
  if (rating === 0) {
    return confirm('You have not added a star rating. Save without a rating?');
  }
  return true;
}

/* ── Confirm / Prompt dialogs ────────────────────────────────── */
function confirmDelete(title) {
  return confirm(`Remove "${title}" from your watchlist?\n\nThis action cannot be undone.`);
}

function confirmMarkWatched(title) {
  return confirm(`Mark "${title}" as watched?\n\nThis will update its status to Watched.`);
}

function promptNotes() {
  const area = document.getElementById('notesInput');
  if (!area) return;
  const result = prompt('Edit your notes for this movie:', area.value);
  if (result !== null) area.value = result;
}

/* ── Search modal ─────────────────────────────────────────────── */
function openAddModal(id, title, year, genre, desc) {
  document.getElementById('mTitle').textContent = `${title} (${year})`;
  document.getElementById('mGenre').textContent = genre;
  document.getElementById('mDesc').textContent  = desc;
  document.getElementById('addMovieId').value   = id;
  document.getElementById('addModal').classList.remove('hidden');
}

function closeAddModal() {
  document.getElementById('addModal')?.classList.add('hidden');
}

/* ── Password strength indicator ─────────────────────────────── */
function updateStrength(val) {
  const fill = document.getElementById('strengthFill');
  if (!fill) return;
  let score = 0;
  if (val.length >= 8)           score++;
  if (/[A-Z]/.test(val))         score++;
  if (/[0-9]/.test(val))         score++;
  if (/[^A-Za-z0-9]/.test(val))  score++;
  const widths = ['0%','25%','50%','75%','100%'];
  const colors = ['#ccc','#E53935','#FB8C00','#F9A825','#2E7D32'];
  fill.style.width      = widths[score];
  fill.style.background = colors[score];
}
