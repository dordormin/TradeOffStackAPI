using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOffStackAPI.Auth;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Controllers;

/// <summary>
/// Gère les comptes utilisateurs et leurs permissions (Admin uniquement).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Admin)] // STRICTEMENT RÉSERVÉ À L'ADMIN
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
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

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var response = await _service.GetByEmailAsync(email);
        return response.Success ? Ok(response.Data) : NotFound(new { message = response.Message });
    }

    [HttpGet("department/{departmentId}")]
    public async Task<IActionResult> GetByDepartment(Guid departmentId)
    {
        var response = await _service.GetByDepartmentAsync(departmentId);
        return Ok(response.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        var response = await _service.AddUserAsync(user);
        if (!response.Success)
        {
            // On pourrait utiliser un switch sur le message pour retourner 409 (Conflict) ou 400 (Bad Request)
            return Conflict(new { message = response.Message });
        }
        return CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] User user)
    {
        if (id != user.Id) 
            return BadRequest("Object ID does not match route ID.");
        
        var response = await _service.UpdateUserAsync(id, user);
        return response.Success ? NoContent() : NotFound(new { message = response.Message });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _service.DeleteUserAsync(id);
        return response.Success ? NoContent() : NotFound(new { message = response.Message });
    }
}