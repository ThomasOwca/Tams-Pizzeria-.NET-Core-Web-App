using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TamsPizzeriaWebApp.Data;

namespace TamsPizzeriaWebApp.Authorization.Requirements
{
    public class EmployeeRequirement : IAuthorizationRequirement
    {
        public EmployeeRequirement() {}
    }
}
