using Application;
using Application.DTOs.Loan;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApplication2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim);
        }

        [HttpPost]
        public IActionResult CreateLoan([FromBody] CreateLoanDto dto)
        {
            var userId = GetCurrentUserId();
            var result = _loanService.CreateLoan(userId, dto);
            return Ok(result);
        }

        [HttpGet("my")]
        public IActionResult GetMyLoans()
        {
            var userId = GetCurrentUserId();
            var result = _loanService.GetMyLoans(userId);
            return Ok(result);
        }

        [HttpPut("my/{loanId}")]
        public IActionResult UpdateMyLoan(int loanId, [FromBody] UpdateLoanDto dto)
        {
            var userId = GetCurrentUserId();
            var result = _loanService.UpdateMyLoan(userId, loanId, dto);
            return Ok(result);
        }

        [HttpDelete("my/{loanId}")]
        public IActionResult DeleteMyLoan(int loanId)
        {
            var userId = GetCurrentUserId();
            var result = _loanService.DeleteMyLoan(userId, loanId);
            return Ok(new { message = "Loan deleted successfully" });
        }

        [Authorize(Roles = "Accountant")]
        [HttpGet("{loanId}")]
        public IActionResult GetLoanById(int loanId)
        {
            var result = _loanService.GetLoanById(loanId);
            return Ok(result);
        }

        [Authorize(Roles = "Accountant")]
        [HttpGet("all")]
        public IActionResult GetAllLoans()
        {
            var result = _loanService.GetAllLoans();
            return Ok(result);
        }

        [Authorize(Roles = "Accountant")]
        [HttpPut("{loanId}/status")]
        public IActionResult UpdateLoanStatus(int loanId, [FromBody] UpdateLoanStatusDto dto)
        {
            var result = _loanService.UpdateLoanStatus(loanId, dto);
            return Ok(result);
        }

        [Authorize(Roles = "Accountant")]
        [HttpPut("{loanId}")]
        public IActionResult UpdateLoan(int loanId, [FromBody] UpdateLoanDto dto)
        {
            var result = _loanService.UpdateLoan(loanId, dto);
            return Ok(result);
        }

        [Authorize(Roles = "Accountant")]
        [HttpDelete("{loanId}/admin")]
        public IActionResult DeleteLoanByAccountant(int loanId)
        {
            var result = _loanService.DeleteLoan(loanId);
            return Ok(new { message = "Loan deleted successfully" });
        }
    }
}
