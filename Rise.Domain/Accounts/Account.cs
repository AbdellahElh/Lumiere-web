using Rise.Domain.Giftcards;
using Rise.Domain.Movies;
using Rise.Domain.Tenturncards;
using Rise.Domain.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Domain.Accounts;
public class Account : Entity
{
    public required string Email { get; set; }
    public required Watchlist Watchlist { get; set; }
    public ICollection<Tenturncard> Tenturncards { get; set; } = new List<Tenturncard>();

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public ICollection<Giftcard> Giftcards { get; set; } = new List<Giftcard>();

    public Account() { }

}
