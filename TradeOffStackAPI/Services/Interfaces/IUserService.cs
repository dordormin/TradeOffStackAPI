using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;

namespace TradeOffStackAPI.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResponse<IEnumerable<User>>> GetAllAsync();
    Task<ServiceResponse<User>> GetByIdAsync(Guid id);
    Task<ServiceResponse<User>> GetByEmailAsync(string email);
    Task<ServiceResponse<IEnumerable<User>>> GetByDepartmentAsync(Guid departmentId);
    Task<ServiceResponse<User>> AddUserAsync(User user);
    Task<ServiceResponse<User>> UpdateUserAsync(Guid id, User user);
    Task<ServiceResponse<bool>> DeleteUserAsync(Guid id);
}