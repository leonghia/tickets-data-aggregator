using System.Globalization;
using TicketsDataAggregator.Models;

namespace TicketsDataAggregator.DataAccess
{
    public class Extracter
    {
        private readonly FileRepository _fileRepository;
        private readonly Dictionary<string, CultureInfo> _countryCodes = new()
        {
            { "com", new CultureInfo("en-US") },
            { "jp", new CultureInfo("ja-JP") },
            { "fr", new CultureInfo("fr-FR") }
        };

        public Extracter(FileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public IEnumerable<Ticket> ExtractTickets(string directory)
        {
            var files = Directory.GetFiles(directory);

            var tickets = new List<Ticket>();

            foreach (var file in files)
            {
                var pageText = _fileRepository.ReadFromPdf(file);
                var ticket = ExtractTicket(pageText);
                tickets.Add(ticket);
            }

            return tickets;

        }

        private static Movie ExtractMovie(string input)
        {
            var title = input.Split("Date:")[0];
            var dateTime = input.Split("Date:")[1].Split("Time:");
            var date = dateTime[0];
            var time = dateTime[1];
            var dateObj = DateTime.Parse(date + " " + time);
            var movie = new Movie
            {
                Title = title,
                DateTime = dateObj
            };
            return movie;
        }

        private static IEnumerable<Movie> ExtractMovies(string pageText)
        {
            var movies = new List<Movie>();
            var stringMovies = ExtractMoviesAsArrayOfString(pageText);
            for (var i = 1; i < stringMovies.Length; i++)
            {
                var movie = ExtractMovie(stringMovies[i]);
                movies.Add(movie);
            }
            return movies;
        }

        private static string[] ExtractMoviesAsArrayOfString(string pageText)
        {
            pageText = pageText.Split("Visit us:")[0];
            var stringMovies = pageText.Split("Title:");
            return stringMovies;
        }

        private static string ExtractCountry(string pageText)
        {
            return pageText.Substring(pageText.LastIndexOf('.') + 1);
        }

        private Ticket ExtractTicket(string pageText)
        {  
            var country = ExtractCountry(pageText);
            CultureInfo.CurrentCulture = _countryCodes[country];
            var movies = ExtractMovies(pageText).ToList();
            var ticket = new Ticket
            {
                Country = country,
                Movies = movies
            };
            return ticket;
        }
    }
}