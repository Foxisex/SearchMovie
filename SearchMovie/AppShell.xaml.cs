using SearchMovie.Views;

namespace SearchMovie
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MovieDetails), typeof(MovieDetails));
        }
    }
}
