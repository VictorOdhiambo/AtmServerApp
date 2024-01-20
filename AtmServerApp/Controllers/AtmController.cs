using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;

namespace AtmServerApp.Controllers
{
    [ApiController]
    [Route("/api/v1/atm/")]
    public class AtmController : Controller
    {
        private AtmService atmService;

        public AtmController(AtmService atmService)
        {
            this.atmService = atmService;
        }

        [Authorize]
        [HttpPost("transfer-funds")]
        public IActionResult TransferFunds(RequestPayload payload)
        {
            var res = atmService.TransferFunds(payload.AccountNoTo, payload.Amount, payload.UserId);
            if (res.Message.Contains("success"))
            {
                return Ok(res);
            }
            else if (res.Message.Contains("failed"))
            {
                return StatusCode(StatusCodes.Status417ExpectationFailed, res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        [Authorize]
        [HttpPost("withdraw-funds")]
        public IActionResult WithdrawFunds(RequestPayload payload)
        {
            var res = atmService.WithdrawFunds(payload.UserId, payload.AtmId, payload.Amount);

            if (res.Message.Contains("success"))
            {
                return Ok(res);
            }
            else if (res.Message.Contains("failed"))
            {
                return StatusCode(StatusCodes.Status417ExpectationFailed, res);
            }
            else
            {
                return BadRequest(res);
            }
        }
    }
}
