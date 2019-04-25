using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsPizzeriaWebApp.Models.EmployeeViewModels
{
    public class EmployeeViewModel
    {
        public string EmployeeFullName { get; set; }
        public string AdminPrivilege { get; set; }
        public string ManagerPrivilege { get; set; }
        public int OrdersCreated { get; set; }
    }
}
