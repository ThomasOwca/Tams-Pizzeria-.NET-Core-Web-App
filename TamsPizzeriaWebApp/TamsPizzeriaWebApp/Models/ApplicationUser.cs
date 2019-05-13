using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TamsPizzeriaWebApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Employee { get; set; }
        public string Manager { get; set; }
        public string Admin { get; set; }

        public int HowManyOrdersPlaced { get; set; }
        public int HowManyCustomerOrdersPlaced { get; set; }
        public int EmployeeID { get; set; }
        public int CustomerID { get; set; }
    }
}
