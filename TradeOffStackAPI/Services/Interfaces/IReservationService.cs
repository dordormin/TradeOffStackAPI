using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Services.Interfaces;

public interface IReservationService
{
    Task<ServiceResponse<IEnumerable<Reservation>>> GetAllAsync();
    Task<ServiceResponse<Reservation>> GetByIdAsync(Guid id);
    Task<ServiceResponse<IEnumerable<Reservation>>> GetByEquipmentAsync(Guid equipmentId);
    Task<ServiceResponse<IEnumerable<Reservation>>> GetByUserAsync(Guid userId);
    Task<ServiceResponse<IEnumerable<Reservation>>> GetByDepartmentAsync(Guid departmentId);
    Task<ServiceResponse<IEnumerable<Reservation>>> GetActiveAsync();
    Task<ServiceResponse<Reservation>> CreateReservationAsync(Reservation reservation);
    Task<ServiceResponse<Reservation>> UpdateReservationAsync(Guid id, Reservation reservation);
    Task<ServiceResponse> ReturnEquipmentAsync(Guid reservationId);
    Task<ServiceResponse> CancelReservationAsync(Guid id);
}