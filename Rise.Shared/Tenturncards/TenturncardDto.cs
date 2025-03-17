namespace Rise.Shared.Tenturncards
{
    public class TenturncardDto
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public string? ActivationCode { get; set; }
        public bool IsActivated { get; set; }
        public int AmountLeft { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}