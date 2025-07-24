// -----------------------------------------------------------------------------
//  Filename: RoleEvent.cs
// 
//  Description: A model that represents an event produced by a MoneyMoov
// Role execution.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  18 07 2025  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using NoFrixion.Common.Permissions;
using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models;

/// <summary>
/// A model that represents an event produced by a MoneyMoov Role execution.
/// </summary>
public class RoleEvent
{
    public Guid ID { get; set; }
    
    public Guid RoleID { get; set; }
    
    public string? Notes { get; set; }
    
    public RoleEventTypeEnum Type { get; set; }
    
    public DateTimeOffset Inserted { get; set; }
    
    public string? AuthoriserHash { get; set; }

    /// <summary>
    /// The name the role had at the time of the event.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The description the role had at the time of the event.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The merchant permissions the role had at the time of the event.
    /// </summary>
    public MerchantPermissions MerchantPermissions { get; set; }

    /// <summary>
    /// The account permissions the role had at the time of the event.
    /// </summary>
    public AccountPermissions AccountPermissions { get; set; }
    
    /// <summary>
    /// The client session timeout seconds the role had at the time of the event.
    /// </summary>
    public int? ClientSessionTimeoutSeconds { get; set; }
    
    /// <summary>
    /// For <see cref="RoleEventTypeEnum.AssignedToUser"/> and <see cref="RoleEventTypeEnum.RemovedFromUser"/> event types.
    /// The user that was assigned to or removed from the role at the time of the event.
    /// </summary>
    public Guid? AssignationEventUserID { get; set; }
    
    /// <summary>
    /// User that triggered the event.
    /// </summary>
    public User? User { get; set; }
}