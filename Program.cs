using TicketsDataAggregator.App;
using TicketsDataAggregator.DataAccess;

const string directory = "./Tickets";
const string fileName = "aggregatedTickets.txt";

try
{
    var fileRepository = new FileRepository();
    var app = new TicketsDataAggregatorApp(fileRepository, new Extracter(fileRepository));
    app.Run(directory, fileName);
}
catch (Exception ex)
{
    Console.WriteLine("Something went wrong. " + ex);
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();