using TicketsDataAggregator.DataAccess;

namespace TicketsDataAggregator.App
{
    public class TicketsDataAggregatorApp
    {
        private readonly FileRepository _fileRepository;
        private readonly Extracter _extracter;

        public TicketsDataAggregatorApp(FileRepository fileRepository, Extracter extracter)
        {
            _fileRepository = fileRepository;
            _extracter = extracter;
        }

        public void Run(string directory, string fileName)
        {
            var tickets = _extracter.ExtractTickets(directory).ToList();
            _fileRepository.WriteToTxt(tickets, fileName);
        }
    }
}