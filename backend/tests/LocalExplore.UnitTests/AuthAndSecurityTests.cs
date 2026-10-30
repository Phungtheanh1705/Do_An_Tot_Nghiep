using LocalExplore.Domain.Entities;
using LocalExplore.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Xunit;
using FluentAssertions;
using System.Security.Claims;

namespace LocalExplore.UnitTests;

public class AuthAndSecurityTests
{
    [Fact]
    public void BCrypt_HashPassword_ShouldVerifyCorrectly()
    {
        // Arrange
        var password = "SecurePassword@123";

        // Act
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        var isValid = BCrypt.Net.BCrypt.Verify(password, hash);
        var isInvalid = BCrypt.Net.BCrypt.Verify("WrongPassword!999", hash);

        // Assert
        hash.Should().NotBeNullOrEmpty();
        isValid.Should().BeTrue();
        isInvalid.Should().BeFalse();
    }

    [Fact]
    public void TokenService_GenerateAccessToken_ShouldIncludeUserClaimsAndRoles()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Jwt:SecretKey", "SuperSecretKeyForLocalExploreGraduationThesis2026!@#" },
            { "Jwt:Issuer", "LocalExplore" },
            { "Jwt:Audience", "LocalExploreClient" },
            { "Jwt:AccessTokenExpirationMinutes", "60" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var tokenService = new TokenService(configuration);
        var user = new User
        {
            Id = 15,
            Email = "host.sapa@localexplore.vn",
            FullName = "Vàng A Thào"
        };
        var roles = new List<string> { "Host", "Customer" };

        // Act
        var token = tokenService.GenerateAccessToken(user, roles, hostProfileId: 101);

        // Assert
        token.Should().NotBeNullOrEmpty();
        token.Split('.').Should().HaveCount(3); // Standard JWT Header.Payload.Signature

        var principal = tokenService.GetPrincipalFromExpiredToken(token);
        principal.Should().NotBeNull();
        
        var nameIdClaim = principal!.FindFirst(ClaimTypes.NameIdentifier);
        nameIdClaim.Should().NotBeNull();
        nameIdClaim!.Value.Should().Be("15");

        var emailClaim = principal.FindFirst(ClaimTypes.Email);
        emailClaim.Should().NotBeNull();
        emailClaim!.Value.Should().Be("host.sapa@localexplore.vn");

        var roleClaims = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        roleClaims.Should().Contain("Host");
        roleClaims.Should().Contain("Customer");

        var hostProfileClaim = principal.FindFirst("HostProfileId");
        hostProfileClaim.Should().NotBeNull();
        hostProfileClaim!.Value.Should().Be("101");
    }
}
