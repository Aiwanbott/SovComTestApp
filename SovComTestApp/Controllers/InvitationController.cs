using Microsoft.AspNetCore.Mvc;
using SovComTestApp.Models;
using SovComTestApp.Interfaces;
using SovComTestApp.Services;

namespace SovComTestApp.Controllers;

[Route("api/invite")]
[ApiController]
public class InvitationController : Controller
{

    private readonly IInvitationService _invitationService;

    public InvitationController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }
    [HttpPost("send")]
    public IActionResult SendInvite([FromBody] Invites invites)
    {
        var response = _invitationService.SendInvite(invites);

        if (response.Code.StartsWith("4"))
        {
            return BadRequest(response);
        }

        if (response.Code.StartsWith("5"))
        {
            return StatusCode(500, response);
        }

        return Ok();
    }
}

