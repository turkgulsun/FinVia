using MediatR;
using Microsoft.AspNetCore.Mvc;
using WalletService.Application.Commands.CreateWallet;
using WalletService.Application.Commands.CreditWallet;
using WalletService.Application.Commands.DebitWallet;
using WalletService.Application.DTOs;
using WalletService.Application.Queries.GetWalletById;
using WalletService.Application.Queries.GetWalletByUserId;

namespace WalletService.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class WalletsController(IMediator mediator, ILogger<WalletsController> logger) : ControllerBase
{
    [HttpGet("by-id/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetWalletByIdQuery(id));
        return Ok(result);
    }
    
    [HttpGet("by-user/{userId}")]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        logger.LogInformation("Getting wallet for userId: {UserId}", userId);

        var result = await mediator.Send(new GetWalletByUserIdQuery(userId));
        return Ok(result);
    }

    [HttpPost("credit")]
    public async Task<IActionResult> Credit([FromBody] CreditWalletCommand command)
    {
        var result = await mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWallet([FromBody] CreateWalletRequest request)
    {
        var command = new CreateWalletCommand(request.UserId, request.Currency);
        var result = await mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("debit")]
    public async Task<IActionResult> Debit([FromBody] DebitWalletCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

}