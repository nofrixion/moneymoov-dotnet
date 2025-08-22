// -----------------------------------------------------------------------------
//  Filename: RoleUser.cs
// 
//  Description: Contains the role user model.:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  19 08 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
//  22 08 2025  Aaron Clauson    Added GetApprovalHash for role user authorisatoions.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.Roles;

public class RoleUser
{
    public Guid ID { get; set; }

    public Guid RoleID { get; set; }

    public Guid UserID { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>
    /// Indicates whether the role user is enabled. 
    /// An enabled role user is authorized to perform actions associated with their assigned role.
    /// If set to <c>false</c>, the user is considered disabled and will not be granted authorization for role-based actions.
    /// </summary>
    public bool IsEnabled { get; set; }

    public List<RoleUserAccount>? Accounts { get; set; }

    public User? User { get; set; }
    
    public Role? Role { get; set; }

    /// <summary>
    /// Gets a hash of the critical fields for the role user. This hash is
    /// used to ensure a role user's details are not modified between the time the
    /// authorisation is given and the time the role user assignment is enabled.
    /// Note the authorisation logic is currently only for the initial role user assignment.
    /// It is not intended to cover subsequent updates to the list of accounts the user gets access to 
    /// for the role. In other words, once a user has been authorised for a role for a merchant they
    /// do not need to be re-authorised if the list of accounts changes. Because of this there is no 
    /// nonce required in the approval hash as it does not need to accommodate updates.
    /// </summary>
    /// <returns>A hash of the role user's critical fields.</returns>
    public string GetApprovalHash()
    {
        var input =
            RoleID.ToString() +
            UserID.ToString() +
            Accounts?.OrderBy(a => a.AccountID).Select(a => a.AccountID.ToString()).Aggregate((a, b) => a + b);

        return HashHelper.CreateHash(input);
    }
}