using TicketsDataAggregator.Models;

namespace TicketsDataAggregator.DataAccess
{
    public interface IFileRepository
    {
        string ReadFromPdf(string fileName);
        void WriteToTxt(List<Ticket> tickets, string fileName);
    }
}