using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TamsPizzeriaWebApp.Authorization.Requirements;
using TamsPizzeriaWebApp.Data;

namespace TamsPizzeriaWebApp.Authorization.Handlers
{
    public class IsEmployeeHandler : AuthorizationHandler<EmployeeRequirement>
    {
        private ApplicationDbContext _context;
        public IsEmployeeHandler([FromServices] ApplicationDbContext context)
        {
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployeeRequirement requirement)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == context.User.Identity.Name);

            if (user != null)
            {
                if (user.Employee == "Yes")
                {
                    context.Succeed(requirement);
                }
            }
            
            return Task.CompletedTask;  
        }
    }
}
