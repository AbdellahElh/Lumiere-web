using Rise.Shared.Events;
using Rise.Shared.Movies;

namespace Rise.Shared.Tickets{
 

    public class AddTicketDto
    {
        public int? MovieId { get; set; }
        public int? EventId { get; set; }
        public string CinemaName { get; set; }
        public DateTime ShowTime { get; set; }
    }
}
