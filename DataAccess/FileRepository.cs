using System.Globalization;
using System.Text;
using TicketsDataAggregator.Models;
using UglyToad.PdfPig;

namespace TicketsDataAggregator.DataAccess
{
    public class FileRepository : IFileRepository
    {
        public string ReadFromPdf(string fileName)
        {
            using (var document = PdfDocument.Open(fileName))
            {
                var page = document.GetPage(1);
                var pageText = page.Text;
                return pageText;
            }
        }

        public void WriteToTxt(List<Ticket> tickets, string fileName)
        {
            // Save tickets to txt
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var stringBuilder = new StringBuilder();
            foreach (var ticket in tickets)
            {
                foreach (var movie in ticket.Movies)
                {
                    stringBuilder.AppendFormat("{0, -30}|{1, -10}|{2, -5}", movie.Title, movie.DateTime.ToShortDateString(), movie.DateTime.ToShortTimeString());
                    stringBuilder.AppendLine();
                }
            }

            File.WriteAllText(fileName, stringBuilder.ToString());
        }
    }
}