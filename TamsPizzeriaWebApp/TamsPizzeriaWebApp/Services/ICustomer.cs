using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TamsPizzeriaWebApp.Services
{
    interface ICustomer
    {
        int GetCurrentOnlineCustomerID(ClaimsPrincipal currentUser);
    }
}
