using PizzeriaData.Models;
using System.Collections.Generic;

namespace TamsPizzeriaWebApp.Models.EmployeeViewModels
{
    public class OrderHistoryViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
    }
}
