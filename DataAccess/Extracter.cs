using System.Globalization;
using TicketsDataAggregator.Models;

namespace TicketsDataAggregator.DataAccess
{
    public class Extracter
    {
        private readonly FileRepository _fileRepository;

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

        private Movie ExtractMovie(string input)
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

        private IEnumerable<Movie> ExtractMovies(string pageText)
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

        private string[] ExtractMoviesAsArrayOfString(string pageText)
        {
            pageText = pageText.Split("Visit us:")[0];
            var stringMovies = pageText.Split("Title:");
            return stringMovies;
        }

        private string ExtractCountry(string pageText)
        {
            return pageText.Substring(pageText.LastIndexOf('.') + 1);
        }
        private Ticket ExtractTicket(string pageText)
        {
            var countryCultures = new Dictionary<string, string>
        {
            { "com", "en-US" },
            { "jp", "ja-JP" },
            { "fr",  "fr-FR"}
        };
            var country = ExtractCountry(pageText);
            CultureInfo.CurrentCulture = new CultureInfo(countryCultures[country]);
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