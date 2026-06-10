using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOffStackAPI.Auth;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _service;

    public DepartmentController(IDepartmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAllAsync();
        return Ok(response.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _service.GetByIdAsync(id);
        return response.Success ? Ok(response.Data) : NotFound(new { message = response.Message });
    }

    [HttpPost]
    [Authorize(Roles = Roles.AdminOrTester)]
    public async Task<IActionResult> Create([FromBody] Department department)
    {
        var response = await _service.AddDepartmentAsync(department);
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response.Data)
            : BadRequest(new { message = response.Message });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.AdminOrTester)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Department department)
    {
        if (id != department.Id)
            return BadRequest("Object ID does not match route ID.");

        var response = await _service.UpdateDepartmentAsync(id, department);
        return response.Success ? NoContent() : NotFound(new { message = response.Message });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.AdminOrTester)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _service.DeleteDepartmentAsync(id);
        return response.Success ? NoContent() : NotFound(new { message = response.Message });
    }
}