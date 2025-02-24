using SearchMovie.Services;
using SearchMovie.ViewModels;
using SQLiteClassLibrary.Models.DTO;

namespace SearchMovie.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
