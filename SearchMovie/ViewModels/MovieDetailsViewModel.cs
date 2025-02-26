using System.ComponentModel;
using System.Runtime.CompilerServices;
using SearchMovie.Services;
using SQLiteClassLibrary.Models.DTO;
using System.Windows.Input;

namespace SearchMovie.ViewModels
{
    public partial class MovieDetailsViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private readonly SearchService _searchService;
        private int _id;
        private MovieDetailsDTO _movieDetails;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MovieDetailsViewModel(SearchService searchService)
        {
            _searchService = searchService;
            GoBackCommand = new Command(async () => await MovieDetailsViewModel.GoBackAsync());
        }

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                    _ = LoadMovieAsync(); // Загружаем фильм при изменении Id
                }
            }
        }

        public MovieDetailsDTO MovieDetails
        {
            get => _movieDetails;
            set
            {
                if (_movieDetails != value)
                {
                    _movieDetails = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand GoBackCommand { get; }

        public static async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        private async Task LoadMovieAsync()
        {
            if (Id > 0)
            {
                MovieDetails = await _searchService.GetMovieByIdAsync(Id);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Получаем параметр "Id" из Shell
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Id", out object? value))
            {
                Id = Convert.ToInt32(value);
            }
        }
    }
}
