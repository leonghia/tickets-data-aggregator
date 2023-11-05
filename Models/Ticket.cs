namespace TicketsDataAggregator.Models
{
    public class Ticket
    {
        public ICollection<Movie> Movies { get; init; }
        public string Country { get; init; }
    }
}