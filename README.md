# Приложение для поиска фильмов

## Описание
Это мобильное приложение, разработанное на .NET MAUI, предназначено для поиска и отображения информации о кинофильмах. Поддерживаются платформы **Android** и **iOS**. Приложение позволяет искать фильмы по **названию**, **жанру** или **имени актера**, а также просматривать подробную информацию о выбранном фильме.

## Функционал
- **Поиск фильмов** — реализован полнотекстовый поиск по "Названию" / "Жанру" / "Актёру" с помощью **SQLite FTS5**.
- **Отображение результатов** — до **30** наиболее релевантных фильмов выводятся под строкой поиска.
- **Экран деталей фильма** — содержит информацию о фильме, включая год выпуска, страну, рейтинг, описание, режиссера, сценариста, список актеров и жанры.
- **Навигация** — реализована с использованием **Shell Navigation**.
- **Паттерн MVVM** — логика отделена от UI, используется **CommunityToolkit.MVVM**.

## Технологии и инструменты
- **.NET MAUI** - кроссплатформенная разработка.
- **SQLite** - локальная база данных.
- **EF Core** - ORM для взаимодействия с базой данных.
- **CommunityToolkit.MVVM** - упрощает реализацию MVVM.
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
SELECT DISTINCT Title
FROM MoviesFTS
WHERE MoviesFTS MATCH @searchTerm
LIMIT 30;
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

