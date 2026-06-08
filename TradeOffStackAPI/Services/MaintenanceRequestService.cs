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
    private readonly TradeOffStackAPI.Data.AppDbContext _context;

    public MaintenanceRequestService(
        IMaintenanceRequestRepository repo, 
        IEquipmentRepository equipmentRepo,
        TradeOffStackAPI.Data.AppDbContext context)
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
        await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        try
        {
            var equipment = await _equipmentRepo.GetByIdAsync(request.EquipmentId);
            if (equipment == null)
                return ServiceResponse<MaintenanceRequest>.Fail("The specified equipment does not exist.");

            if (await _repo.HasOpenRequestAsync(request.EquipmentId))
                return ServiceResponse<MaintenanceRequest>.Fail("This equipment already has an open maintenance request.");

            equipment.Status = AssetStatus.OutForRepair;
            await _equipmentRepo.UpdateAsync(equipment);

            var success = await _repo.AddAsync(request);
            
            await transaction.CommitAsync();
            return success
                ? ServiceResponse<MaintenanceRequest>.Ok(request, "Request created.")
                : ServiceResponse<MaintenanceRequest>.Fail("Failed to create the request.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ServiceResponse<MaintenanceRequest>.Fail($"Transaction error: {ex.Message}");
        }
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<MaintenanceRequest>> UpdateRequestAsync(Guid id, MaintenanceRequest request)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            return ServiceResponse<MaintenanceRequest>.Fail("Request not found.");

        existing.Status = request.Status;
        existing.Priority = request.Priority;
        existing.Description = request.Description;
        existing.ScheduledDate = request.ScheduledDate;

        var success = await _repo.UpdateAsync(existing);
        return success
            ? ServiceResponse<MaintenanceRequest>.Ok(existing, "Request updated.")
            : ServiceResponse<MaintenanceRequest>.Fail("Failed to update.");
    }

    /// <inheritdoc />

    public async Task<ServiceResponse> CompleteRequestAsync(Guid id, string? technicianNotes)
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

            var equipment = await _equipmentRepo.GetByIdAsync(request.EquipmentId);
            if (equipment != null)
            {
                equipment.Status = AssetStatus.Available;
                await _equipmentRepo.UpdateAsync(equipment);
            }

            await transaction.CommitAsync();
            return ServiceResponse.Ok("Request completed.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ServiceResponse.Fail($"Transaction error: {ex.Message}");
        }
    }

    /// <inheritdoc />

    public async Task<ServiceResponse> CancelRequestAsync(Guid id)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        try
        {
            var request = await _repo.GetByIdAsync(id);
            if (request == null)
                return ServiceResponse.Fail("Request not found.");

            request.Status = MaintenanceStatus.Cancelled;
            await _repo.UpdateAsync(request);

            var equipment = await _equipmentRepo.GetByIdAsync(request.EquipmentId);
            if (equipment != null && equipment.Status == AssetStatus.OutForRepair)
            {
                equipment.Status = AssetStatus.Available;
                await _equipmentRepo.UpdateAsync(equipment);
            }

            await transaction.CommitAsync();
            return ServiceResponse.Ok("Request cancelled.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ServiceResponse.Fail($"Transaction error: {ex.Message}");
        }
    }
}