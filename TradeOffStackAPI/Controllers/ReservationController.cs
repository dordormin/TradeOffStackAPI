using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _service;
    private readonly ICurrentUserService _currentUser;

    public ReservationController(IReservationService service, ICurrentUserService currentUser)
    {
        _service = service;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (_currentUser.IsInRole(UserRole.Admin) || _currentUser.IsInRole(UserRole.Manager))
        {
            var response = await _service.GetAllAsync();
            return Ok(response.Data);
        }

        if (_currentUser.DepartmentId is { } departmentId)
        {
            var response = await _service.GetByDepartmentAsync(departmentId);
            return Ok(response.Data);
        }

        if (_currentUser.UserId is { } userId)
        {
            var response = await _service.GetByUserAsync(userId);
            return Ok(response.Data);
        }
        
        return Forbid();
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

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(Guid userId)
    {
        var response = await _service.GetByUserAsync(userId);
        return Ok(response.Data);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var response = await _service.GetActiveAsync();
        return Ok(response.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Reservation reservation)
    {
        var response = await _service.CreateReservationAsync(reservation);
        return response.Success
            ? CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response.Data)
            : Conflict(new { message = response.Message }); // 409 Conflict est plus approprié pour les règles métier
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Reservation reservation)
    {
        if (id != reservation.Id)
            return BadRequest("Object ID does not match route ID.");

        var response = await _service.UpdateReservationAsync(id, reservation);
        return response.Success ? NoContent() : NotFound(new { message = response.Message });
    }

    [HttpPost("{id}/return")]
    public async Task<IActionResult> Return(Guid id)
    {
        var response = await _service.ReturnEquipmentAsync(id);
        return response.Success ? NoContent() : NotFound(new { message = response.Message });
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var response = await _service.CancelReservationAsync(id);
        return response.Success ? NoContent() : NotFound(new { message = response.Message });
    }
}