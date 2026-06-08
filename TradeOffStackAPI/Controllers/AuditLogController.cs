using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOffStackAPI.Auth;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Controllers;

/// <summary>
/// Permet la consultation des pistes d'audit (Logs de modification).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Admin)] // Piste d'audit : consultation administrateur uniquement.
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
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    /// <summary>
    /// Récupère l'historique des modifications pour une entité spécifique.
    /// </summary>
    [HttpGet("{entityType}/{entityId}")]
    public async Task<IActionResult> GetByEntity(string entityType, Guid entityId) =>
        Ok(await _service.GetByEntityAsync(entityType, entityId));
}
