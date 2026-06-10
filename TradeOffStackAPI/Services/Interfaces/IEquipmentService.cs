using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Services.Interfaces;

/// <summary>
/// Service interface for managing equipment inventory.
/// </summary>
public interface IEquipmentService
{
    /// <summary>Retrieves the full equipment inventory.</summary>
    Task<ServiceResponse<IEnumerable<Equipment>>> GetInventoryAsync();
    
    /// <summary>Retrieves a specific piece of equipment by its ID.</summary>
    Task<ServiceResponse<Equipment>> GetByIdAsync(Guid id);
    
    /// <summary>Retrieves all equipment matching a specific status.</summary>
    Task<ServiceResponse<IEnumerable<Equipment>>> GetByStatusAsync(AssetStatus status);
    
    /// <summary>Retrieves all equipment belonging to a specific category.</summary>
    Task<ServiceResponse<IEnumerable<Equipment>>> GetByCategoryAsync(AssetCategory category);
    
    /// <summary>Registers a new piece of equipment into the inventory.</summary>
    Task<ServiceResponse<Equipment>> AddEquipmentAsync(Equipment equipment);
    
    /// <summary>Updates the details of an existing piece of equipment.</summary>
    Task<ServiceResponse<Equipment>> UpdateEquipmentAsync(Guid id, Equipment equipment);
    
    /// <summary>Removes a piece of equipment from the inventory.</summary>
    Task<ServiceResponse> DeleteEquipmentAsync(Guid id);
    
    /// <summary>Assigns a software license to the equipment.</summary>
    Task<ServiceResponse> AssignLicenseAsync(Guid equipmentId, Guid softwareLicenseId);
    
    /// <summary>Revokes a software license from the equipment.</summary>
    Task<ServiceResponse> RevokeLicenseAsync(Guid equipmentId, Guid softwareLicenseId);
}