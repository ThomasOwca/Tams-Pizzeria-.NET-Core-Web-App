using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsPizzeriaWebApp.Models.CreateOrderViewModels
{
    public class OrderConfirmationViewModel
    {
        public int Quantity { get; set; }
        public string PizzaSize { get; set; }
        public string PizzaCrust { get; set; }
        public string Topping1 { get; set; }
        public string Topping2 { get; set; }
        public string Topping3 { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string PickUpLocation { get; set; }
        public string TimeSubmitted { get; set; }
        public string DateSubmitted { get; set; }
        public string EstimatedReady { get; set; }
        public int ConfirmationNumber { get; set; }
    }
}
