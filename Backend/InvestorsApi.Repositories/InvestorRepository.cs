using InvestorsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestorsApi.Repositories
{
    public interface IInvestorRepository : IRepository<Investor>
    {
    }

    public class InvestorRepository : IInvestorRepository
    {
        private readonly InvestorsContext _context;

        public InvestorRepository(InvestorsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Investor>> GetAllAsync()
        {
            return await _context.Investors.Include(i => i.Commitments).ToListAsync();
        }

        public async Task<Investor> GetByIdAsync(int id)
        {
            return await _context.Investors.Include(i => i.Commitments).FirstOrDefaultAsync(i => i.Id == id);
        }

        //Add, Update, Delete
    }
}
