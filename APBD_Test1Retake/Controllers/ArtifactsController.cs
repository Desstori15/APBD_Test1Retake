using Microsoft.AspNetCore.Mvc;
using APBD_Test1Retake.Models.DTOs;
using APBD_Test1Retake.Services;

namespace APBD_Test1Retake.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtifactsController : ControllerBase
{
    private readonly IProjectService _service;

    public ArtifactsController(IProjectService service)
    {
        _service = service;
    }

    // post  api artifacts
    [HttpPost]
    public async Task<IActionResult> AddArtifactWithProject([FromBody] AddArtifactRequest request)
    {
        var success = await _service.AddArtifactWithProjectAsync(request);
        if (!success)
            return BadRequest("bad artifact or project data ");
        return Created("", null);
    }
}