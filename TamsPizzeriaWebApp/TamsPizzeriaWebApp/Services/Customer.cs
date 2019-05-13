using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TamsPizzeriaWebApp.Data;

namespace TamsPizzeriaWebApp.Services
{
    public class Customer : ICustomer
    {
        private ApplicationDbContext _context;

        public Customer(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CreateNewRegistrationCustomerID()
        {
            return _context.Users.Max(user => user.CustomerID) + 1; 
        }

        public int GetCurrentOnlineCustomerID(ClaimsPrincipal currentUser)
        {
            return _context.Users.FirstOrDefault(user => user.UserName == currentUser.Identity.Name).CustomerID;
        }
    }
}
