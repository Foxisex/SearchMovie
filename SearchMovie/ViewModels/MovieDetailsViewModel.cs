using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SearchMovie.Services;
using SQLiteClassLibrary.Models.DTO;

namespace SearchMovie.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public partial class MovieDetailsViewModel : ObservableObject
    {
        private readonly SearchService _searchService;

        public MovieDetailsViewModel(SearchService searchService)
        {
            _searchService = searchService;
        }

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private MovieDetailsDTO _movieDetails;

        [RelayCommand]
        public async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        // Метод вызывается автоматически при изменении Id
        partial void OnIdChanged(int value)
        {
            _ = LoadMovieAsync();
        }

        private async Task LoadMovieAsync()
        {
            MovieDetails = await _searchService.GetMovieByIdAsync(Id);
        }
    }
}
