namespace InvestorsApi.Models
{

    public class InvestorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InvestorType { get; set; }
        public string Country { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime LastUpdated { get; set; }
        public IEnumerable<Commitment> Commitments { get; set; }
        public decimal TotalCommitmentAmount { get; set; }
    }
}