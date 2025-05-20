using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Commands.RegisterUser;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}