using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using TradeOffStackAPI.Data;

namespace TradeOffStackAPI.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _repo;
    private readonly IEquipmentRepository _equipmentRepo;
    private readonly AppDbContext _context;

    public ReservationService(
        IReservationRepository repo, 
        IEquipmentRepository equipmentRepo,
        AppDbContext context)
    {
        _repo = repo;
        _equipmentRepo = equipmentRepo;
        _context = context;
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<IEnumerable<Reservation>>> GetAllAsync()
    {
        var reservations = await _repo.GetAllAsync();
        return ServiceResponse<IEnumerable<Reservation>>.Ok(reservations);
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<Reservation>> GetByIdAsync(Guid id)
    {
        var reservation = await _repo.GetByIdAsync(id);
        return reservation != null
            ? ServiceResponse<Reservation>.Ok(reservation)
            : ServiceResponse<Reservation>.Fail("Reservation not found.");
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<IEnumerable<Reservation>>> GetByEquipmentAsync(Guid equipmentId)
    {
        var reservations = await _repo.GetByEquipmentAsync(equipmentId);
        return ServiceResponse<IEnumerable<Reservation>>.Ok(reservations);
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<IEnumerable<Reservation>>> GetByUserAsync(Guid userId)
    {
        var reservations = await _repo.GetByUserAsync(userId);
        return ServiceResponse<IEnumerable<Reservation>>.Ok(reservations);
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<IEnumerable<Reservation>>> GetByDepartmentAsync(Guid departmentId)
    {
        var reservations = await _repo.GetByDepartmentAsync(departmentId);
        return ServiceResponse<IEnumerable<Reservation>>.Ok(reservations);
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<IEnumerable<Reservation>>> GetActiveAsync()
    {
        var reservations = await _repo.GetActiveAsync();
        return ServiceResponse<IEnumerable<Reservation>>.Ok(reservations);
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<Reservation>> CreateReservationAsync(Reservation reservation)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        try
        {
            var equipment = await _equipmentRepo.GetByIdAsync(reservation.EquipmentId);
            if (equipment == null)
                return ServiceResponse<Reservation>.Fail("The specified equipment does not exist.");
            
            if (equipment.Status != AssetStatus.Available)
                return ServiceResponse<Reservation>.Fail($"The equipment is not available. Current status: {equipment.Status}.");
            
            if (await _repo.HasActiveReservationAsync(reservation.EquipmentId))
                return ServiceResponse<Reservation>.Fail("This equipment already has an active reservation.");

            equipment.Status = AssetStatus.Reserved;
            await _equipmentRepo.UpdateAsync(equipment);

            var success = await _repo.AddAsync(reservation);
            
            await transaction.CommitAsync();
            
            return success
                ? ServiceResponse<Reservation>.Ok(reservation, "Reservation created.")
                : ServiceResponse<Reservation>.Fail("Failed to save the reservation.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ServiceResponse<Reservation>.Fail($"An error occurred: {ex.Message}");
        }
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<Reservation>> UpdateReservationAsync(Guid id, Reservation reservation)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null)
            return ServiceResponse<Reservation>.Fail("Reservation not found.");

        existing.Status = reservation.Status;
        existing.StartDate = reservation.StartDate;
        existing.EndDate = reservation.EndDate;
        existing.Notes = reservation.Notes;

        var success = await _repo.UpdateAsync(existing);
        return success
            ? ServiceResponse<Reservation>.Ok(existing, "Reservation updated.")
            : ServiceResponse<Reservation>.Fail("Failed to update.");
    }

    /// <inheritdoc />

    public async Task<ServiceResponse> ReturnEquipmentAsync(Guid reservationId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        try
        {
            var reservation = await _repo.GetByIdAsync(reservationId);
            if (reservation == null)
                return ServiceResponse.Fail("Reservation not found.");

            reservation.Status = ReservationStatus.Returned;
            reservation.ReturnDate = DateTime.UtcNow;
            await _repo.UpdateAsync(reservation);

            var equipment = await _equipmentRepo.GetByIdAsync(reservation.EquipmentId);
            if (equipment != null)
            {
                equipment.Status = AssetStatus.Available;
                await _equipmentRepo.UpdateAsync(equipment);
            }

            await transaction.CommitAsync();
            return ServiceResponse.Ok("Equipment returned.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ServiceResponse.Fail($"Transaction error: {ex.Message}");
        }
    }

    /// <inheritdoc />

    public async Task<ServiceResponse> CancelReservationAsync(Guid id)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        try
        {
            var reservation = await _repo.GetByIdAsync(id);
            if (reservation == null)
                return ServiceResponse.Fail("Reservation not found.");

            reservation.Status = ReservationStatus.Cancelled;
            await _repo.UpdateAsync(reservation);

            var equipment = await _equipmentRepo.GetByIdAsync(reservation.EquipmentId);
            if (equipment != null && equipment.Status == AssetStatus.Reserved)
            {
                equipment.Status = AssetStatus.Available;
                await _equipmentRepo.UpdateAsync(equipment);
            }

            await transaction.CommitAsync();
            return ServiceResponse.Ok("Reservation cancelled.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ServiceResponse.Fail($"Transaction error: {ex.Message}");
        }
    }
}