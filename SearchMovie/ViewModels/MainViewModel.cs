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

            var results = await _movieService.SearchMoviesAsync(Query);
            // Очищаем предыдущие результаты
            MoviePreview.Clear();

            foreach (var movie in results)
            {
                MoviePreview.Add(movie);
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
