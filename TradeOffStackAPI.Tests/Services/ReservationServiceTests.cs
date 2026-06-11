using Moq;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services;
using TradeOffStackAPI.Services.Interfaces;
using TradeOffStackAPI.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using TradeOffStackAPI.Dtos;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TradeOffStackAPI.Tests.Services;

public class ReservationServiceTests
{
    private readonly Mock<IReservationRepository> _mockRepo;
    private readonly Mock<IEquipmentRepository> _mockEquipmentRepo;
    private readonly AssetDbContext _context;
    private readonly Mock<IUserService> _mockUserService;
    private readonly IReservationService _service;

    public ReservationServiceTests()
    {
        _mockRepo = new Mock<IReservationRepository>();
        _mockEquipmentRepo = new Mock<IEquipmentRepository>();
        
        var options = new DbContextOptionsBuilder<AssetDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            // CORRECTION : On dit à la base In-Memory d'ignorer les appels aux transactions
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        _context = new AssetDbContext(options);
        _mockUserService = new Mock<IUserService>();

        _service = new ReservationService(_mockRepo.Object, _mockEquipmentRepo.Object, _context, _mockUserService.Object);
    }

    private static Reservation NewReservation(Guid equipmentId) => new()
    {
        Id = Guid.NewGuid(),
        EquipmentId = equipmentId,
        UserId = Guid.NewGuid(),
        StartDate = new DateTime(2026, 6, 5, 0, 0, 0, DateTimeKind.Utc)
    };

    [Fact]
    public async Task CreateReservationAsync_WhenEquipmentAvailable_ShouldSucceed()
    {
        // Arrange
        var equipmentId = Guid.NewGuid();
        var equipment = new Equipment { Id = equipmentId, Name = "Laptop", Status = AssetStatus.Available };
        
        _mockEquipmentRepo.Setup(r => r.GetByIdAsync(equipmentId)).ReturnsAsync(equipment);
        _mockRepo.Setup(r => r.HasActiveReservationAsync(equipmentId)).ReturnsAsync(false);
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Reservation>()))
            .Callback<Reservation>(r => 
            {
                _context.Reservations.Add(r);
                _context.SaveChanges();
            })
            .ReturnsAsync(true);
        _mockEquipmentRepo.Setup(r => r.UpdateAsync(It.IsAny<Equipment>())).ReturnsAsync(true);

        var reservation = NewReservation(equipmentId);

        // Act
        var response = await _service.CreateReservationAsync(reservation);

        // Assert
        Assert.True(response.Success);
        Assert.Equal(AssetStatus.Reserved, equipment.Status);
        _mockRepo.Verify(r => r.AddAsync(reservation), Times.Once);
    }

    [Fact]
    public async Task CreateReservationAsync_WhenEquipmentNotAvailable_ShouldFail()
    {
        // Arrange
        var equipmentId = Guid.NewGuid();
        var equipment = new Equipment { Id = equipmentId, Name = "Laptop", Status = AssetStatus.Reserved };
        _mockEquipmentRepo.Setup(r => r.GetByIdAsync(equipmentId)).ReturnsAsync(equipment);

        var reservation = NewReservation(equipmentId);

        // Act
        var response = await _service.CreateReservationAsync(reservation);

        // Assert
        Assert.False(response.Success);
        Assert.Contains("not available", response.Message);
    }
}