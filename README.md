# Приложение для поиска фильмов

## Описание
Это мобильное приложение, разработанное на .NET MAUI, предназначено для поиска и отображения информации о кинофильмах. Поддерживаются платформы **Android** и **iOS**. Приложение позволяет искать фильмы по **названию**, **жанру** или **имени актера**, а также просматривать подробную информацию о выбранном фильме.

## Функционал
- **Отображение результатов** — до **20** наиболее релевантных фильмов выводятся под строкой поиска.
- **Экран деталей фильма** — содержит информацию о фильме, включая год выпуска, страну, рейтинг, описание, режиссера, сценариста, список актеров и жанры.
- **Навигация** — реализована с использованием **Shell Navigation**.
- **Паттерн MVVM** — логика отделена от UI.

## Технологии и инструменты
- **.NET MAUI** - кроссплатформенная разработка.
- **SQLite** - локальная база данных.
- **EF Core** - ORM для взаимодействия с базой данных.
- **CsvHelper** - считывание данных из csv в базу данных.

## Развертывание проекта
### Требования
- **.NET 8+**
- **Android SDK / iOS SDK**
- **Visual Studio 2022** с установленным MAUI Workload

### Установка
1. Клонируйте репозиторий:
   ```sh
   git clone https://github.com/Foxisex/SearchMovie.git
   ```
2. Перейдите в каталог проекта:
   ```sh
   cd SearchMovie
   ```
3. Установите зависимости:
   ```sh
   dotnet restore
   ```
4. Создайте и запустите миграцию базы данных:
   ```sh
   dotnet ef migrations add MigrationName --project .\SQLiteClassLibrary\ --startup-project .\SQLiteClassLibrary\
   dotnet ef database update --project .\SQLiteClassLibrary\ --startup-project .\SQLiteClassLibrary\
   ```
5. Запустите приложение:
   ```sh
   dotnet build
   dotnet maui run android  # или dotnet maui run ios
   ```
   Либо после применения миграций сбилдите проект средствами VS

## SQL-запрос для поиска фильмов
```sql
SELECT 
    m.Id, m.Title, m.Year, m.Rating, m.UrlLogo, m.Overview,
    GROUP_CONCAT(DISTINCT g.Name) AS Genres,
    GROUP_CONCAT(DISTINCT a.Name) AS Actors
FROM Movies m
LEFT JOIN MovieGenres mg ON mg.MovieId = m.Id
LEFT JOIN Genres g ON mg.GenreId = g.Id
LEFT JOIN MovieActors ma ON ma.MovieId = m.Id
LEFT JOIN Actors a ON ma.ActorId = a.Id
WHERE 
    m.Title LIKE @searchTerm 
    OR g.Name LIKE @searchTerm
    OR a.Name LIKE @searchTerm
GROUP BY m.Id
ORDER BY 
    CASE WHEN m.Title LIKE @searchTerm THEN 1 ELSE 0 END DESC,
    CASE WHEN g.Name LIKE @searchTerm THEN 1 ELSE 0 END DESC,
    CASE WHEN a.Name LIKE @searchTerm THEN 1 ELSE 0 END DESC
LIMIT @pageSize;
```
## Скриншоты
<p align="center">
<img src="Images/SearchExample_Title.jpg" width="300">
<img src="Images/SearchExample_Actor.jpg" width="300">
</p>
<p align="center">
<img src="Images/SearchExample_Genre.jpg" width="300">
<img src="Images/MovieDetails.jpg" width="300">
</p>

