using Moq;
using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services;
using TradeOffStackAPI.Services.Interfaces;
using Xunit;

namespace TradeOffStackAPI.Tests.Services;

public class DepartmentServiceTests
{
    private readonly Mock<IDepartmentRepository> _mockRepo;
    private readonly IDepartmentService _service;

    public DepartmentServiceTests()
    {
        _mockRepo = new Mock<IDepartmentRepository>();
        _service = new DepartmentService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllDepartments()
    {
        // Arrange
        var departments = new List<Department> { new(), new() };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(departments);

        // Act
        var response = await _service.GetAllAsync();

        // Assert
        Assert.True(response.Success);
        Assert.Equal(2, response.Data!.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldFail()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Department?)null);

        // Act
        var response = await _service.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Department not found.", response.Message);
    }

    [Fact]
    public async Task AddDepartmentAsync_WithEmptyName_ShouldFail()
    {
        // Arrange
        var department = new Department { Name = "" };

        // Act
        var response = await _service.AddDepartmentAsync(department);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Department name is required.", response.Message);
    }

    [Fact]
    public async Task UpdateDepartmentAsync_WhenNotExists_ShouldFail()
    {
        // Arrange
        var department = new Department { Id = Guid.NewGuid(), Name = "Test" };
        _mockRepo.Setup(r => r.GetByIdAsync(department.Id)).ReturnsAsync((Department?)null);

        // Act
        var response = await _service.UpdateDepartmentAsync(department.Id, department);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Department not found.", response.Message);
    }

    [Fact]
    public async Task DeleteDepartmentAsync_WhenNotExists_ShouldFail()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(false);

        // Act
        var response = await _service.DeleteDepartmentAsync(id);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Department not found or failed to delete.", response.Message);
    }
}