// -----------------------------------------------------------------------------
//  Filename: PermissionGroup.cs
// 
//  Description: Represents a group of permissions that can be assigned to users:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  15 08 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using JetBrains.Annotations;
using NoFrixion.Common.Permissions;

namespace NoFrixion.MoneyMoov.Models.Permissions;

public class PermissionGroup
{
    public Guid ID { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid MerchantID { get; set; }

    public List<PermissionGroupMerchantPermission>? MerchantPermissions { get; set; }

    public List<PermissionGroupAccountPermission>? AccountPermissions { get; set; }
    
    public PermissionGroupUserPermission? UserPermissions { get; set; }
    
    public List<User>? Users { get; set; }
    
    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
}

public class PermissionGroupUserPermission
{
    public Guid ID { get; set; }

    public Guid GroupID { get; set; }

    public UserPermissions Permissions { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
}

public class PermissionGroupAccountPermission
{
    public Guid ID { get; set; }

    public Guid GroupID { get; set; }

    public Guid AccountID { get; set; }

    public AccountPermissions Permissions { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
}

public class PermissionGroupMerchantPermission
{
    public Guid ID { get; set; }

    public Guid GroupID { get; set; }

    public Guid MerchantID { get; set; }

    public MerchantPermissions Permissions { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
}