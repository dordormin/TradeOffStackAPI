using Microsoft.Extensions.Options;
using Moq;
using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services;
using TradeOffStackAPI.Services.Interfaces;
using Xunit;

namespace TradeOffStackAPI.Tests.Services;

public class EquipmentServiceTests
{
    private readonly Mock<IEquipmentRepository> _mockRepo;
    private readonly Mock<IOptions<CloudflareR2Settings>> _mockR2Settings;
    private readonly Mock<IDepreciationService> _mockDepreciationService;
    private readonly IEquipmentService _service;

    public EquipmentServiceTests()
    {
        _mockRepo = new Mock<IEquipmentRepository>();
        _mockR2Settings = new Mock<IOptions<CloudflareR2Settings>>();
        _mockDepreciationService = new Mock<IDepreciationService>();
        
        _mockR2Settings.Setup(s => s.Value).Returns(new CloudflareR2Settings
        {
            PublicUrl = "https://r2.example.com/bucket"
        });
        
        _service = new EquipmentService(_mockRepo.Object, _mockDepreciationService.Object, _mockR2Settings.Object);
    }

    private static Equipment NewEquipment(string name = "Dell XPS") => new()
    {
        Id = Guid.NewGuid(),
        Name = name,
        SerialNumber = "SN-001",
        Status = AssetStatus.Available,
        Category = AssetCategory.Laptop
    };

    [Fact]
    public async Task GetInventoryAsync_ShouldBuildImageUrls()
    {
        // Arrange
        var equipment1 = NewEquipment();
        equipment1.Image = "image1.jpg";
        var equipment2 = NewEquipment("HP EliteBook");
        equipment2.Image = "image2.jpg";
        var data = new List<Equipment> { equipment1, equipment2 };
        
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(data);

        // Act
        var response = await _service.GetInventoryAsync();
        var result = response.Data!.ToList();

        // Assert
        Assert.True(response.Success);
        Assert.Equal(2, result.Count);
        Assert.Equal("https://r2.example.com/bucket/Equipments/image1.jpg", result[0].ImageUrl);
        Assert.Equal("https://r2.example.com/bucket/Equipments/image2.jpg", result[1].ImageUrl);
    }

    [Fact]
    public async Task AddEquipmentAsync_ShouldBuildImageUrlBeforeSaving()
    {
        // Arrange
        var equipment = NewEquipment();
        equipment.Image = "new_image.png";
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Equipment>())).ReturnsAsync(true);

        // Act
        var response = await _service.AddEquipmentAsync(equipment);

        // Assert
        Assert.True(response.Success);
        _mockRepo.Verify(r => r.AddAsync(It.Is<Equipment>(e => 
            e.ImageUrl == "https://r2.example.com/bucket/Equipments/new_image.png"
        )), Times.Once);
    }
    
    [Fact]
    public async Task UpdateEquipmentAsync_WhenExists_ShouldBuildUrlAndUpdate()
    {
        // Arrange
        var equipmentId = Guid.NewGuid();
        var existingEquipment = new Equipment { Id = equipmentId, Name = "Old Name" };
        var updatedEquipmentData = NewEquipment("New Name");
        updatedEquipmentData.Image = "updated.jpg";

        _mockRepo.Setup(r => r.GetByIdAsync(equipmentId)).ReturnsAsync(existingEquipment);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Equipment>())).ReturnsAsync(true);

        // Act
        var response = await _service.UpdateEquipmentAsync(equipmentId, updatedEquipmentData);

        // Assert
        Assert.True(response.Success);
        _mockRepo.Verify(r => r.UpdateAsync(It.Is<Equipment>(e => 
            e.Id == equipmentId &&
            e.Name == "New Name" &&
            e.ImageUrl == "https://r2.example.com/bucket/Equipments/updated.jpg"
        )), Times.Once);
    }

    [Fact]
    public async Task UpdateEquipmentAsync_WhenNotExists_ShouldFail()
    {
        // Arrange
        var equipment = NewEquipment();
        _mockRepo.Setup(r => r.GetByIdAsync(equipment.Id)).ReturnsAsync((Equipment?)null);

        // Act
        var response = await _service.UpdateEquipmentAsync(equipment.Id, equipment);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Equipment not found.", response.Message);
    }

    [Fact]
    public async Task AddEquipmentAsync_WithEmptyName_ShouldFail()
    {
        // Arrange
        var equipment = NewEquipment(name: "");

        // Act
        var response = await _service.AddEquipmentAsync(equipment);
        
        // Assert
        Assert.False(response.Success);
        Assert.Equal("Equipment name is required.", response.Message);
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldFail()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Equipment?)null);

        // Act
        var response = await _service.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.False(response.Success);
    }

    [Fact]
    public async Task DeleteEquipmentAsync_WhenNotExists_ShouldFail()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        // Act
        var response = await _service.DeleteEquipmentAsync(id);

        // Assert
        Assert.False(response.Success);
    }
}