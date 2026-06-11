namespace TradeOffStackAPI.Services;

using Microsoft.EntityFrameworkCore;
using TradeOffStackAPI.Data;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Services.Interfaces;

public class SaaSService : ISaaSService
{
    private readonly AssetDbContext _context;
    private readonly CoreDbContext _coreContext;

    public SaaSService(AssetDbContext context, CoreDbContext coreContext)
    {
        _context = context;
        _coreContext = coreContext;
    }

    public async Task<ServiceResponse<IEnumerable<SaaSProvider>>> GetProvidersAsync()
    {
        var providers = await _context.SaaSProviders.ToListAsync();
        return ServiceResponse<IEnumerable<SaaSProvider>>.Ok(providers);
    }

    public async Task<ServiceResponse<SaaSProvider>> CreateProviderAsync(SaaSProvider provider)
    {
        if (string.IsNullOrWhiteSpace(provider.Name))
            return ServiceResponse<SaaSProvider>.Fail("Provider name is required.");

        provider.Id = provider.Id == Guid.Empty ? Guid.NewGuid() : provider.Id;
        provider.CreatedAt = DateTime.UtcNow;
        _context.SaaSProviders.Add(provider);
        await _context.SaveChangesAsync();
        return ServiceResponse<SaaSProvider>.Ok(provider);
    }

    public async Task<ServiceResponse<SaaSProvider>> UpdateProviderAsync(Guid id, SaaSProvider provider)
    {
        var existing = await _context.SaaSProviders.FindAsync(id);
        if (existing == null)
            return ServiceResponse<SaaSProvider>.Fail("Provider not found.");

        if (string.IsNullOrWhiteSpace(provider.Name))
            return ServiceResponse<SaaSProvider>.Fail("Provider name is required.");

        existing.Name = provider.Name;
        existing.WebsiteUrl = provider.WebsiteUrl;
        existing.Category = provider.Category;
        existing.ContactEmail = provider.ContactEmail;

        await _context.SaveChangesAsync();
        return ServiceResponse<SaaSProvider>.Ok(existing);
    }

    public async Task<ServiceResponse> DeleteProviderAsync(Guid id)
    {
        var existing = await _context.SaaSProviders
            .Include(p => p.Subscriptions)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (existing == null)
            return ServiceResponse.Fail("Provider not found.");

        if (existing.Subscriptions.Count > 0)
            return ServiceResponse.Fail("Cannot delete a provider with active subscriptions.");

        _context.SaaSProviders.Remove(existing);
        await _context.SaveChangesAsync();
        return ServiceResponse.Ok("Provider deleted.");
    }

    public async Task<ServiceResponse<IEnumerable<SaaSSubscription>>> GetSubscriptionsAsync()
    {
        var subs = await _context.SaaSSubscriptions
            .Include(s => s.Provider)
            .ToListAsync();
        return ServiceResponse<IEnumerable<SaaSSubscription>>.Ok(subs);
    }

    public async Task<ServiceResponse<SaaSSubscription>> CreateSubscriptionAsync(SaaSSubscription subscription)
    {
        if (string.IsNullOrWhiteSpace(subscription.PlanName))
            return ServiceResponse<SaaSSubscription>.Fail("Plan name is required.");

        var provider = await _context.SaaSProviders.FindAsync(subscription.ProviderId);
        if (provider == null)
            return ServiceResponse<SaaSSubscription>.Fail("Provider not found.");

        subscription.Id = subscription.Id == Guid.Empty ? Guid.NewGuid() : subscription.Id;
        subscription.CreatedAt = DateTime.UtcNow;
        _context.SaaSSubscriptions.Add(subscription);
        await _context.SaveChangesAsync();
        subscription.Provider = provider;
        return ServiceResponse<SaaSSubscription>.Ok(subscription);
    }

    public async Task<ServiceResponse<SaaSSubscription>> UpdateSubscriptionAsync(Guid id, SaaSSubscription subscription)
    {
        var existing = await _context.SaaSSubscriptions.FindAsync(id);
        if (existing == null)
            return ServiceResponse<SaaSSubscription>.Fail("Subscription not found.");

        if (string.IsNullOrWhiteSpace(subscription.PlanName))
            return ServiceResponse<SaaSSubscription>.Fail("Plan name is required.");

        var provider = await _context.SaaSProviders.FindAsync(subscription.ProviderId);
        if (provider == null)
            return ServiceResponse<SaaSSubscription>.Fail("Provider not found.");

        var activeAssignments = await _context.SaaSAssignments
            .CountAsync(a => a.SubscriptionId == id && a.IsActive);

        if (subscription.TotalSeats < activeAssignments)
            return ServiceResponse<SaaSSubscription>.Fail(
                $"Cannot reduce seats below active assignments ({activeAssignments}).");

        existing.ProviderId = subscription.ProviderId;
        existing.PlanName = subscription.PlanName;
        existing.BillingCycle = subscription.BillingCycle;
        existing.CostPerSeat = subscription.CostPerSeat;
        existing.TotalSeats = subscription.TotalSeats;
        existing.RenewalDate = subscription.RenewalDate;
        existing.Status = subscription.Status;

        await _context.SaveChangesAsync();
        existing.Provider = provider;
        return ServiceResponse<SaaSSubscription>.Ok(existing);
    }

    public async Task<ServiceResponse> DeleteSubscriptionAsync(Guid id)
    {
        var existing = await _context.SaaSSubscriptions.FindAsync(id);
        if (existing == null)
            return ServiceResponse.Fail("Subscription not found.");

        var activeAssignments = await _context.SaaSAssignments
            .Where(a => a.SubscriptionId == id && a.IsActive)
            .ToListAsync();

        foreach (var assignment in activeAssignments)
            assignment.IsActive = false;

        _context.SaaSSubscriptions.Remove(existing);
        await _context.SaveChangesAsync();
        return ServiceResponse.Ok("Subscription deleted.");
    }

    public async Task<ServiceResponse<IEnumerable<SaaSAssignment>>> GetAllAssignmentsAsync()
    {
        var assignments = await _context.SaaSAssignments
            .Where(a => a.IsActive)
            .Include(a => a.Subscription)
            .ThenInclude(s => s!.Provider)
            .ToListAsync();

        await PopulateUsersAsync(assignments);
        return ServiceResponse<IEnumerable<SaaSAssignment>>.Ok(assignments);
    }

    public async Task<ServiceResponse<IEnumerable<SaaSAssignment>>> GetAssignmentsBySubscriptionAsync(Guid subscriptionId)
    {
        var assignments = await _context.SaaSAssignments
            .Where(a => a.SubscriptionId == subscriptionId && a.IsActive)
            .Include(a => a.Subscription)
            .ThenInclude(s => s!.Provider)
            .ToListAsync();

        await PopulateUsersAsync(assignments);
        return ServiceResponse<IEnumerable<SaaSAssignment>>.Ok(assignments);
    }

    public async Task<ServiceResponse<SaaSAssignment>> AssignUserAsync(SaaSAssignment assignment)
    {
        var subscription = await _context.SaaSSubscriptions.FindAsync(assignment.SubscriptionId);
        if (subscription == null)
            return ServiceResponse<SaaSAssignment>.Fail("Subscription not found.");

        if (subscription.Status != "Active")
            return ServiceResponse<SaaSAssignment>.Fail("Subscription is not active.");

        var user = await _coreContext.Users.FindAsync(assignment.UserId);
        if (user == null)
            return ServiceResponse<SaaSAssignment>.Fail("User not found.");

        var alreadyAssigned = await _context.SaaSAssignments
            .AnyAsync(a => a.SubscriptionId == assignment.SubscriptionId
                           && a.UserId == assignment.UserId
                           && a.IsActive);
        if (alreadyAssigned)
            return ServiceResponse<SaaSAssignment>.Fail("User is already assigned to this subscription.");

        var activeCount = await _context.SaaSAssignments
            .CountAsync(a => a.SubscriptionId == assignment.SubscriptionId && a.IsActive);
        if (activeCount >= subscription.TotalSeats)
            return ServiceResponse<SaaSAssignment>.Fail("No available seats for this subscription.");

        assignment.Id = assignment.Id == Guid.Empty ? Guid.NewGuid() : assignment.Id;
        assignment.AssignedDate = DateTime.UtcNow;
        assignment.IsActive = true;

        _context.SaaSAssignments.Add(assignment);
        await _context.SaveChangesAsync();

        assignment.User = user;
        assignment.Subscription = subscription;
        return ServiceResponse<SaaSAssignment>.Ok(assignment);
    }

    public async Task<ServiceResponse<bool>> UnassignUserAsync(Guid assignmentId)
    {
        var assignment = await _context.SaaSAssignments.FindAsync(assignmentId);
        if (assignment == null)
            return ServiceResponse<bool>.Fail("Assignment not found.");

        assignment.IsActive = false;
        await _context.SaveChangesAsync();
        return ServiceResponse<bool>.Ok(true);
    }

    private async Task PopulateUsersAsync(IEnumerable<SaaSAssignment> assignments)
    {
        foreach (var assignment in assignments)
            assignment.User = await _coreContext.Users.FindAsync(assignment.UserId);
    }
}
