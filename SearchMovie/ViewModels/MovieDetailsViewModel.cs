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

        public ICommand GoBackCommand { get; }

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                    _ = LoadMovieAsync();
                }
            }
        }

        public int Year => _movieDetails?.Year ?? 0;
        public string Country => _movieDetails?.Country;
        public string Director => _movieDetails?.Director;
        public string Screenwriter => _movieDetails?.Screenwriter;
        public double Rating => _movieDetails?.Rating ?? 0;
        public List<string> Genres => _movieDetails != null ? _movieDetails.Genres : [];
        public List<string> Actors => _movieDetails != null ? _movieDetails.Actors : [];
        public string Overview => _movieDetails?.Overview;
        public string UrlLogo => _movieDetails?.UrlLogo;
        public string Title => _movieDetails?.Title;

        // Конструктор
        public MovieDetailsViewModel(SearchService searchService)
        {
            _searchService = searchService;
            GoBackCommand = new Command(async () => await GoBackAsync());
        }

        public static async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        private async Task LoadMovieAsync()
        {
            if (Id > 0)
            {
                _movieDetails = await _searchService.GetMovieByIdAsync(Id);
                OnPropertyChanged(nameof(Year));
                OnPropertyChanged(nameof(Country));
                OnPropertyChanged(nameof(Director));
                OnPropertyChanged(nameof(Screenwriter));
                OnPropertyChanged(nameof(Rating));
                OnPropertyChanged(nameof(Genres));
                OnPropertyChanged(nameof(Actors));
                OnPropertyChanged(nameof(Overview));
                OnPropertyChanged(nameof(UrlLogo));
                OnPropertyChanged(nameof(Title));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Id", out object? value))
            {
                Id = Convert.ToInt32(value);
            }
        }
    }
}