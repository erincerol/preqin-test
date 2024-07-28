using FluentAssertions;
using InvestorsApi.Controllers;
using InvestorsApi.Models;
using InvestorsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InvestorsApi.Tests
{
    public class InvestorsControllerTests
    {
        private readonly Mock<IInvestorService> _investorServiceMock;
        private readonly InvestorsController _controller;

        public InvestorsControllerTests()
        {
            _investorServiceMock = new Mock<IInvestorService>();
            _controller = new InvestorsController(_investorServiceMock.Object);
        }

        [Fact]
        public async Task GetInvestors_ShouldReturnOkWithInvestors()
        {
            var investors = new List<InvestorDto>
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
                    },
                    TotalCommitmentAmount = 100
                }
            };

            _investorServiceMock.Setup(service => service.GetInvestorsAsync()).ReturnsAsync(investors);

            var result = await _controller.GetInvestors();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            var returnedInvestors = okResult.Value as IEnumerable<InvestorDto>;
            returnedInvestors.Should().BeEquivalentTo(investors);
        }

        [Fact]
        public async Task GetInvestor_ShouldReturnOkWithInvestor()
        {
            var investor = new InvestorDto
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
                },
                TotalCommitmentAmount = 100
            };

            _investorServiceMock.Setup(service => service.GetInvestorByIdAsync(It.IsAny<int>())).ReturnsAsync(investor);

            var result = await _controller.GetInvestor(1);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            var returnedInvestor = okResult.Value as InvestorDto;
            returnedInvestor.Should().BeEquivalentTo(investor);
        }

        [Fact]
        public async Task GetInvestor_ShouldReturnNotFound()
        {
            _investorServiceMock.Setup(service => service.GetInvestorByIdAsync(It.IsAny<int>())).ReturnsAsync((InvestorDto)null);

            var result = await _controller.GetInvestor(1);

            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult.StatusCode.Should().Be(404);
        }
    }
}
