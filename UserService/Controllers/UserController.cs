using Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Queries;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;
    public UserController(
        IMediator mediator,
        ILogger<UserController> logger
        )
    {
        _mediator = mediator;
        _logger = logger;
    }


    [HttpGet]
    [Route("search/{searchQuery}")]
    public async Task<ActionResult> SearchByUsername(string searchQuery)
    {
        var users = await _mediator.Send(new SearchUserByUsernameQuery(searchQuery));
        return users.Result;
    }
}
