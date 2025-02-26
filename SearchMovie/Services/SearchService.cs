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

        // Метод для создания виртуальной таблицы FTS5
        public async Task InitializeFTSAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS MoviesFTS;");
            await _context.Database.ExecuteSqlRawAsync(@"
            CREATE VIRTUAL TABLE IF NOT EXISTS MoviesFTS USING fts5(Title, Genre, Actor);
        ");
        }

        public async Task SyncFTSDataAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(@"
        INSERT INTO MoviesFTS (Title, Genre, Actor)
        SELECT m.Title, g.Name, a.Name
        FROM Movies m
        JOIN MovieGenres mg ON m.Id = mg.MovieId
        JOIN Genres g ON mg.GenreId = g.Id
        JOIN MovieActors ma ON m.Id = ma.MovieId
        JOIN Actors a ON ma.ActorId = a.Id;
    ");
        }

        public async Task<List<Movie>> SearchMoviesAsync(string searchTerm)
        {
            // Получаем уникальные названия фильмов из FTS5
            var movieTitles = await _context.MoviesFTS
                .FromSqlRaw($"SELECT DISTINCT Title FROM MoviesFTS WHERE MoviesFTS MATCH {{0}}", searchTerm).Take(30)
                .Select(m => m.Title)  // Извлекаем только названия фильмов
                .ToListAsync();

            if (movieTitles.Count == 0) return []; // Если ничего не найдено, сразу возвращаем пустой список

            // Получаем полные данные фильмов
            var movies = await _context.Movies
                .Where(m => movieTitles.Contains(m.Title)) // Фильтруем по найденным названиям
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .ToListAsync();

            return movies;
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
