using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteClassLibrary.Models;
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
        public async Task<List<MoviePreviewDTO>> SearchMoviesAsync(string searchTerm)
        {
            var query = await _context.Movies
                .Where(m =>
                    m.Title.Contains(searchTerm) ||
                    m.MovieGenres.Any(mg => mg.Genre.Name.Contains(searchTerm)) ||
                    m.MovieActors.Any(ma => ma.Actor.Name.Contains(searchTerm))
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
                .ToListAsync(); // Выполняем SQL-запрос, загружаем данные в память

            // Теперь сортируем в памяти, потому что SQLite не умеет сортировать по `Contains()`
            var sortedMovies = query
                .OrderByDescending(m => m.Title.Contains(searchTerm))  // 1. Совпадение по названию
                .ThenByDescending(mg => mg.Genres.Contains(searchTerm))  // 2. Совпадение по жанру
                .ThenByDescending(ma => ma.Actors.Contains(searchTerm))  // 3. Совпадение по актёрам
                .Take(20) // Ограничиваем 20 фильмами
                .ToList();

            return sortedMovies;
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
