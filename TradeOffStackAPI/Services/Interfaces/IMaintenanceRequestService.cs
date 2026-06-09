using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Services.Interfaces;

public interface IMaintenanceRequestService
{
    Task<ServiceResponse<IEnumerable<MaintenanceRequest>>> GetAllAsync();
    Task<ServiceResponse<MaintenanceRequest>> GetByIdAsync(Guid id);
    Task<ServiceResponse<IEnumerable<MaintenanceRequest>>> GetByEquipmentAsync(Guid equipmentId);
    Task<ServiceResponse<IEnumerable<MaintenanceRequest>>> GetByStatusAsync(MaintenanceStatus status);
    Task<ServiceResponse<MaintenanceRequest>> CreateRequestAsync(MaintenanceRequest request);
    Task<ServiceResponse<MaintenanceRequest>> UpdateRequestAsync(Guid id, MaintenanceRequest request);
    Task<ServiceResponse> CompleteRequestAsync(Guid id, string? technicianNotes);
    Task<ServiceResponse> CancelRequestAsync(Guid id);
    Task<ServiceResponse> DeleteRequestAsync(Guid id);
}