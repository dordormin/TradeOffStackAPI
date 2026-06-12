namespace TradeOffStackAPI.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOffStackAPI.Auth;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SaaSController : ControllerBase
{
    private readonly ISaaSService _saasService;

    public SaaSController(ISaaSService saasService)
    {
        _saasService = saasService;
    }

    [HttpGet("providers")]
    public async Task<IActionResult> GetProviders()
    {
        var response = await _saasService.GetProvidersAsync();
        return Ok(response.Data);
    }

    [HttpPost("providers")]
    [Authorize(Roles = Roles.AdminOrManagerOrTester)]
    public async Task<IActionResult> CreateProvider([FromBody] SaaSProvider provider)
    {
        var response = await _saasService.CreateProviderAsync(provider);
        if (!response.Success) return BadRequest(new { message = response.Message });
        return Ok(response.Data);
    }

    [HttpPut("providers/{id}")]
    [Authorize(Roles = Roles.AdminOrManagerOrTester)]
    public async Task<IActionResult> UpdateProvider(Guid id, [FromBody] SaaSProvider provider)
    {
        var response = await _saasService.UpdateProviderAsync(id, provider);
        if (!response.Success) return NotFound(new { message = response.Message });
        return Ok(response.Data);
    }

    [HttpDelete("providers/{id}")]
    [Authorize(Roles = Roles.AdminOrTester)]
    public async Task<IActionResult> DeleteProvider(Guid id)
    {
        var response = await _saasService.DeleteProviderAsync(id);
        if (!response.Success) return BadRequest(new { message = response.Message });
        return Ok(new { message = "Success" });
    }

    [HttpGet("subscriptions")]
    public async Task<IActionResult> GetSubscriptions()
    {
        var response = await _saasService.GetSubscriptionsAsync();
        return Ok(response.Data);
    }

    [HttpPost("subscriptions")]
    [Authorize(Roles = Roles.AdminOrManagerOrTester)]
    public async Task<IActionResult> CreateSubscription([FromBody] SaaSSubscription subscription)
    {
        var response = await _saasService.CreateSubscriptionAsync(subscription);
        if (!response.Success) return BadRequest(new { message = response.Message });
        return Ok(response.Data);
    }

    [HttpPut("subscriptions/{id}")]
    [Authorize(Roles = Roles.AdminOrManagerOrTester)]
    public async Task<IActionResult> UpdateSubscription(Guid id, [FromBody] SaaSSubscription subscription)
    {
        if (id != subscription.Id)
            return BadRequest("Object ID does not match route ID.");

        var response = await _saasService.UpdateSubscriptionAsync(id, subscription);
        if (!response.Success) return BadRequest(new { message = response.Message });
        return Ok(response.Data);
    }

    [HttpDelete("subscriptions/{id}")]
    [Authorize(Roles = Roles.AdminOrTester)]
    public async Task<IActionResult> DeleteSubscription(Guid id)
    {
        var response = await _saasService.DeleteSubscriptionAsync(id);
        if (!response.Success) return NotFound(new { message = response.Message });
        return Ok(new { message = "Success" });
    }

    [HttpGet("assignments")]
    public async Task<IActionResult> GetAllAssignments()
    {
        var response = await _saasService.GetAllAssignmentsAsync();
        return Ok(response.Data);
    }

    [HttpGet("subscriptions/{id}/assignments")]
    public async Task<IActionResult> GetAssignments(Guid id)
    {
        var response = await _saasService.GetAssignmentsBySubscriptionAsync(id);
        return Ok(response.Data);
    }

    [HttpPost("assignments")]
    [Authorize(Roles = Roles.AdminOrManagerOrTester)]
    public async Task<IActionResult> AssignUser([FromBody] SaaSAssignment assignment)
    {
        var response = await _saasService.AssignUserAsync(assignment);
        if (!response.Success) return BadRequest(new { message = response.Message });
        return Ok(response.Data);
    }

    [HttpDelete("assignments/{id}")]
    [Authorize(Roles = Roles.AdminOrManagerOrTester)]
    public async Task<IActionResult> UnassignUser(Guid id)
    {
        var response = await _saasService.UnassignUserAsync(id);
        if (!response.Success) return NotFound(new { message = response.Message });
        return Ok(new { message = "Success" });
    }
}
