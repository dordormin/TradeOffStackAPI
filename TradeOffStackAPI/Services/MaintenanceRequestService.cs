using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TradeOffStackAPI.Services;

public class MaintenanceRequestService : IMaintenanceRequestService
{
    private readonly IMaintenanceRequestRepository _repo;
    private readonly IEquipmentRepository _equipmentRepo;
    private readonly TradeOffStackAPI.Data.AssetDbContext _context;

    public MaintenanceRequestService(
        IMaintenanceRequestRepository repo, 
        IEquipmentRepository equipmentRepo,
        TradeOffStackAPI.Data.AssetDbContext context)
    {
        _repo = repo;
        _equipmentRepo = equipmentRepo;
        _context = context;
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<IEnumerable<MaintenanceRequest>>> GetAllAsync()
    {
        var requests = await _repo.GetAllAsync();
        return ServiceResponse<IEnumerable<MaintenanceRequest>>.Ok(requests);
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<MaintenanceRequest>> GetByIdAsync(Guid id)
    {
        var request = await _repo.GetByIdAsync(id);
        return request != null
            ? ServiceResponse<MaintenanceRequest>.Ok(request)
            : ServiceResponse<MaintenanceRequest>.Fail("Maintenance request not found.");
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<IEnumerable<MaintenanceRequest>>> GetByEquipmentAsync(Guid equipmentId)
    {
        var requests = await _repo.GetByEquipmentAsync(equipmentId);
        return ServiceResponse<IEnumerable<MaintenanceRequest>>.Ok(requests);
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<IEnumerable<MaintenanceRequest>>> GetByStatusAsync(MaintenanceStatus status)
    {
        var requests = await _repo.GetByStatusAsync(status);
        return ServiceResponse<IEnumerable<MaintenanceRequest>>.Ok(requests);
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<MaintenanceRequest>> CreateRequestAsync(MaintenanceRequest request)
    {
        try
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
                try
                {
                    var equipment = await _equipmentRepo.GetByIdAsync(request.EquipmentId);
                    if (equipment == null)
                        return ServiceResponse<MaintenanceRequest>.Fail("The specified equipment does not exist.");

                    if (await _repo.HasOpenRequestAsync(request.EquipmentId))
                        return ServiceResponse<MaintenanceRequest>.Fail("This equipment already has an open maintenance request.");

                    var success = await _repo.AddAsync(request);
                    if (!success)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResponse<MaintenanceRequest>.Fail("Failed to create the request.");
                    }

                    await UpdateEquipmentStatusAsync(request.EquipmentId);
                    
                    await transaction.CommitAsync();
                    return ServiceResponse<MaintenanceRequest>.Ok(request, "Request created.");
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
        catch (Exception ex)
        {
            return ServiceResponse<MaintenanceRequest>.Fail($"Transaction error: {ex.Message}");
        }
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<MaintenanceRequest>> UpdateRequestAsync(Guid id, MaintenanceRequest request)
    {
        try
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
                try
                {
                    var existing = await _repo.GetByIdAsync(id);
                    if (existing == null)
                        return ServiceResponse<MaintenanceRequest>.Fail("Request not found.");

                    var oldEquipmentId = existing.EquipmentId;
                    var newEquipmentId = request.EquipmentId;

                    existing.Status = request.Status;
                    existing.Priority = request.Priority;
                    existing.Description = request.Description;
                    existing.ScheduledDate = request.ScheduledDate;
                    existing.EquipmentId = request.EquipmentId;

                    var success = await _repo.UpdateAsync(existing);
                    if (!success)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResponse<MaintenanceRequest>.Fail("Failed to update request.");
                    }

                    await UpdateEquipmentStatusAsync(oldEquipmentId);
                    if (oldEquipmentId != newEquipmentId)
                    {
                        await UpdateEquipmentStatusAsync(newEquipmentId);
                    }

                    await transaction.CommitAsync();
                    return ServiceResponse<MaintenanceRequest>.Ok(existing, "Request updated.");
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
        catch (Exception ex)
        {
            return ServiceResponse<MaintenanceRequest>.Fail($"An error occurred: {ex.Message}");
        }
    }

    /// <inheritdoc />

    public async Task<ServiceResponse> CompleteRequestAsync(Guid id, string? technicianNotes)
    {
        try
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
                try
                {
                    var request = await _repo.GetByIdAsync(id);
                    if (request == null)
                        return ServiceResponse.Fail("Request not found.");

                    request.Status = MaintenanceStatus.Completed;
                    request.CompletedDate = DateTime.UtcNow;
                    request.TechnicianNotes = technicianNotes;
                    await _repo.UpdateAsync(request);

                    await UpdateEquipmentStatusAsync(request.EquipmentId);

                    await transaction.CommitAsync();
                    return ServiceResponse.Ok("Request completed.");
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail($"Transaction error: {ex.Message}");
        }
    }

    /// <inheritdoc />

    public async Task<ServiceResponse> CancelRequestAsync(Guid id)
    {
        try
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
                try
                {
                    var request = await _repo.GetByIdAsync(id);
                    if (request == null)
                        return ServiceResponse.Fail("Request not found.");

                    request.Status = MaintenanceStatus.Cancelled;
                    await _repo.UpdateAsync(request);

                    await UpdateEquipmentStatusAsync(request.EquipmentId);

                    await transaction.CommitAsync();
                    return ServiceResponse.Ok("Request cancelled.");
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
        catch (Exception ex)
        {
            return ServiceResponse.Fail($"Transaction error: {ex.Message}");
        }
    }

    private async Task UpdateEquipmentStatusAsync(Guid equipmentId)
    {
        var equipment = await _equipmentRepo.GetByIdAsync(equipmentId);
        if (equipment == null) return;

        // In app context context is injected
        var hasOpenMaintenance = await _context.MaintenanceRequests
            .AnyAsync(r => r.EquipmentId == equipmentId && 
                           (r.Status == MaintenanceStatus.Pending || r.Status == MaintenanceStatus.InProgress));
                           
        if (hasOpenMaintenance)
        {
            equipment.Status = AssetStatus.OutForRepair;
        }
        else
        {
            var hasActiveReservation = await _context.Reservations
                .AnyAsync(r => r.EquipmentId == equipmentId && 
                               (r.Status == ReservationStatus.Active || r.Status == ReservationStatus.Pending));
                               
            if (hasActiveReservation)
            {
                equipment.Status = AssetStatus.Reserved;
            }
            else
            {
                equipment.Status = AssetStatus.Available;
            }
        }
        
        await _equipmentRepo.UpdateAsync(equipment);
    }

    public async Task<ServiceResponse> DeleteRequestAsync(Guid id)
    {
        var request = await _repo.GetByIdAsync(id);
        if (request == null)
            return ServiceResponse.Fail("Request not found.");

        var equipmentId = request.EquipmentId;
        var success = await _repo.DeleteAsync(id);
        if (success)
        {
            await UpdateEquipmentStatusAsync(equipmentId);
            return ServiceResponse.Ok("Request successfully deleted.");
        }
        return ServiceResponse.Fail("Failed to delete request.");
    }
}