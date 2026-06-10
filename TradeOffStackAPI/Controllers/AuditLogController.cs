using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOffStackAPI.Auth;
using TradeOffStackAPI.Services.Interfaces;
using System.Security.Claims;
using System.Linq;
using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Controllers;

/// <summary>
/// Permet la consultation des pistes d'audit (Logs de modification).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.AdminOrTester)] // Piste d'audit : consultation admin et testeurs restreints.
public class AuditLogController : ControllerBase
{
    private readonly IAuditLogService _service;

    public AuditLogController(IAuditLogService service)
    {
        _service = service;
    }

    /// <summary>
    /// Liste tous les journaux d'audit enregistrés.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var logs = await _service.GetAllAsync();
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role");



        return Ok(logs);
    }

    /// <summary>
    /// Récupère l'historique des modifications pour une entité spécifique.
    /// </summary>
    [HttpGet("{entityType}/{entityId}")]
    public async Task<IActionResult> GetByEntity(string entityType, Guid entityId)
    {
        var logs = await _service.GetByEntityAsync(entityType, entityId);
        var currentUserRole = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role");



        return Ok(logs);
    }
}
