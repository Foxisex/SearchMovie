using SearchMovie.ViewModels;

namespace SearchMovie.Views;

public partial class MovieDetails : ContentPage
{
	public MovieDetails(MovieDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

}