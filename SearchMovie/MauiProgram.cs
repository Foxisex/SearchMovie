using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SearchMovie.Views;
using SearchMovie.Services;
using SearchMovie.ViewModels;
using SQLiteClassLibrary;

namespace SearchMovie
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            // Добавляем контекст дб из миграции
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Filename={Path.Combine(FileSystem.AppDataDirectory, "movies.db")}", x => x.MigrationsAssembly(nameof(SQLiteClassLibrary)))
            );
            builder.Services.AddScoped<DataSeeder>();
            builder.Services.AddScoped<SearchService>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddTransient<MovieDetails>();
            builder.Services.AddTransient<MovieDetailsViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
