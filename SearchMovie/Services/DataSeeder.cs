using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SQLiteClassLibrary.Models;
using SQLiteClassLibrary;
using SQLiteClassLibrary.Models.DTO;
using Microsoft.EntityFrameworkCore;
using CsvHelper;
using System.Globalization;
using System.Diagnostics;


namespace SearchMovie.Services
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedDatabaseFromCsvAsync(Stream csvStream) // Заполнение БД значениями из csv при первом запуске
        {
            if (await _context.Movies.AnyAsync())
            {
                return; // База данных уже заполнена
            }

            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<MovieCsvDTO>();

            foreach (var record in records)
            {
                var movie = new Movie
                {
                    Title = record.Title.Replace(';', ','),
                    Year = record.Year,
                    Country = record.Country,
                    Rating = record.Rating,
                    Overview = record.Overview.Replace(';', ','),
                    Director = record.Director.Replace(';', ','),
                    Screenwriter = record.Screenwriter.Replace(';', ','),
                    UrlLogo = record.UrlLogo
                };

                // Добавление актеров
                var actorNames = record.Actors.Split(';');
                foreach (var actorName in actorNames)
                {
                    var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == actorName.Trim());
                    if (actor == null)
                    {
                        actor = new Actor { Name = actorName.Trim() };
                        _context.Actors.Add(actor);
                    }
                    movie.MovieActors.Add(new MovieActor { Actor = actor });
                }

                // Добавление жанров
                var genreNames = record.Genres.Split(';');
                foreach (var genreName in genreNames)
                {
                    var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName.Trim());
                    if (genre == null)
                    {
                        genre = new Genre { Name = genreName.Trim() };
                        _context.Genres.Add(genre);
                    }
                    movie.MovieGenres.Add(new MovieGenre { Genre = genre });
                }

                _context.Movies.Add(movie);
            }

            await _context.SaveChangesAsync();
        }

    }
}
