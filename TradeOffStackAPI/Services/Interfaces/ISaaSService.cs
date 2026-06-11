namespace TradeOffStackAPI.Services.Interfaces;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Dtos;

public interface ISaaSService
{
    Task<ServiceResponse<IEnumerable<SaaSProvider>>> GetProvidersAsync();
    Task<ServiceResponse<SaaSProvider>> CreateProviderAsync(SaaSProvider provider);
    Task<ServiceResponse<SaaSProvider>> UpdateProviderAsync(Guid id, SaaSProvider provider);
    Task<ServiceResponse> DeleteProviderAsync(Guid id);

    Task<ServiceResponse<IEnumerable<SaaSSubscription>>> GetSubscriptionsAsync();
    Task<ServiceResponse<SaaSSubscription>> CreateSubscriptionAsync(SaaSSubscription subscription);
    Task<ServiceResponse<SaaSSubscription>> UpdateSubscriptionAsync(Guid id, SaaSSubscription subscription);
    Task<ServiceResponse> DeleteSubscriptionAsync(Guid id);

    Task<ServiceResponse<IEnumerable<SaaSAssignment>>> GetAllAssignmentsAsync();
    Task<ServiceResponse<IEnumerable<SaaSAssignment>>> GetAssignmentsBySubscriptionAsync(Guid subscriptionId);
    Task<ServiceResponse<SaaSAssignment>> AssignUserAsync(SaaSAssignment assignment);
    Task<ServiceResponse<bool>> UnassignUserAsync(Guid assignmentId);
}
