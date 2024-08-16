using System.Security.Claims;
using NoFrixion.Common.Permissions;
using NoFrixion.MoneyMoov.Claims;
using Xunit;

namespace NoFrixion.MoneyMoov.UnitTests.Extensions;

public class IdentityExtensionsTests
{
    [Fact]
    public void HasMerchantPermission_Returns_True_When_Permission_Found_For_Merchant()
    {
        // Arrange
        var merchantID = Guid.NewGuid();

        var merchantPermissions = $"{MerchantPermissions.CanApprovePayruns},{MerchantPermissions.CanCreatePayruns}";

        var identity = new ClaimsIdentity(new[]
        {
            new Claim($"{ClaimTypePrefixes.MERCHANT}.{merchantID}", merchantPermissions),
        });

        // Act

        var hasPermissions = identity.HasMerchantPermission(MerchantPermissions.CanApprovePayruns, merchantID);

        // Assert

        Assert.True(hasPermissions);
    }
    
    [Fact]
    public void HasMerchantPermission_Returns_False_When_Permission_Not_Found_For_Merchant()
    {
        // Arrange
        var merchantID = Guid.NewGuid();

        var merchantPermissions = $"{MerchantPermissions.CanApprovePayruns},{MerchantPermissions.CanCreatePayruns}";

        var identity = new ClaimsIdentity(new[]
        {
            new Claim($"{ClaimTypePrefixes.MERCHANT}.{merchantID}", merchantPermissions),
        });

        // Act

        var hasPermissions = identity.HasMerchantPermission(MerchantPermissions.CanCreateAccounts, merchantID);

        // Assert

        Assert.False(hasPermissions);
    }
}