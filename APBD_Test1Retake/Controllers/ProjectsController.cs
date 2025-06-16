using Microsoft.AspNetCore.Mvc;
using APBD_Test1Retake.Models.DTOs;
using APBD_Test1Retake.Services;

namespace APBD_Test1Retake.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectsController(IProjectService service)
    {
        _service = service;
    }

    // get  api projects 201
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var result = await _service.GetProjectByIdAsync(id);
        if (result == null)
            return NotFound($"Project with thid ID {id} not found.");
        return Ok(result);
    }
}