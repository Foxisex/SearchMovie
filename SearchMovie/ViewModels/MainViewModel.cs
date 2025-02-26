using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using SearchMovie.Services;
using SQLiteClassLibrary.Models.DTO;
using SearchMovie.Views;
using Microsoft.Maui.Controls;

namespace SearchMovie.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        private readonly SearchService _movieService;
        private string? _query;

        public MainViewModel(SearchService movieService)
        {
            _movieService = movieService;
            MoviePreview = [];

            SearchMoviesCommand = new Command(async () => await SearchMoviesAsync());
            TapCommand = new Command<int>(async (id) => await TapAsync(id));
        }

        public ObservableCollection<MoviePreviewDTO> MoviePreview { get; }

        public string? Query
        {
            get => _query;
            set
            {
                if (_query != value)
                {
                    _query = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SearchMoviesCommand { get; }
        public ICommand TapCommand { get; }

        private async Task SearchMoviesAsync()
        {
            Query = Regex.Replace(Query ?? "", "[^\\p{L}0-9 ]", "").Trim();

            if (string.IsNullOrWhiteSpace(Query))
            {
                return;
            }

            // Очищаем предыдущие результаты
            MoviePreview.Clear();

            // Выполняем поиск
            var movies = await _movieService.SearchMoviesAsync(Query);

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

        private async Task TapAsync(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                { "Id", id }
            };

            await Shell.Current.GoToAsync(nameof(MovieDetails), parameters);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
