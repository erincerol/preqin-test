using InvestorsApi.Models;
using InvestorsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvestorsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestorsController : ControllerBase
    {
        private readonly IInvestorService _investorService;

        public InvestorsController(IInvestorService investorService)
        {
            _investorService = investorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvestorDto>>> GetInvestors()
        {
            var investors = await _investorService.GetInvestorsAsync();
            return Ok(investors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvestorDto>> GetInvestor(int id)
        {
            var investor = await _investorService.GetInvestorByIdAsync(id);

            if (investor == null)
            {
                return NotFound();
            }

            return Ok(investor);
        }
    }
}
