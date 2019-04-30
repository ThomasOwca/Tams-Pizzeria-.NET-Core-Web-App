using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsPizzeriaWebApp.Models.EmployeeViewModels
{
    public class OrderHistoryDetailsViewModel
    {
        public int ID { get; set; }
        public int FulFilledByID { get; set; }
        public DateTime OrderDate { get; set;}
        public int Confirmation { get; set; }
        public string Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool OnlineOrder { get; set; }
        public string Crust { get; set; }
        public string Size { get; set; }
        public string Topping1 { get; set; }
        public string Topping2 { get; set; }
        public string Topping3 { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public decimal FinalTotal { get; set; }
        public string StoreID { get; set; }
        public string StoreStreetAddress { get; set; }
        public string StoreCity { get; set; }
        public string StoreState { get; set; }
        public int StoreZipCode { get; set; }
    }
}
