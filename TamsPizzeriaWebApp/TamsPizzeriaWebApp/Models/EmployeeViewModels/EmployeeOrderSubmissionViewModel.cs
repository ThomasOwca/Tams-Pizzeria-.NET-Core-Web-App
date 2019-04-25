using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsPizzeriaWebApp.Models.EmployeeViewModels
{
    public class EmployeeOrderSubmissionViewModel
    {
        public string Size { get; set; }
        public string Crust { get; set; }
        public string Topping1 { get; set; }
        public string Topping2 { get; set; }
        public string Topping3 { get; set; }
        public int Quantity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ConfirmationNumber { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public int EmployeeID { get; set; }
        public string TimeSubmitted { get; set; }
        public string DateSubmitted { get; set; }
    }
}
