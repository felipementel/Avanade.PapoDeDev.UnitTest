using Avanade.PapoDeDev.UnitTest.Domain.Aggregates.Bank.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Avanade.PapoDeDev.UnitTest.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1", Deprecated = false)]
    [ApiExplorerSettings(GroupName = "v1")]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly ILogger<BankController> _logger;

        public BankController(
            IBankService bankService,
            ILogger<BankController> logger)
        {
            _bankService = bankService;
            _logger = logger;
        }

        [HttpPost(template: "Account")]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Domain.Aggregates.Account.Entities.Account), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAccountAsync(
            [FromBody] Domain.Aggregates.Account.Entities.Account account)
        {
            var item = await _bankService.AddAccountAsync(account);

            if (string.IsNullOrWhiteSpace(item))
            {
                return Ok(item);
            }
            else
            {
                return BadRequest(error: item);
            }
        }

        /// <summary>
        /// Deposit
        /// </summary>
        /// <remarks>
        /// Request Sample 
        /// </remarks>
        /// <param name="accountId">Account number</param>
        /// <param name="value">Value in USD</param>
        /// <returns></returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>  
        /// <response code="500">When exception happen</response> 
        [HttpPut(template: "Deposit/{accountId}/value/{value}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Domain.Aggregates.Account.Entities.Account), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DepositAsync(
            [FromRoute] string accountId,
            [FromRoute] string value)
        {
            var item = await _bankService.DepositAsync(accountId, Decimal.Parse(value));

            if (item == string.Empty)
            {
                return Ok(item);
            }
            else
            {
                return BadRequest(error: item);
            }
        }

        /// <summary>
        /// WithDraw
        /// </summary>
        /// <remarks>
        /// Request Sample 
        /// </remarks>
        /// <param name="accountId">Account Number</param>
        /// <param name="value">Value in USD</param>
        /// <returns></returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>  
        /// <response code="500">When exception happen</response> 
        [HttpPut(template: "WithDraw/{accountId}/value/{value}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Domain.Aggregates.Account.Entities.Account), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> WithDrawAsync(
        [FromRoute] string accountId,
        [FromRoute] string value)
        {
            var item = await _bankService.WithDrawAsync(accountId, Decimal.Parse(value));

            if (item == string.Empty)
            {
                return Ok(item);
            }
            else
            {
                return BadRequest(error: item);
            }
        }
    }
}