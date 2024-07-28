using FluentAssertions;
using InvestorsApi.Models;
using InvestorsApi.Repositories;
using InvestorsApi.Services;
using Moq;
using Xunit;

namespace InvestorsApi.Tests
{
    public class InvestorServiceTests
    {
        private readonly Mock<IInvestorRepository> _mockInvestorRepository;
        private readonly Mock<ICommitmentRepository> _mockCommitmentRepository;
        private readonly IInvestorService _investorService;

        public InvestorServiceTests()
        {
            _mockInvestorRepository = new Mock<IInvestorRepository>();
            _mockCommitmentRepository = new Mock<ICommitmentRepository>();
            _investorService = new InvestorService(_mockInvestorRepository.Object, _mockCommitmentRepository.Object);
        }

        [Fact]
        public async Task GetInvestorsAsync_ShouldReturnInvestorsWithCommitmentsAndTotalCommitmentAmount()
        {
            var investors = new List<Investor>
            {
                new()
                {
                    Id = 1,
                    Name = "Investor 1",
                    InvestorType = "Type 1",
                    Country = "Country 1",
                    DateAdded = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow,
                    Commitments = new List<Commitment>
                    {
                        new() { Id = 1, AssetClass = "Class 1", Amount = 100, Currency = "GBP", InvestorId = 1 }
                    }
                }
            };

            _mockInvestorRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(investors);
            _mockCommitmentRepository.Setup(repo => repo.GetAllCommitmentsAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
                investors.FirstOrDefault(i => i.Id == id)?.Commitments);

            var result = await _investorService.GetInvestorsAsync();

            result.Should().HaveCount(1);
            result.First().TotalCommitmentAmount.Should().Be(100);
        }

        [Fact]
        public async Task GetInvestorByIdAsync_ShouldReturnInvestorWithCommitmentsAndTotalCommitmentAmount()
        {
            var investor = new Investor
            {
                Id = 1,
                Name = "Investor 1",
                InvestorType = "Type 1",
                Country = "Country 1",
                DateAdded = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                Commitments = new List<Commitment>
                {
                    new() { Id = 1, AssetClass = "Class 1", Amount = 100, Currency = "GBP", InvestorId = 1 }
                }
            };

            _mockInvestorRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(investor);
            _mockCommitmentRepository.Setup(repo => repo.GetAllCommitmentsAsync(It.IsAny<int>())).ReturnsAsync(investor.Commitments);

            var result = await _investorService.GetInvestorByIdAsync(1);

            result.Should().NotBeNull();
            result.TotalCommitmentAmount.Should().Be(100);
        }
    }
}
