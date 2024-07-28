using InvestorsApi.Models;
using InvestorsApi.Repositories;

namespace InvestorsApi.Services
{
    public interface IInvestorService
    {
        Task<IEnumerable<InvestorDto>> GetInvestorsAsync();
        Task<InvestorDto> GetInvestorByIdAsync(int id);
    }

    public class InvestorService : IInvestorService
    {
        private readonly IInvestorRepository _investorRepository;
        private readonly ICommitmentRepository _commitmentRepository;

        public InvestorService(IInvestorRepository investorRepository, ICommitmentRepository commitmentRepository)
        {
            _investorRepository = investorRepository;
            _commitmentRepository = commitmentRepository;
        }

        public async Task<IEnumerable<InvestorDto>> GetInvestorsAsync()
        {
            var investors = await _investorRepository.GetAllAsync();
            var investorDtos = new List<InvestorDto>();

            foreach (var investor in investors)
            {
                var commitments = await _commitmentRepository.GetAllCommitmentsAsync(investor.Id);
                var totalCommitmentAmount = commitments.Sum(c => c.Amount);

                var investorDto = new InvestorDto
                {
                    Id = investor.Id,
                    Name = investor.Name,
                    InvestorType = investor.InvestorType,
                    Country = investor.Country,
                    DateAdded = investor.DateAdded,
                    LastUpdated = investor.LastUpdated,
                    TotalCommitmentAmount = totalCommitmentAmount
                };

                investorDtos.Add(investorDto);
            }

            return investorDtos;
        }

        public async Task<InvestorDto> GetInvestorByIdAsync(int id)
        {
            var investor = await _investorRepository.GetByIdAsync(id);

            if (investor == null)
            {
                return null;
            }

            var commitments = await _commitmentRepository.GetAllCommitmentsAsync(id);
            var totalCommitmentAmount = commitments.Sum(c => c.Amount);

            var investorDto = new InvestorDto
            {
                Id = investor.Id,
                Name = investor.Name,
                InvestorType = investor.InvestorType,
                Country = investor.Country,
                DateAdded = investor.DateAdded,
                LastUpdated = investor.LastUpdated,
                TotalCommitmentAmount = totalCommitmentAmount,
                Commitments = commitments,
            };

            return investorDto;
        }
    }
}
