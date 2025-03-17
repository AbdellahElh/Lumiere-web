using Rise.Domain.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Domain.Tenturncards;
public class Tenturncard : Entity
{
    public int? AccountId { get; set; }
    public required string ActivationCode { get; set; }
    public bool IsActivated { get; set; } = false; 
    public Account? Account { get; set; }
    public int AmountLeft { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? ExpirationDate { get; set; }

}
