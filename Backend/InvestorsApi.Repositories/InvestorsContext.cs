using InvestorsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestorsApi.Repositories
{
    public class InvestorsContext : DbContext
    {
        public InvestorsContext(DbContextOptions<InvestorsContext> options) : base(options) { }

        public DbSet<Investor> Investors { get; set; }
        public DbSet<Commitment> Commitments { get; set; }
    }
}