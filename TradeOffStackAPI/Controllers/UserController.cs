using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOffStackAPI.Auth;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace TradeOffStackAPI.Controllers;

/// <summary>
/// <summary>
/// Gère les comptes utilisateurs et leurs permissions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Accessible à tous les utilisateurs authentifiés
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAllAsync();
        return Ok(response.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role");

        if (currentUserRole != Roles.Admin && currentUserId != id.ToString())
        {
            return Forbid();
        }

        var response = await _service.GetByIdAsync(id);
        return response.Success ? Ok(response.Data) : NotFound(new { message = response.Message });
    }

    [HttpGet("email/{email}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var response = await _service.GetByEmailAsync(email);
        return response.Success ? Ok(response.Data) : NotFound(new { message = response.Message });
    }

    [HttpGet("department/{departmentId}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetByDepartment(Guid departmentId)
    {
        var response = await _service.GetByDepartmentAsync(departmentId);
        return Ok(response.Data);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        var response = await _service.AddUserAsync(user);
        if (!response.Success)
        {
            return Conflict(new { message = response.Message });
        }
        return CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] User user)
    {
        if (id != user.Id) 
            return BadRequest("Object ID does not match route ID.");

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role");

        if (currentUserRole != Roles.Admin && currentUserId != id.ToString())
        {
            return Forbid();
        }

        // Si ce n'est pas un administrateur, on préserve les champs sensibles pour éviter l'élévation de privilèges
        if (currentUserRole != Roles.Admin)
        {
            var existingUserResponse = await _service.GetByIdAsync(id);
            if (!existingUserResponse.Success || existingUserResponse.Data == null)
                return NotFound(new { message = "User not found." });

            var existingUser = existingUserResponse.Data;
            user.Role = existingUser.Role;
            user.DepartmentId = existingUser.DepartmentId;
            user.IsActive = existingUser.IsActive;
            user.Email = existingUser.Email; // Empêche de changer d'email pour éviter les détournements
        }
        
        var response = await _service.UpdateUserAsync(id, user);
        return response.Success ? Ok(new { message = "Success" }) : NotFound(new { message = response.Message });
    }

    [HttpPut("{id}/change-password")]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role");

        if (currentUserRole != Roles.Admin && currentUserId != id.ToString())
        {
            return Forbid();
        }

        var userResponse = await _service.GetByIdAsync(id);
        if (!userResponse.Success || userResponse.Data == null)
            return NotFound(new { message = "User not found." });

        var user = userResponse.Data;
        
        // Si l'utilisateur n'est pas admin, il doit fournir son mot de passe actuel correct
        if (currentUserRole != Roles.Admin)
        {
            if (string.IsNullOrEmpty(request.OldPassword) || !BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
            {
                return BadRequest(new { message = "Incorrect current password." });
            }
        }

        if (string.IsNullOrEmpty(request.NewPassword))
            return BadRequest(new { message = "New password cannot be empty." });

        var response = await _service.UpdatePasswordAsync(id, request.NewPassword);
        return response.Success ? Ok(new { message = "Password updated successfully." }) : BadRequest(new { message = response.Message });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _service.DeleteUserAsync(id);
        return response.Success ? Ok(new { message = "Success" }) : NotFound(new { message = response.Message });
    }
}

public class ChangePasswordRequest
{
    [JsonPropertyName("old_password")]
    public string OldPassword { get; set; } = string.Empty;

    [JsonPropertyName("new_password")]
    public string NewPassword { get; set; } = string.Empty;
}