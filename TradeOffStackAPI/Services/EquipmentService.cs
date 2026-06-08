using Microsoft.Extensions.Options;
using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services.Interfaces;

namespace TradeOffStackAPI.Services;

/// <summary>
/// Service implementation for managing equipment inventory.
/// Handles business rules, validations, and image URL construction.
/// </summary>
public class EquipmentService : IEquipmentService
{
    private readonly IEquipmentRepository _repo;
    private readonly string _r2BaseUrl;

    /// <summary>
    /// Initializes a new instance of the <see cref="EquipmentService"/> class.
    /// </summary>
    /// <param name="repo">The equipment repository.</param>
    /// <param name="r2Settings">Cloudflare R2 settings for image URLs.</param>
    public EquipmentService(IEquipmentRepository repo, IOptions<CloudflareR2Settings> r2Settings)
    {
        _repo = repo;
        _r2BaseUrl = r2Settings.Value.PublicUrl;
    }

    /// <summary>
    /// Constructs the full URLs for the equipment's image based on the R2 base URL.
    /// </summary>
    /// <param name="equipment">The equipment entity to modify.</param>
    private void BuildEquipmentImageUrls(Equipment equipment)
    {
        if (!string.IsNullOrEmpty(equipment.Image))
        {
            var url = $"{_r2BaseUrl}/Equipments/{equipment.Image}";
            equipment.ImageUrl = url;
            equipment.ImageUrlHttps = url;
        }
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<IEnumerable<Equipment>>> GetInventoryAsync()
    {
        var equipments = await _repo.GetAllAsync();
        foreach (var eq in equipments)
        {
            BuildEquipmentImageUrls(eq);
        }
        return ServiceResponse<IEnumerable<Equipment>>.Ok(equipments);
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<Equipment>> GetByIdAsync(Guid id)
    {
        var equipment = await _repo.GetByIdAsync(id);
        if (equipment == null)
        {
            return ServiceResponse<Equipment>.Fail("Equipment not found.");
        }
        BuildEquipmentImageUrls(equipment);
        return ServiceResponse<Equipment>.Ok(equipment);
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<IEnumerable<Equipment>>> GetByStatusAsync(AssetStatus status)
    {
        var equipments = await _repo.GetByStatusAsync(status);
        foreach (var eq in equipments)
        {
            BuildEquipmentImageUrls(eq);
        }
        return ServiceResponse<IEnumerable<Equipment>>.Ok(equipments);
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<IEnumerable<Equipment>>> GetByCategoryAsync(AssetCategory category)
    {
        var equipments = await _repo.GetByCategoryAsync(category);
        foreach (var eq in equipments)
        {
            BuildEquipmentImageUrls(eq);
        }
        return ServiceResponse<IEnumerable<Equipment>>.Ok(equipments);
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<Equipment>> AddEquipmentAsync(Equipment equipment)
    {
        if (string.IsNullOrEmpty(equipment.Name))
            return ServiceResponse<Equipment>.Fail("Equipment name is required.");
        
        BuildEquipmentImageUrls(equipment);
        
        var success = await _repo.AddAsync(equipment);
        return success
            ? ServiceResponse<Equipment>.Ok(equipment, "Equipment successfully created.")
            : ServiceResponse<Equipment>.Fail("Failed to save the equipment.");
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<Equipment>> UpdateEquipmentAsync(Guid id, Equipment equipment)
    {
        var existingEquipment = await _repo.GetByIdAsync(id);
        if (existingEquipment == null)
            return ServiceResponse<Equipment>.Fail("Equipment not found.");

        existingEquipment.Name = equipment.Name;
        existingEquipment.SerialNumber = equipment.SerialNumber;
        existingEquipment.Status = equipment.Status;
        existingEquipment.Category = equipment.Category;
        existingEquipment.Description = equipment.Description;
        existingEquipment.Price = equipment.Price;
        existingEquipment.Image = equipment.Image;
        existingEquipment.PurchaseDate = equipment.PurchaseDate;

        BuildEquipmentImageUrls(existingEquipment);
        
        var success = await _repo.UpdateAsync(existingEquipment);
        return success
            ? ServiceResponse<Equipment>.Ok(existingEquipment, "Equipment successfully updated.")
            : ServiceResponse<Equipment>.Fail("Failed to update the equipment.");
    }

    /// <inheritdoc />
    public async Task<ServiceResponse> DeleteEquipmentAsync(Guid id)
    {
        var success = await _repo.DeleteAsync(id);
        return success
            ? ServiceResponse.Ok("Equipment successfully deleted.")
            : ServiceResponse.Fail("Equipment not found or failed to delete.");
    }
}