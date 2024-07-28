using InvestorsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestorsApi.Repositories
{
    public interface ICommitmentRepository : IRepository<Commitment>
    {
        Task<IEnumerable<Commitment>> GetAllCommitmentsAsync(int investorId);
        Task<IEnumerable<Commitment>> GetCommitmentsAsync(string assetClass, int investorId);
    }

    public class CommitmentRepository : ICommitmentRepository
    {
        private readonly InvestorsContext _context;

        public CommitmentRepository(InvestorsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Commitment>> GetCommitmentsAsync(string assetClass, int investorId)
        {
            return await _context.Commitments
                .Where(c => c.AssetClass == assetClass && c.InvestorId == investorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Commitment>> GetAllCommitmentsAsync(int investorId)
        {
            return await _context.Commitments
                .Where(c => c.InvestorId == investorId)
                .ToListAsync();
        }

        public Task<IEnumerable<Commitment>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Commitment> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}