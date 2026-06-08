using Moq;
using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services;
using TradeOffStackAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TradeOffStackAPI.Tests.Services;

public class MaintenanceRequestServiceTests
{
    private readonly Mock<IMaintenanceRequestRepository> _mockRepo;
    private readonly Mock<IEquipmentRepository> _mockEquipmentRepo;
    private readonly TradeOffStackAPI.Data.AppDbContext _context;
    private readonly IMaintenanceRequestService _service;

    public MaintenanceRequestServiceTests()
    {
        _mockRepo = new Mock<IMaintenanceRequestRepository>();
        _mockEquipmentRepo = new Mock<IEquipmentRepository>();

        var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<TradeOffStackAPI.Data.AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        _context = new TradeOffStackAPI.Data.AppDbContext(options);

        _service = new MaintenanceRequestService(_mockRepo.Object, _mockEquipmentRepo.Object, _context);
    }

    [Fact]
    public async Task CreateRequestAsync_WhenEquipmentNotFound_ShouldFail()
    {
        // Arrange
        var request = new MaintenanceRequest { EquipmentId = Guid.NewGuid() };
        _mockEquipmentRepo.Setup(r => r.GetByIdAsync(request.EquipmentId)).ReturnsAsync((Equipment?)null);

        // Act
        var response = await _service.CreateRequestAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("The specified equipment does not exist.", response.Message);
    }

    [Fact]
    public async Task CreateRequestAsync_WhenHasOpenRequest_ShouldFail()
    {
        // Arrange
        var request = new MaintenanceRequest { EquipmentId = Guid.NewGuid() };
        _mockEquipmentRepo.Setup(r => r.GetByIdAsync(request.EquipmentId)).ReturnsAsync(new Equipment());
        _mockRepo.Setup(r => r.HasOpenRequestAsync(request.EquipmentId)).ReturnsAsync(true);

        // Act
        var response = await _service.CreateRequestAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("This equipment already has an open maintenance request.", response.Message);
    }

    [Fact]
    public async Task CompleteRequestAsync_WhenRequestNotFound_ShouldFail()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(requestId)).ReturnsAsync((MaintenanceRequest?)null);

        // Act
        var response = await _service.CompleteRequestAsync(requestId, "Notes");

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Request not found.", response.Message);
    }
}