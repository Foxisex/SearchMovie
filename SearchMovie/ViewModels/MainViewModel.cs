using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SearchMovie.Services;
using SQLiteClassLibrary.Models;
using SQLiteClassLibrary.Models.DTO;
using SearchMovie.Views;

namespace SearchMovie.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly SearchService _movieService;

        public MainViewModel(SearchService movieService)
        {
            _movieService = movieService;
            MoviePreview = new ObservableCollection<MoviePreviewDTO>();
        }

        public ObservableCollection<MoviePreviewDTO> MoviePreview { get; set; }

        [ObservableProperty]
        private string? _query;

        [RelayCommand] 
        public async Task SearchMoviesAsync()
        {
            _query = Regex.Replace(_query ?? "", "[^\\p{L}0-9 ]", "").Trim();

            if (_query.Length == 0)
            {
                return;
            }
            // Очищаем предыдущие результаты
            MoviePreview.Clear();

            // Выполняем поиск
            var movies = await _movieService.SearchMoviesAsync(_query);
            // Добавляем найденные фильмы в коллекцию
            foreach (var movie in movies)
            {
                MoviePreview.Add(new MoviePreviewDTO
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Year = movie.Year,
                    Rating = movie.Rating,
                    Overview = movie.Overview,
                    UrlLogo = movie.UrlLogo,
                    Genres = movie.MovieGenres.Select(mg => mg.Genre.Name).ToList()
                });
            }
        }
        [RelayCommand]
        public async Task TapAsync(int id)
        {
            await Shell.Current.GoToAsync($"{nameof(MovieDetails)}?Id={id}");
        }
    }
}
