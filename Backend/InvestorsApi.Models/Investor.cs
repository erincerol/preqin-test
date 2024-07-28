using System.ComponentModel.DataAnnotations;

namespace InvestorsApi.Models
{
    public class Investor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string InvestorType { get; set; }

        [Required]
        public string Country { get; set; }

        public DateTime DateAdded { get; set; }
        public DateTime LastUpdated { get; set; }

        public ICollection<Commitment> Commitments { get; set; } = new List<Commitment>();

    }
}