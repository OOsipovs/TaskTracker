using AuthService.Application.Services;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using TaskTracker.Shared;
using TaskTracker.Shared.DTOs;
using Xunit;

namespace AuthService.Tests.Services;

public class AuthenticationServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly IOptions<JwtSettings> _jwtSettings;
    private readonly AuthenticationService _authService;

    public AuthenticationServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _jwtSettings = Options.Create(new JwtSettings
        {
            Secret = "YourSuperSecretKeyForJWTTokenGenerationMustBeAtLeast32Characters",
            Issuer = "TaskTrackerAuthService",
            Audience = "TaskTrackerClients",
            ExpirationInMinutes = 60
        });
        _authService = new AuthenticationService(_mockUserRepository.Object, _jwtSettings);
    }

    [Fact]
    public async Task RegisterAsync_WithNewUsername_ReturnsLoginResponse()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "newuser",
            Password = "password123"
        };

        _mockUserRepository.Setup(x => x.UsernameExistsAsync(request.Username))
            .ReturnsAsync(false);

        _mockUserRepository.Setup(x => x.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => { u.Id = 1; return u; });

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("newuser", result.Username);
        Assert.NotEmpty(result.Token);
        _mockUserRepository.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingUsername_ReturnsNull()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "existinguser",
            Password = "password123"
        };

        _mockUserRepository.Setup(x => x.UsernameExistsAsync(request.Username))
            .ReturnsAsync(true);

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.Null(result);
        _mockUserRepository.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsLoginResponse()
    {
        // Arrange
        var password = "password123";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = hashedPassword
        };

        var request = new LoginRequest
        {
            Username = "testuser",
            Password = password
        };

        _mockUserRepository.Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync(user);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
        Assert.Equal(1, result.UserId);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword")
        };

        var request = new LoginRequest
        {
            Username = "testuser",
            Password = "wrongpassword"
        };

        _mockUserRepository.Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync(user);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WithNonExistentUser_ReturnsNull()
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = "nonexistent",
            Password = "password123"
        };

        _mockUserRepository.Setup(x => x.GetByUsernameAsync(request.Username))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.Null(result);
    }
}