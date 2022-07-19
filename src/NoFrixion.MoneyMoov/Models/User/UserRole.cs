using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models
{
    public class UserRole
    {
        public Guid UserID { get; set; }

        public Guid MerchantID { get; set; }

        public UserRolesEnum RoleType { get; set; }
    }
}
