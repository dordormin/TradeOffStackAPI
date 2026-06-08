using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOffStackAPI.Auth;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MaintenanceRequestController : ControllerBase
{
    private readonly IMaintenanceRequestService _service;

    public MaintenanceRequestController(IMaintenanceRequestService service)
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

    [HttpGet("equipment/{equipmentId}")]
    public async Task<IActionResult> GetByEquipment(Guid equipmentId)
    {
        var response = await _service.GetByEquipmentAsync(equipmentId);
        return Ok(response.Data);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(MaintenanceStatus status)
    {
        var response = await _service.GetByStatusAsync(status);
        return Ok(response.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MaintenanceRequest request)
    {
        var response = await _service.CreateRequestAsync(request);
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response.Data)
            : Conflict(new { message = response.Message });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] MaintenanceRequest request)
    {
        if (id != request.Id)
            return BadRequest("Object ID does not match route ID.");

        var response = await _service.UpdateRequestAsync(id, request);
        return response.Success ? NoContent() : NotFound(new { message = response.Message });
    }

    [HttpPost("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteMaintenanceDto? body)
    {
        var response = await _service.CompleteRequestAsync(id, body?.TechnicianNotes);
        return response.Success ? NoContent() : NotFound(new { message = response.Message });
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var response = await _service.CancelRequestAsync(id);
        return response.Success ? NoContent() : NotFound(new { message = response.Message });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.AdminOrManager)]
    public async Task<IActionResult> Delete(Guid id)
    {
        // Note: La logique métier de suppression n'est pas implémentée dans le service,
        // car annuler est généralement préférable. On pourrait l'ajouter si nécessaire.
        var response = await _service.CancelRequestAsync(id);
        return response.Success ? NoContent() : NotFound(new { message = response.Message });
    }
}

public record CompleteMaintenanceDto(string? TechnicianNotes);