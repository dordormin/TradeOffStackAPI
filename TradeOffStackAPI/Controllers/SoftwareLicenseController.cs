using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOffStackAPI.Auth;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SoftwareLicenseController : ControllerBase
{
    private readonly ISoftwareLicenseService _service;

    public SoftwareLicenseController(ISoftwareLicenseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAllAsync();
        return Ok(response.Data);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var response = await _service.GetActiveLicensesAsync();
        return Ok(response.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _service.GetByIdAsync(id);
        return response.Success ? Ok(response.Data) : NotFound(new { message = response.Message });
    }

    [HttpPost]
    [Authorize(Roles = Roles.AdminOrManagerOrTester)]
    public async Task<IActionResult> Create([FromBody] SoftwareLicense license)
    {
        var response = await _service.AddAsync(license);
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response.Data)
            : BadRequest(new { message = response.Message });
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.AdminOrManagerOrTester)]
    public async Task<IActionResult> Update(Guid id, [FromBody] SoftwareLicense license)
    {
        if (id != license.Id)
            return BadRequest("Object ID does not match route ID.");

        var response = await _service.UpdateAsync(id, license);
        return response.Success ? (IActionResult)Ok(new { message = "Success" }) : NotFound(new { message = response.Message });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.AdminOrTester)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _service.DeleteAsync(id);
        return response.Success ? (IActionResult)Ok(new { message = "Success" }) : NotFound(new { message = response.Message });
    }
}
