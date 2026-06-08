using Microsoft.Extensions.Options;
using Moq;
using TradeOffStackAPI.Dtos;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Repositories.Interfaces;
using TradeOffStackAPI.Services;
using TradeOffStackAPI.Services.Interfaces;
using Xunit;

namespace TradeOffStackAPI.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly Mock<IOptions<CloudflareR2Settings>> _mockR2Settings;
    private readonly IUserService _service;

    public UserServiceTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        _mockR2Settings = new Mock<IOptions<CloudflareR2Settings>>();

        _mockR2Settings.Setup(s => s.Value).Returns(new CloudflareR2Settings
        {
            PublicUrl = "https://r2.example.com/bucket"
        });

        _service = new UserService(_mockRepo.Object, _mockR2Settings.Object);
    }

    private static User NewUser(string email = "test@example.com") => new()
    {
        Id = Guid.NewGuid(),
        FirstName = "Test",
        LastName = "User",
        Email = email
    };

    [Fact]
    public async Task GetAllAsync_ShouldBuildProfileImageUrls()
    {
        // Arrange
        var user1 = NewUser("user1@test.com");
        user1.ProfileImage = "avatar1.jpg";
        var user2 = NewUser("user2@test.com");
        var data = new List<User> { user1, user2 };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(data);

        // Act
        var response = await _service.GetAllAsync();
        var result = response.Data!.ToList();

        // Assert
        Assert.True(response.Success);
        Assert.Equal(2, result.Count);
        Assert.Equal("https://r2.example.com/bucket/Users/avatar1.jpg", result[0].ProfileImageUrl);
        Assert.Null(result[1].ProfileImageUrl);
    }

    [Fact]
    public async Task AddUserAsync_ShouldBuildUrlBeforeSaving()
    {
        // Arrange
        var user = NewUser();
        user.ProfileImage = "new_avatar.png";
        _mockRepo.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync((User?)null);
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(true);

        // Act
        var response = await _service.AddUserAsync(user);

        // Assert
        Assert.True(response.Success);
        _mockRepo.Verify(r => r.AddAsync(It.Is<User>(u => 
            u.ProfileImageUrl == "https://r2.example.com/bucket/Users/new_avatar.png"
        )), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldBuildUrlBeforeSaving()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = new User { Id = userId };
        var updatedUserData = NewUser();
        updatedUserData.ProfileImage = "updated_avatar.png";

        _mockRepo.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(existingUser);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(true);

        // Act
        var response = await _service.UpdateUserAsync(userId, updatedUserData);

        // Assert
        Assert.True(response.Success);
        _mockRepo.Verify(r => r.UpdateAsync(It.Is<User>(u => 
            u.Id == userId &&
            u.ProfileImageUrl == "https://r2.example.com/bucket/Users/updated_avatar.png"
        )), Times.Once);
    }

    [Fact]
    public async Task AddUserAsync_WhenEmailExists_ShouldFail()
    {
        // Arrange
        var user = NewUser();
        _mockRepo.Setup(r => r.GetByEmailAsync(user.Email)).ReturnsAsync(new User());

        // Act
        var response = await _service.AddUserAsync(user);
        
        // Assert
        Assert.False(response.Success);
        Assert.Contains("existe déjà", response.Message);
    }
}