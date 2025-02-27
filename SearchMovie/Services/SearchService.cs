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
            searchTerm = $"%{searchTerm}%";

            var query = await _context.Movies
                .AsNoTracking()
                .Where(m =>
                    EF.Functions.Like(m.Title, searchTerm) ||
                    m.MovieGenres.Any(mg => EF.Functions.Like(mg.Genre.Name, searchTerm)) ||
                    m.MovieActors.Any(ma => EF.Functions.Like(ma.Actor.Name, searchTerm))
                )
                .Select(m => new
                {
                    Movie = m,
                    Genres = m.MovieGenres.Select(mg => mg.Genre.Name),
                    Actors = m.MovieActors.Select(ma => ma.Actor.Name)
                })
                .OrderByDescending(m => EF.Functions.Like(m.Movie.Title, searchTerm))
                .ThenByDescending(m => m.Genres.Any(g => EF.Functions.Like(g, searchTerm)))
                .ThenByDescending(m => m.Actors.Any(a => EF.Functions.Like(a, searchTerm)))
                .Take(pageSize)
                .ToListAsync();

            return query.Select(m => new MoviePreviewDTO
            {
                Id = m.Movie.Id,
                Title = m.Movie.Title,
                Year = m.Movie.Year,
                Rating = m.Movie.Rating,
                UrlLogo = m.Movie.UrlLogo,
                Overview = m.Movie.Overview,
                Genres = m.Genres.Distinct().ToList(),
                Actors = m.Actors.Distinct().ToList()
            }).ToList();
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
