using MediatR;
using Microsoft.AspNetCore.Mvc;
using KycService.Application.Commands.SubmitKyc;
using KycService.Application.Commands.VerifyKyc;
using KycService.Application.Queries.GetKycStatus;

namespace KycService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KycController(IMediator mediator) : ControllerBase
{
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitKyc([FromBody] SubmitKycCommand command)
    {
        var result = await mediator.Send(command);
        return Ok((result));
    }

    [HttpGet("status/{userId}")]
    public async Task<IActionResult> GetStatus(Guid userId)
    {
        var result = await mediator.Send(new GetKycStatusQuery(userId));
        return Ok(result);
    }
    
    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerifyKycCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}