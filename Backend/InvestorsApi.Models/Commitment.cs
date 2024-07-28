using System.ComponentModel.DataAnnotations;

namespace InvestorsApi.Models
{
    public class Commitment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AssetClass { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        public int InvestorId { get; set; }
    }
}
