using Microsoft.Extensions.Options;
using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly string _r2BaseUrl;

    public UserService(IUserRepository repo, IOptions<CloudflareR2Settings> r2Settings)
    {
        _repo = repo;
        _r2BaseUrl = r2Settings.Value.PublicUrl;
    }

    private void BuildUserImageUrls(User user)
    {
        if (!string.IsNullOrEmpty(user.ProfileImage))
        {
            user.ProfileImageUrl = $"{_r2BaseUrl}/Users/{user.ProfileImage}";
        }
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<IEnumerable<User>>> GetAllAsync()
    {
        var users = await _repo.GetAllAsync();
        foreach (var user in users)
        {
            BuildUserImageUrls(user);
        }
        return ServiceResponse<IEnumerable<User>>.Ok(users);
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<User>> GetByIdAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null)
        {
            return ServiceResponse<User>.Fail("User not found.");
        }
        BuildUserImageUrls(user);
        return ServiceResponse<User>.Ok(user);
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<User>> GetByEmailAsync(string email)
    {
        var user = await _repo.GetByEmailAsync(email);
        if (user == null)
        {
            return ServiceResponse<User>.Fail("User not found.");
        }
        BuildUserImageUrls(user);
        return ServiceResponse<User>.Ok(user);
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<IEnumerable<User>>> GetByDepartmentAsync(Guid departmentId)
    {
        var users = await _repo.GetByDepartmentAsync(departmentId);
        foreach (var user in users)
        {
            BuildUserImageUrls(user);
        }
        return ServiceResponse<IEnumerable<User>>.Ok(users);
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<User>> AddUserAsync(User user)
    {
        if (string.IsNullOrEmpty(user.FirstName))
            return ServiceResponse<User>.Fail("First name is required.");
        if (string.IsNullOrEmpty(user.Email))
            return ServiceResponse<User>.Fail("Email is required.");

        var existing = await _repo.GetByEmailAsync(user.Email);
        if (existing != null)
            return ServiceResponse<User>.Fail($"A user with the email '{user.Email}' already exists.");

        if (string.IsNullOrEmpty(user.PasswordHash))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!");
        }

        BuildUserImageUrls(user);
        
        var success = await _repo.AddAsync(user);
        return success 
            ? ServiceResponse<User>.Ok(user, "User successfully created.") 
            : ServiceResponse<User>.Fail("Failed to save the user.");
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<User>> UpdateUserAsync(Guid id, User user)
    {
        var existingUser = await _repo.GetByIdAsync(id);
        if (existingUser == null) 
            return ServiceResponse<User>.Fail("User not found.");

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.ProfileImage = user.ProfileImage;
        existingUser.Role = user.Role;
        existingUser.DepartmentId = user.DepartmentId;
        existingUser.IsActive = user.IsActive;

        BuildUserImageUrls(existingUser);
        
        var success = await _repo.UpdateAsync(existingUser);
        return success 
            ? ServiceResponse<User>.Ok(existingUser, "User updated.") 
            : ServiceResponse<User>.Fail("Failed to update the user.");
    }

    /// <inheritdoc />

    public async Task<ServiceResponse<bool>> DeleteUserAsync(Guid id)
    {
        var success = await _repo.DeleteAsync(id);
        return success 
            ? ServiceResponse<bool>.Ok(true, "User deleted.") 
            : ServiceResponse<bool>.Fail("User not found or failed to delete.");
    }
}

// Classe pour mapper la configuration de Cloudflare R2
public class CloudflareR2Settings
{
    public const string SectionName = "CloudflareR2";
    public string PublicUrl { get; set; } = string.Empty;
}