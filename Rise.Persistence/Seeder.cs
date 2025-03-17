using Rise.Domain.Accounts;
using Rise.Domain.Events;
using Rise.Domain.Giftcards;
using Rise.Domain.Movies;
using Rise.Domain.MovieWatchlists;
using Rise.Domain.Tenturncards;
using Rise.Domain.Tickets;

namespace Rise.Persistence;

public class Seeder
{
    private readonly ApplicationDbContext dbContext;

    public Seeder(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Seed()
    {
        if (HasAlreadyBeenSeeded())
            return;
        SeedDb();
    }

    private bool HasAlreadyBeenSeeded()
    {
        return dbContext.Accounts.Any()
            || dbContext.Movies.Any()
            || dbContext.Events.Any()
            || dbContext.Watchlists.Any()
            || dbContext.MovieWatchlist.Any()
            || dbContext.Cinemas.Any()
            || dbContext.Showtime.Any()
            || dbContext.Tickets.Any()
            || dbContext.Giftcards.Any()
            || dbContext.Tenturncards.Any();
    }

    public static List<DateTime> GenerateWeeklyShowtimes(
    DateTime baseDate,
    TimeSpan startTime,
    TimeSpan interval,
    List<DayOfWeek>? daysOfWeek = null,
    int occurrencesPerDay = 3)
    {
        var showtimes = new List<DateTime>();
        var startDate = baseDate.Date; // we allow baseDate to be today
        var endDate = startDate.AddDays(6); // generate a 7-day window
        daysOfWeek ??= new List<DayOfWeek>
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };

        // When baseDate is today, we want to skip any showtimes that are already past.
        bool isToday = startDate == DateTime.Today;

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (daysOfWeek.Contains(date.DayOfWeek))
            {
                for (int i = 0; i < occurrencesPerDay; i++)
                {
                    var showtime = date + startTime + TimeSpan.FromHours(i * interval.TotalHours);
                    if (isToday && showtime < DateTime.Now)
                    {
                        // Skip showtimes earlier than now on the current day.
                        continue;
                    }
                    showtimes.Add(showtime);
                }
            }
        }

        return showtimes;
    }

    private void SeedDb()
    {
        var account = new Account
        {
            Email = "testuser2@test.be",
            Watchlist = null!,
        };
        var watchlist = new Watchlist
        {
            UserId = account.Id,
            Account = account,
        };
        var movie = new Movie
        {
            EventId = 2,
            Title = "The Matrix",
            Genre = "Action",
            Description = "Een computerhacker leert van mysterieuze rebellen over de ware aard van zijn werkelijkheid en zijn rol in de oorlog tegen de beheerders ervan.",
            Duration = 135,
            Director = "Lana Wachowski",
            Cast = ["Lana Wachowski", "Lilly Wachowski"],
            ReleaseDate = new DateTime(1999, 3, 31),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/MatrixVideoImage.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/MatrixCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/MatrixCover.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/MatrixCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/MatrixVideo.mp4",
        };

        var movie2 = new Movie
        {
            Title = "Inception",
            Genre = "Science Fiction",
            Description = "Wanneer meester-dromer Dom Cobb (Leonardo DiCaprio) de kans krijgt om zijn verleden achter te laten, krijgt hij de uitdaging van zijn leven ",
            Duration = 148,
            Director = "Christopher Nolan",
            Cast = ["Leonardo DiCaprio", "Joseph Gordon-Levitt", "Elliot Page"],
            ReleaseDate = new DateTime(1999, 3, 31),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/InceptionVideoImage.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/InceptionCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/InceptionCover.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/InceptionCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/InceptionVideo.mp4",
        };

        var movie3 = new Movie
        {
            Title = "The Dark Knight",
            Genre = "Action",
            Description = "In Gotham City neemt de criminaliteit weer toe, en de strijd tussen Batman (Christian Bale) en de georganiseerde misdaad bereikt nieuwe hoogtes.",
            Duration = 152,
            Director = "Christopher Nolan",
            Cast = ["Christian Bale", "Heath Ledger", "Aaron Eckhart"],
            ReleaseDate = new DateTime(2008, 7, 23),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/TheDarkNightVideoImage.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/TheDarkNightCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/TheDarkNightCover.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/TheDarkNightCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/TheDarkNightVideo.mp4",
        };

        var movie4 = new Movie
        {
            Title = "Interstellar",
            Genre = "Adventure",
            Description = "In een toekomst waarin de aarde in verval is, komt voormalig NASA-piloot Cooper (Matthew McConaughey) in actie om de mensheid te redden.",
            Duration = 169,
            Director = "Christopher Nolan",
            Cast = ["Matthew McConaughey", "Anne Hathaway", "Jessica Chastain"],
            ReleaseDate = new DateTime(2014, 11, 5),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/InterstellarVideoImage.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/InterstellarCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/InterstellarCover.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/InterstellarCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/IntersellarVideo.mp4",
        };

        var movie5 = new Movie
        {
            Title = "Parasite",
            Genre = "Thriller",
            Description = "In deze scherpe sociale satire volgt de arme familie Kim die hun leven verandert wanneer ze infiltreren in het rijke huishouden van de Park-familie.",
            Duration = 155,
            Director = "Christopher Nolan",
            Cast = ["Kang-ho Song", "Sun-kyun Lee", "Yeo-jeong Jo"],
            ReleaseDate = new DateTime(2014, 11, 5),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/ParasiteVideoImage.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/ParasiteCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/ParasiteCover.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/ParasiteCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/ParasiteVideo.mp4",
        };

        // New movies
        var movie6 = new Movie
        {
            Title = "Avengers: Endgame",
            Genre = "Action",
            Description = "After the devastating events of Avengers: Infinity War, the universe is in ruins.",
            Duration = 181,
            Director = "Anthony Russo, Joe Russo",
            Cast = ["Robert Downey Jr.", "Chris Evans", "Mark Ruffalo", "Chris Hemsworth", "Scarlett Johansson", "Jeremy Renner"],
            ReleaseDate = new DateTime(2019, 4, 26),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/AvengersEndgameBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/AvengerEndgameCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/AvengersEndgameBanner.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/AvengerEndgameCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/AvangersEndgameVideo.mp4",
        };

        var movie7 = new Movie
        {
            Title = "Joker",
            Genre = "Crime",
            Description = "In Gotham City, mentally troubled comedian Arthur Fleck embarks on a downward spiral.",
            Duration = 122,
            Director = "Todd Phillips",
            Cast = ["Joaquin Phoenix", "Robert De Niro", "Zazie Beetz"],
            ReleaseDate = new DateTime(2019, 10, 4),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/JokerBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/JokerCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/JokerBanner.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/JokerCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/JokerVideo.mp4",
        };

        var movie8 = new Movie
        {
            Title = "The Shawshank Redemption",
            Genre = "Drama",
            Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption.",
            Duration = 142,
            Director = "Frank Darabont",
            Cast = ["Tim Robbins", "Morgan Freeman", "Bob Gunton"],
            ReleaseDate = new DateTime(1994, 9, 23),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/ShawshankBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/ShawshankCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/ShawshankBanner.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/ShawshankCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/ShawshankVideo.mp4",
        };

        var movie9 = new Movie
        {
            Title = "Pulp Fiction",
            Genre = "Crime",
            Description = "The lives of two mob hitmen, a boxer, and a gangster's wife intertwine in tales of violence and redemption.",
            Duration = 154,
            Director = "Quentin Tarantino",
            Cast = ["John Travolta", "Uma Thurman", "Samuel L. Jackson"],
            ReleaseDate = new DateTime(1994, 10, 14),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/PulpBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/PulpCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/PulpBanner.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/PulpCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/PulpVideo.mp4",
        };

        var movie10 = new Movie
        {
            Title = "The Godfather",
            Genre = "Crime",
            Description = "The aging patriarch of an organized crime dynasty transfers control to his reluctant son.",
            Duration = 175,
            Director = "Francis Ford Coppola",
            Cast = ["Marlon Brando", "Al Pacino", "James Caan"],
            ReleaseDate = new DateTime(1972, 3, 24),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/TheGodfatherBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/TheGodfatherCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/TheGodfatherBanner.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/TheGodfatherCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/TheGodfatherVideo.mp4",
        };

        var movie11 = new Movie
        {
            EventId = 3,
            Title = "Forrest Gump",
            Genre = "Drama",
            Description = "An Alabama man with an IQ of 75 witnesses historical events in this heartfelt story.",
            Duration = 142,
            Director = "Robert Zemeckis",
            Cast = ["Tom Hanks", "Robin Wright", "Gary Sinise"],
            ReleaseDate = new DateTime(1994, 7, 6),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/ForrestgumpBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/ForrestgumpCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/ForrestgumpBanner.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/ForrestgumpCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/ForrestgumpVideo.mp4",
        };

        var movie12 = new Movie
        {
            Title = "The Lion King",
            Genre = "Animation",
            Description = "A lion prince flees his kingdom only to learn the true meaning of responsibility.",
            Duration = 88,
            Director = "Roger Allers, Rob Minkoff",
            Cast = ["Matthew Broderick", "Jeremy Irons", "James Earl Jones"],
            ReleaseDate = new DateTime(1994, 6, 24),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/TheLionKingBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/TheLionKingCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/TheLionKingBanner.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/TheLionKingCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/TheLionKingVideo.mp4",
        };

        // Future movies

        var movie13 = new Movie
        {
            Title = "Avengers: The Kang Dynasty",
            Genre = "Action, Sci-Fi",
            Description = "The Avengers face off against Kang the Conqueror, a time-traveling adversary threatening the multiverse.",
            Duration = 150,
            Director = "Destin Daniel Cretton",
            Cast = new List<string> { "Robert Downey Jr.", "Chris Evans", "Mark Ruffalo", "Chris Hemsworth", "Scarlett Johansson", "Samuel L. Jackson" },
            ReleaseDate = new DateTime(2025, 5, 1),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/AvangersKangVideoBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/AvangersKangCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/AvangersKangVideoBanner.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/AvangersKangCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/AVENGERS.mp4",
        };

        var movie14 = new Movie
        {
            Title = "Star Wars: New Jedi Order",
            Genre = "Action, Adventure, Sci-Fi",
            Description = "A new generation of Jedi faces emerging threats in the galaxy, continuing the legacy after the fall of the Empire.",
            Duration = 140,
            Director = "Kenneth Branagh",
            Cast = new List<string> { "Ewan McGregor", "Daisy Ridley", "John Boyega", "Oscar Isaac" },
            ReleaseDate = new DateTime(2025, 12, 19),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/StarWarsBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/StarWarsCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/StarWarsBanner.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/StarWarsCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/StarWars.mp4",
        };

        var movie15 = new Movie
        {
            Title = "Deadpool 3",
            Genre = "Action, Comedy",
            Description = "Deadpool returns to the Marvel Cinematic Universe, bringing his unique brand of humor and chaos to a new set of adventures.",
            Duration = 110,
            Director = "Adil El Arbi, Bilall Fallah",
            Cast = new List<string> { "Ryan Reynolds", "Hugh Jackman", "Zazie Beetz" },
            ReleaseDate = new DateTime(2025, 7, 25),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/DeadPBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/DeadPCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/DeadPBanner.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/DeadPCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/Deadpool&Wolverine.mp4",
        };

        var movie16 = new Movie
        {
            Title = "Fantastic Four",
            Genre = "Action, Adventure, Sci-Fi",
            Description = "Marvel's First Family is introduced to the MCU, showcasing their origins and their fight to save the world from a new threat.",
            Duration = 130,
            Director = "Matt Shakman",
            Cast = new List<string> { "Ioan Gruffudd", "Jessica Alba", "Michael B. Jordan", "Kate Mara" },
            ReleaseDate = new DateTime(2025, 2, 14),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/FantasticBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/FantasticCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/FantasticBanner.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/FantasticCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/TheFantasticFour.mp4",
        };

        var movie17 = new Movie
        {
            Title = "Jurassic World: Extinction",
            Genre = "Action, Adventure, Sci-Fi",
            Description = "The Jurassic World franchise continues as new threats emerge from the resurrected dinosaurs, pushing humanity to the brink of extinction.",
            Duration = 140,
            Director = "Colin Trevorrow",
            Cast = new List<string> { "Chris Pratt", "Bryce Dallas Howard", "Vincent D'Onofrio" },
            ReleaseDate = new DateTime(2025, 6, 15),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/JurassicWorldBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/JurassicWorldCover.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/JurassicWorldBanner.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/JurassicWorldCover.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/JURASSICWORLD.mp4",
        };
        var movie18 = new Movie
        {
            EventId = 1,
            Title = "Home Alone 1",
            Genre = "Comedy",
            Description = "A young boy is left home alone and must defend his house from burglars during Christmas.",
            Duration = 103,
            Director = "Chris Columbus",
            Cast = new List<string> { "Macaulay Culkin", "Joe Pesci", "Daniel Stern", "John Heard" },
            ReleaseDate = new DateTime(1990, 11, 16),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/HomeAloneBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/HomeAlone1.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/HomeAloneBanner.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/HomeAlone1.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/HomeAloneVideo1.mp4"
        };

        var movie19 = new Movie
        {
            EventId = 1,
            Title = "Home Alone 2: Lost in New York",
            Genre = "Comedy",
            Description = "Kevin is left home alone once again, this time in New York City, where he battles the same burglars from the first movie.",
            Duration = 120,
            Director = "Chris Columbus",
            Cast = new List<string> { "Macaulay Culkin", "Joe Pesci", "Daniel Stern", "Tim Curry" },
            ReleaseDate = new DateTime(1992, 11, 20),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/HomeAloneBanner2.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/HomeAlone2.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/HomeAloneBanner2.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/HomeAlone2.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/HomeAloneVideo2.mp4"
        };

        var movie20 = new Movie
        {
            EventId = 1,
            Title = "Home Alone 3",
            Genre = "Comedy",
            Description = "A new young boy, Alex, is left home alone, where he battles international thieves trying to steal a top-secret computer chip.",
            Duration = 103,
            Director = "Raja Gosnell",
            Cast = new List<string> { "Alex D. Linz", "Olek Krupa", "Rya Kihlstedt", "Haviland Morris" },
            ReleaseDate = new DateTime(1997, 12, 12),
            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/HomeAloneBanner3.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/HomeAlone3.webp",
            BannerImageUrl = "https://riseopslag2425.blob.core.windows.net/images/HomeAloneBanner3.webp",
            PosterImageUrl = "https://riseopslag2425.blob.core.windows.net/images/HomeAlone3.webp",
            MovieLink = "https://riseopslag2425.blob.core.windows.net/images/HomeAloneVideo3.mp4"
        };

        var cinema = new Cinema { Name = "Brugge", Location = "Brugge", };
        var cinema2 = new Cinema { Name = "Mechelen", Location = "Mechelen" };
        var cinema3 = new Cinema { Name = "Antwerpen", Location = "Antwerpen", };
        var cinema4 = new Cinema { Name = "Cinema Cartoons", Location = "Antwerpen", };

        var showtimeDatesMovieCinema = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );

        var showtimesMovieCinema = showtimeDatesMovieCinema.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema.Id,
            MovieId = movie.Id,
            ShowTime = showTimeDate,
            Cinema = cinema,
            Movie = movie
        }).ToList();

        var showtimeDatesMovieCinema2 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(9), // First show starts at 9:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );

        var showtimesMovieCinema2 = showtimeDatesMovieCinema2.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema2.Id,
            MovieId = movie.Id,
            ShowTime = showTimeDate,
            Cinema = cinema2,
            Movie = movie
        }).ToList();

        var showtimeDatesMovie2Cinema = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(12), // First show starts at 12:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 2              // Three shows per day
        );

        var showtimesMovie2Cinema = showtimeDatesMovie2Cinema.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema.Id,
            MovieId = movie2.Id,
            ShowTime = showTimeDate,
            Cinema = cinema,
            Movie = movie2
        }).ToList();

        var showtimeDatesMovie2Cinema3 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(11), // First show starts at 11:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );

        var showtimesMovie2Cinema3 = showtimeDatesMovie2Cinema3.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema3.Id,
            MovieId = movie2.Id,
            ShowTime = showTimeDate,
            Cinema = cinema3,
            Movie = movie2
        }).ToList();

        var showtimeDatesMovie3Cinema4 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(8), // First show starts at 8:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 4              // Three shows per day
        );

        var showtimesMovie3Cinema4 = showtimeDatesMovie3Cinema4.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema4.Id,
            MovieId = movie3.Id,
            ShowTime = showTimeDate,
            Cinema = cinema4,
            Movie = movie3
        }).ToList();

        var showtimeDatesMovie3Cinema2 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );

        var showtimesMovie3Cinema2 = showtimeDatesMovie3Cinema4.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema2.Id,
            MovieId = movie3.Id,
            ShowTime = showTimeDate,
            Cinema = cinema2,
            Movie = movie3
        }).ToList();

        var showtimeDatesMovie4Cinema4 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );

        var showtimesMovie4Cinema4 = showtimeDatesMovie4Cinema4.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema4.Id,
            MovieId = movie4.Id,
            ShowTime = showTimeDate,
            Cinema = cinema4,
            Movie = movie4
        }).ToList();

        var showtimeDatesMovie4Cinema = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );

        var showtimesMovie4Cinema = showtimeDatesMovie4Cinema4.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema.Id,
            MovieId = movie4.Id,
            ShowTime = showTimeDate,
            Cinema = cinema,
            Movie = movie4
        }).ToList();

        var showtimeDatesMovie4Cinema3 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );

        var showtimesMovie4Cinema3 = showtimeDatesMovie4Cinema3.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema3.Id,
            MovieId = movie4.Id,
            ShowTime = showTimeDate,
            Cinema = cinema3,
            Movie = movie4
        }).ToList();

        var showtimeDatesMovie5Cinema = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );

        var showtimesMovie5Cinema = showtimeDatesMovie5Cinema.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema.Id,
            MovieId = movie5.Id,
            ShowTime = showTimeDate,
            Cinema = cinema,
            Movie = movie5
        }).ToList();

        var showtimeDatesMovie5Cinema3 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );

        var showtimesMovie5Cinema3 = showtimeDatesMovie5Cinema3.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema3.Id,
            MovieId = movie5.Id,
            ShowTime = showTimeDate,
            Cinema = cinema3,
            Movie = movie5
        }).ToList();

        var showtimeDatesMovie6Cinema = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );
        
        var showtimesMovie6Cinema = showtimeDatesMovie6Cinema.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema.Id,
            MovieId = movie6.Id,
            ShowTime = showTimeDate,
            Cinema = cinema,
            Movie = movie6
        }).ToList();

        var showtimeDatesMovie7Cinema2 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );
        
        var showtimesMovie7Cinema2 = showtimeDatesMovie7Cinema2.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema2.Id,
            MovieId = movie7.Id,
            ShowTime = showTimeDate,
            Cinema = cinema2,
            Movie = movie7
        }).ToList();

        var showtimeDatesMovie8Cinema3 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );
        
        var showtimesMovie8Cinema3 = showtimeDatesMovie8Cinema3.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema3.Id,
            MovieId = movie8.Id,
            ShowTime = showTimeDate,
            Cinema = cinema3,
            Movie = movie8
        }).ToList();

        var showtimeDatesMovie9Cinema4 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );
        
        var showtimesMovie9Cinema4 = showtimeDatesMovie9Cinema4.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema4.Id,
            MovieId = movie9.Id,
            ShowTime = showTimeDate,
            Cinema = cinema4,
            Movie = movie9
        }).ToList();

        var showtimeDatesMovie10Cinema = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );
        
        var showtimesMovie10Cinema = showtimeDatesMovie10Cinema.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema.Id,
            MovieId = movie10.Id,
            ShowTime = showTimeDate,
            Cinema = cinema,
            Movie = movie10
        }).ToList();

        var showtimeDatesMovie11Cinema2 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );
        
        var showtimesMovie11Cinema2 = showtimeDatesMovie11Cinema2.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema2.Id,
            MovieId = movie11.Id,
            ShowTime = showTimeDate,
            Cinema = cinema2,
            Movie = movie11
        }).ToList();

        var showtimeDatesMovie12Cinema3 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );
        
        var showtimesMovie12Cinema3 = showtimeDatesMovie12Cinema3.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema3.Id,
            MovieId = movie12.Id,
            ShowTime = showTimeDate,
            Cinema = cinema3,
            Movie = movie12
        }).ToList();

        var showtimeDatesMovie13Cinema4 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );

        var showtimesMovie13Cinema4 = showtimeDatesMovie13Cinema4.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema4.Id,
            MovieId = movie13.Id,
            ShowTime = showTimeDate,
            Cinema = cinema4,
            Movie = movie13
        }).ToList();

        var showtimeDatesMovie14Cinema = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );
        
        var showtimesMovie14Cinema = showtimeDatesMovie14Cinema.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema.Id,
            MovieId = movie14.Id,
            ShowTime = showTimeDate,
            Cinema = cinema,
            Movie = movie14
        }).ToList();

        var showtimeDatesMovie15Cinema3 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );
        
        var showtimesMovie15Cinema3 = showtimeDatesMovie15Cinema3.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema3.Id,
            MovieId = movie15.Id,
            ShowTime = showTimeDate,
            Cinema = cinema3,
            Movie = movie15
        }).ToList();

        var showtimeDatesMovie16Cinema4 = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );
        
        var showtimesMovie16Cinema4 = showtimeDatesMovie16Cinema4.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema4.Id,
            MovieId = movie16.Id,
            ShowTime = showTimeDate,
            Cinema = cinema4,
            Movie = movie16
        }).ToList();

        var showtimeDatesMovie17Cinema = GenerateWeeklyShowtimes(
            baseDate: DateTime.Today,
            startTime: TimeSpan.FromHours(10), // First show starts at 10:00 AM
            interval: TimeSpan.FromHours(4),  // Shows are spaced 4 hours apart
            occurrencesPerDay: 3              // Three shows per day
        );
        
        var showtimesMovie17Cinema = showtimeDatesMovie17Cinema.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema.Id,
            MovieId = movie17.Id,
            ShowTime = showTimeDate,
            Cinema = cinema,
            Movie = movie17
        }).ToList();

        var event1 = new Event
        {
            Title = "Kerstklassiekers Marathon",
            Genre = "Komedie",
            Type = "Film Marathon",
            Price = "25",
            Description = "Bereid je voor op een magische filmmarathon vol humor, avontuur en hartverwarmende momenten! Tijdens ons speciale kerstfilm-evenement nemen we je mee terug naar de iconische avonturen van Kevin McCallister in Home Alone 1, 2 en 3.",
            Duration = 551,
            Director = "Lana Wachowski",
            Cast = ["Lana Wachowski", "Lilly Wachowski"],
            ReleaseDate = new DateTime(1999, 03, 31),

            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/KerstMaratonBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/KerstMaratonCover.webp",
            EventLink = "https://riseopslag2425.blob.core.windows.net/images/HomeAloneVideo1.mp4",
        };

        var event2 = new Event
        {
            Title = "Filmontbijt",
            Genre = "Action",
            Type = "Ontbijt met film",
            Price = "21",
            Description = "Heerlijk vegetarisch ontbijt in Grand Café De Republiek, speciaal samengesteld door onze lokale partner Marlow. Daarna kan je zalig ontspannen in onze comfortabele zeteltjes en genieten van een film. ",
            Duration = 210,
            Director = "Asif Kapadia",
            Cast = ["Amy Winehouse"],
            ReleaseDate = new DateTime(2015, 08, 15),

            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/MatrixVideoImage.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/FilmOntbijt.webp",
            EventLink = "https://riseopslag2425.blob.core.windows.net/images/MatrixVideo.mp4",
        };

        var event3 = new Event
        {
            Title = "Comedyavond",
            Genre = "Comedy",
            Type = "Comedyavond",
            Price = "20",
            Description = "In WICKED maken we kennis met het nog onbekende verhaal van de Witches of Oz. Cynthia Erivo is te zien als als Elphaba, een jonge vrouw die zich onzeker voelt vanwege haar ongebruikelijke groene huid en die haar ware kracht nog moet ontdekken. ",
            Duration = 160,
            Director = "Jon M. Chu",
            Cast = ["Jonathan Bailey", "Ariana Grande", "Cynthia Erivo"],
            ReleaseDate = new DateTime(2024, 12, 04),

            VideoPlaceholderUrl = "https://riseopslag2425.blob.core.windows.net/images/ForrestgumpBanner.webp",
            CoverImageUrl = "https://riseopslag2425.blob.core.windows.net/images/ComedyNightCover.webp",
            EventLink = "https://riseopslag2425.blob.core.windows.net/images/ForrestgumpVideo.mp4",
        };

        var showtimeDatesEventCinema = new List<DateTime>
   {
        DateTime.Parse("2025-12-04T14:00:00"),
            DateTime.Parse("2025-12-05T14:00:00"),
            DateTime.Parse("2025-12-06T14:00:00"),
            DateTime.Parse("2025-12-07T14:00:00"),
            DateTime.Parse("2025-12-08T14:00:00"),
            DateTime.Parse("2025-12-09T14:00:00"),
            DateTime.Parse("2025-12-10T14:00:00"),
            DateTime.Parse("2025-12-11T14:00:00"),
            DateTime.Parse("2025-12-12T14:00:00"),
            DateTime.Parse("2025-12-13T14:00:00"),
            DateTime.Parse("2025-12-18T14:00:00"),
            DateTime.Parse("2025-01-09T12:00:00"),
   };
        var showtimesEventCinema = showtimeDatesEventCinema.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema.Id,
            EventId = event1.Id,
            ShowTime = showTimeDate,
            Cinema = cinema,
            Event = event1
        }).ToList();

        var showtimeDatesEventCinema2 = new List<DateTime>
        {
           DateTime.Parse("2025-12-04T14:00:00"),
            DateTime.Parse("2025-12-05T14:00:00"),
            DateTime.Parse("2025-12-06T14:00:00"),
            DateTime.Parse("2025-12-07T14:00:00"),
            DateTime.Parse("2025-12-08T14:00:00"),
            DateTime.Parse("2025-12-09T14:00:00"),
            DateTime.Parse("2025-12-10T14:00:00"),
            DateTime.Parse("2025-12-11T14:00:00"),
            DateTime.Parse("2025-12-12T14:00:00"),
            DateTime.Parse("2025-12-13T14:00:00"),
            DateTime.Parse("2025-12-04T14:00:00"),
            DateTime.Parse("2025-12-18T14:00:00"),
            DateTime.Parse("2025-01-09T12:00:00"),

        };
        var showtimesEventCinema2 = showtimeDatesEventCinema2.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema2.Id,
            EventId = event1.Id,
            ShowTime = showTimeDate,
            Cinema = cinema2,
            Event = event1
        }).ToList();

        var showtimeDatesEvent2Cinema = new List<DateTime>
        {
             DateTime.Parse("2025-12-04T14:00:00"),
            DateTime.Parse("2025-12-05T14:00:00"),
            DateTime.Parse("2025-12-06T14:00:00"),
            DateTime.Parse("2025-12-07T14:00:00"),
            DateTime.Parse("2025-12-08T14:00:00"),
            DateTime.Parse("2025-12-09T14:00:00"),
            DateTime.Parse("2025-12-10T14:00:00"),
            DateTime.Parse("2025-12-11T14:00:00"),
            DateTime.Parse("2025-12-12T14:00:00"),
            DateTime.Parse("2025-12-13T14:00:00"),
             DateTime.Parse("2025-12-04T15:00:00"),
            DateTime.Parse("2025-12-18T09:00:00"),
            DateTime.Parse("2025-01-09T11:00:00"),
        };
        var showtimesEvent2Cinema = showtimeDatesEvent2Cinema.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema.Id,
            EventId = event2.Id,
            ShowTime = showTimeDate,
            Cinema = cinema,
            Event = event2
        }).ToList();

        var showtimeDatesEvent2Cinema3 = new List<DateTime>{
   DateTime.Parse("2025-12-04T14:00:00"),
            DateTime.Parse("2025-12-05T14:00:00"),
            DateTime.Parse("2025-12-06T14:00:00"),
            DateTime.Parse("2025-12-07T14:00:00"),
            DateTime.Parse("2025-12-08T14:00:00"),
            DateTime.Parse("2025-12-09T14:00:00"),
            DateTime.Parse("2025-12-10T14:00:00"),
            DateTime.Parse("2025-12-11T14:00:00"),
            DateTime.Parse("2025-12-12T14:00:00"),
            DateTime.Parse("2025-12-13T14:00:00"),
              DateTime.Parse("2025-12-04T13:00:00"),
            DateTime.Parse("2025-12-18T10:00:00"),
            DateTime.Parse("2025-01-09T16:00:00"),

        };
        var showtimesEvent2Cinema3 = showtimeDatesEvent2Cinema3.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema3.Id,
            EventId = event2.Id,
            ShowTime = showTimeDate,
            Cinema = cinema3,
            Event = event2
        }).ToList();

        var showtimeDatesEvent3Cinema4 = new List<DateTime> {

            DateTime.Parse("2025-12-04T14:00:00"),
            DateTime.Parse("2025-12-05T14:00:00"),
            DateTime.Parse("2025-12-06T14:00:00"),
            DateTime.Parse("2025-12-07T14:00:00"),
            DateTime.Parse("2025-12-08T14:00:00"),
            DateTime.Parse("2025-12-09T14:00:00"),
            DateTime.Parse("2025-12-10T14:00:00"),
            DateTime.Parse("2025-12-11T14:00:00"),
            DateTime.Parse("2025-12-12T14:00:00"),
            DateTime.Parse("2025-12-13T14:00:00"),
              DateTime.Parse("2025-12-04T14:00:00"),
            DateTime.Parse("2025-12-18T14:00:00"),
            DateTime.Parse("2025-01-09T12:00:00"),


        };
        var showtimesEvent3Cinema4 = showtimeDatesEvent3Cinema4.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema4.Id,
            EventId = event3.Id,
            ShowTime = showTimeDate,
            Cinema = cinema4,
            Event = event3
        }).ToList();

        var showtimeDatesEvent3Cinema2 = new List<DateTime> {

            DateTime.Parse("2025-12-04T14:00:00"),
            DateTime.Parse("2025-12-05T14:00:00"),
            DateTime.Parse("2025-12-06T14:00:00"),
            DateTime.Parse("2025-12-07T14:00:00"),
            DateTime.Parse("2025-12-08T14:00:00"),
            DateTime.Parse("2025-12-09T14:00:00"),
            DateTime.Parse("2025-12-10T14:00:00"),
            DateTime.Parse("2025-12-11T14:00:00"),
            DateTime.Parse("2025-12-12T14:00:00"),
            DateTime.Parse("2025-12-13T14:00:00"),
          DateTime.Parse("2025-12-04T13:00:00"),
            DateTime.Parse("2025-12-18T14:30:00"),
            DateTime.Parse("2025-01-09T17:00:00"),

        };
        var showtimesEvent3Cinema2 = showtimeDatesEvent3Cinema4.Select(showTimeDate => new Showtime
        {
            CinemaId = cinema2.Id,
            EventId = event3.Id,
            ShowTime = showTimeDate,
            Cinema = cinema2,
            Event = event3
        }).ToList();

        var movieWatchlist = new MovieWatchlist
        {
            WatchlistId = watchlist.Id,
            MovieId = movie.Id,
            Movie = movie,
            Watchlist = watchlist,
        };
        watchlist.MovieWatchlists.Add(movieWatchlist);
        account.Watchlist = watchlist;
       
        var giftcard = new Giftcard
        {
            AccountId = account.Id,
            Account = account,
            value = 10,
        };
        var tenturncard1 = new Tenturncard
        {
            ActivationCode = "GBTA123456",
            AmountLeft = 10,
        };
        var tenturncard2 = new Tenturncard
        {
            ActivationCode = "GBTA654321",
            AmountLeft = 10,
        };
        var tenturncard3 = new Tenturncard
        {
            ActivationCode = "GBTA789100",
            AmountLeft = 10,
        };
        var ticket = new Ticket
        {
            AccountId = account.Id,
            Account = account,
            DateTime = DateTime.Now,
            Location = "Cinema XYZ",
            Type = TicketType.Standaard,
            MovieId = movie?.Id,
            Movie = movie,
            EventId = null,
            Event = null
        };

        dbContext.Accounts.Add(account);

        dbContext.Movies.Add(movie);
        dbContext.Movies.Add(movie2);
        dbContext.Movies.Add(movie3);
        dbContext.Movies.Add(movie4);
        dbContext.Movies.Add(movie5);
        dbContext.Movies.Add(movie6);
        dbContext.Movies.Add(movie7);
        dbContext.Movies.Add(movie8);
        dbContext.Movies.Add(movie9);
        dbContext.Movies.Add(movie10);
        dbContext.Movies.Add(movie11);
        dbContext.Movies.Add(movie12);
        dbContext.Movies.AddRange(new List<Movie> { movie13, movie14, movie15, movie16, movie17,movie18,movie19,movie20 });


        dbContext.Events.Add(event1);
        dbContext.Events.Add(event2);
        dbContext.Events.Add(event3);
     
        dbContext.Cinemas.Add(cinema);
        dbContext.Cinemas.Add(cinema2);
        dbContext.Cinemas.Add(cinema3);
        dbContext.Cinemas.Add(cinema4);

        dbContext.MovieWatchlist.Add(movieWatchlist);


        dbContext.Showtime.AddRange(showtimesMovie2Cinema);
        dbContext.Showtime.AddRange(showtimesMovieCinema2);
        dbContext.Showtime.AddRange(showtimesMovieCinema);
        dbContext.Showtime.AddRange(showtimesMovie2Cinema3);
        dbContext.Showtime.AddRange(showtimesMovie3Cinema4);
        dbContext.Showtime.AddRange(showtimesMovie3Cinema2);
        dbContext.Showtime.AddRange(showtimesMovie4Cinema4);
        dbContext.Showtime.AddRange(showtimesMovie4Cinema);
        dbContext.Showtime.AddRange(showtimesMovie4Cinema3);
        dbContext.Showtime.AddRange(showtimesMovie5Cinema);
        dbContext.Showtime.AddRange(showtimesMovie5Cinema3);

        dbContext.Showtime.AddRange(showtimesMovie6Cinema);
        dbContext.Showtime.AddRange(showtimesMovie7Cinema2);
        dbContext.Showtime.AddRange(showtimesMovie8Cinema3);
        dbContext.Showtime.AddRange(showtimesMovie9Cinema4);
        dbContext.Showtime.AddRange(showtimesMovie10Cinema);
        dbContext.Showtime.AddRange(showtimesMovie11Cinema2);
        dbContext.Showtime.AddRange(showtimesMovie12Cinema3);

        // future movies
        dbContext.Showtime.AddRange(showtimesMovie13Cinema4);
        dbContext.Showtime.AddRange(showtimesMovie14Cinema);
        dbContext.Showtime.AddRange(showtimesMovie15Cinema3);
        dbContext.Showtime.AddRange(showtimesMovie16Cinema4);
        dbContext.Showtime.AddRange(showtimesMovie17Cinema);

        dbContext.Showtime.AddRange(showtimesEvent2Cinema);
        dbContext.Showtime.AddRange(showtimesEventCinema2);
        dbContext.Showtime.AddRange(showtimesEventCinema);
        dbContext.Showtime.AddRange(showtimesEvent2Cinema3);
        dbContext.Showtime.AddRange(showtimesEvent3Cinema4);
        dbContext.Showtime.AddRange(showtimesEvent3Cinema2);
      

        dbContext.Giftcards.Add(giftcard);
        dbContext.Tickets.Add(ticket);
        dbContext.Tenturncards.Add(tenturncard1);
        dbContext.Tenturncards.Add(tenturncard2);
        dbContext.Tenturncards.Add(tenturncard3);
        dbContext.SaveChanges();
    }
}