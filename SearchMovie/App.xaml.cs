using Microsoft.Extensions.DependencyInjection;
using SearchMovie.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SearchMovie
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            base.OnStart();
            SQLitePCL.Batteries_V2.Init();
            var searchService = _serviceProvider.GetRequiredService<SearchService>();
            await searchService.InitializeFTSAsync();
            await searchService.SyncFTSDataAsync();
            await DataSeederInitializeAsync();
        }

        private async Task DataSeederInitializeAsync()
        {
            var dataSeeder = _serviceProvider.GetRequiredService<DataSeeder>();

            using Stream stream = await FileSystem.OpenAppPackageFileAsync("kinopoisk-top250.csv");
            
            await dataSeeder.SeedDatabaseFromCsvAsync(stream);
        }
    }
}