using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TamsPizzeriaWebApp.Models.AccountViewModels;
using TamsPizzeriaWebApp.Services;

namespace TamsPizzeriaWebApp.Controllers
{
    public class CustomerController : Controller
    {
        private IOrderHistory _orderHistory;
        private ICustomer _customerStats;

        public CustomerController(IOrderHistory history, ICustomer customer)
        {
            _orderHistory = history;
            _customerStats = customer;
        }

        public IActionResult History()
        {
            int currentCustomerID = _customerStats.GetCurrentOnlineCustomerID(User);
            var orders = _orderHistory.GetAllOrdersByOnlineCustomerID(currentCustomerID);

            var model = new OnlineHistoryViewModel
            {
                Orders = orders
            };

            return View(model);
        }
    }
}