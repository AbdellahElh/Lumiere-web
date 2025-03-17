using Rise.Domain.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Domain.Giftcards;
public class Giftcard : Entity
{
    public required int AccountId { get; set; }
    public required Account Account { get; set; }

    public required int value { get; set; }
}