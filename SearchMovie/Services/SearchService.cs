using SQLiteClassLibrary;
using Microsoft.EntityFrameworkCore;
using SQLiteClassLibrary.Models.DTO;

namespace SearchMovie.Services
{
    public class SearchService
    {
        private readonly AppDbContext _context;

        public SearchService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<MoviePreviewDTO>> SearchMoviesAsync(string searchTerm, int pageSize = 20)
        {
            var query = await _context.Movies
                .AsNoTracking()
                .Where(m =>
                    EF.Functions.Like(m.Title, $"%{searchTerm}%") ||
                    m.MovieGenres.Any(mg => EF.Functions.Like(mg.Genre.Name, $"%{searchTerm}%")) ||
                    m.MovieActors.Any(ma => EF.Functions.Like(ma.Actor.Name, $"%{searchTerm}%"))
                )
                .Select(m => new MoviePreviewDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year,
                    Rating = m.Rating,
                    UrlLogo = m.UrlLogo,
                    Overview = m.Overview,
                    Genres = m.MovieGenres.Select(mg => mg.Genre.Name).ToList(),
                    Actors = m.MovieActors.Select(ma => ma.Actor.Name).ToList()
                })
                .OrderByDescending(m => m.Title.Contains(searchTerm))
                .ThenByDescending(mg => mg.Genres.Contains(searchTerm))
                .ThenByDescending(ma => ma.Actors.Contains(searchTerm))
                .Take(pageSize)
                .ToListAsync();
            return query;
        }

        public async Task<MovieDetailsDTO> GetMovieByIdAsync(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return null;

            return new MovieDetailsDTO
            {
                Title = movie.Title,
                Year = movie.Year,
                Country = movie.Country,
                Rating = movie.Rating,
                Overview = movie.Overview,
                Director = movie.Director,
                Screenwriter = movie.Screenwriter,
                UrlLogo = movie.UrlLogo,
                Genres = movie.MovieGenres.Select(mg => mg.Genre.Name).ToList(),
                Actors = movie.MovieActors.Select(ma => ma.Actor.Name).ToList()
            };
        }
    }
}
