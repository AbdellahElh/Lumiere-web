using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rise.Shared.Giftcards;
using Rise.Shared.Tenturncards;
using Rise.Shared.Tickets;
using Rise.Shared.Watchlists;

namespace Rise.Shared.Accounts;

public class AccountDto
{   
    public string Email { get; init; }
    public string Password { get; init; }
    public WatchlistDto Watchlist { get; set; } = new WatchlistDto();
    public ICollection<TenturncardDto> Tenturncards { get; set; } = new List<TenturncardDto>();
    public ICollection<TicketDto> Tickets { get; set; } = new List<TicketDto>();
    public ICollection<GiftcardDto> Giftcards { get; set; } = new List<GiftcardDto>();
}