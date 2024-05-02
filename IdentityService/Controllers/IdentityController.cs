using Common.DTOs.UserDTOs;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<IdentityController> _logger;
    public IdentityController(
        IMediator mediator,
        ILogger<IdentityController> logger
        )
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var token = await _mediator.Send(request);
        return token.Result;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        var token = await _mediator.Send(request);
        return token.Result;
    }
}
