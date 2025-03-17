using Rise.Shared.Events;
using Rise.Shared.Movies;

namespace Rise.Shared.Tickets{
    public enum TicketType
    {
        Standaard,
        Senior,   
        Student   
    }

    public class TicketDto
    {

        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        public  TicketType Type { get; set; }

            public decimal Price
            {
                get
                {
                    return Type switch
                    {
                        TicketType.Standaard => 12.0m,
                        TicketType.Senior => 11.5m,
                        TicketType.Student => 10.0m,
                        _ => 12.0m
                    };
                }
            }
        public string Location { get; set; }

        public MovieDto Movie { get; init; }
        public EventDto Event { get; init; }
    }
}
