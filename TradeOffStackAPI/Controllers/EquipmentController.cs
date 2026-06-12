using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOffStackAPI.Auth;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService _service;

    public EquipmentController(IEquipmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetInventoryAsync();
        return Ok(response.Data);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(AssetStatus status)
    {
        var response = await _service.GetByStatusAsync(status);
        return Ok(response.Data);
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(AssetCategory category)
    {
        var response = await _service.GetByCategoryAsync(category);
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
    public async Task<IActionResult> Create([FromBody] Equipment equipment)
    {
        var response = await _service.AddEquipmentAsync(equipment);
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response.Data)
            : BadRequest(new { message = response.Message });
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = Roles.AdminOrManagerOrTester)]
    public async Task<IActionResult> Update(Guid id, [FromBody] Equipment equipment)
    {
        if (id != equipment.Id)
            return BadRequest("Object ID does not match route ID.");

        var response = await _service.UpdateEquipmentAsync(id, equipment);
        return response.Success ? Ok(new { message = "Success" }) : NotFound(new { message = response.Message });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.AdminOrTester)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _service.DeleteEquipmentAsync(id);
        return response.Success ? Ok(new { message = "Success" }) : NotFound(new { message = response.Message });
    }

    [HttpPost("{id}/licenses/{licenseId}")]
    [Authorize(Roles = Roles.AdminOrManagerOrTester)]
    public async Task<IActionResult> AssignLicense(Guid id, Guid licenseId)
    {
        var response = await _service.AssignLicenseAsync(id, licenseId);
        return response.Success ? Ok(new { message = response.Message }) : BadRequest(new { message = response.Message });
    }

    [HttpDelete("{id}/licenses/{licenseId}")]
    [Authorize(Roles = Roles.AdminOrManagerOrTester)]
    public async Task<IActionResult> RevokeLicense(Guid id, Guid licenseId)
    {
        var response = await _service.RevokeLicenseAsync(id, licenseId);
        return response.Success ? Ok(new { message = "Success" }) : BadRequest(new { message = response.Message });
    }
}