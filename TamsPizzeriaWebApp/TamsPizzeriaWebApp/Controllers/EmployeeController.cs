using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PizzeriaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TamsPizzeriaWebApp.Data;
using TamsPizzeriaWebApp.Models.EmployeeViewModels;
using TamsPizzeriaWebApp.Services;

namespace TamsPizzeriaWebApp.Controllers
{
    [Authorize("IsEmployee")]
    public class EmployeeController : Controller
    {
        private IPizzeriaMenu _pizzeria;
        private IOrder _order;
        private IOrderHistory _orderHistory;
  
        public EmployeeController(IPizzeriaMenu pizzeria, IOrder order, IOrderHistory orderHistory)
        {
            _pizzeria = pizzeria;
            _order = order;
            _orderHistory = orderHistory;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Service()
        {
            List<int> quantities = new List<int>();

            for (int i = 1; i <= 10; i++)
            {
                quantities.Add(i);
            }

            EmployeeCreateOrderViewModel model = new EmployeeCreateOrderViewModel
            {
                Sizes = _pizzeria.GetSizes().Select(s => new SelectListItem { Text = s.Type }),
                Crusts = _pizzeria.GetCrusts().Select(c => new SelectListItem {  Text = c.Type }),
                Topping1 = _pizzeria.GetToppings().Select(t => new SelectListItem { Text = t.Type }),
                Topping2 = _pizzeria.GetToppings().Select(t => new SelectListItem { Text = t.Type }),
                Topping3 = _pizzeria.GetToppings().Select(t => new SelectListItem { Text = t.Type }),
                Quantities = quantities.Select(q => new SelectListItem { Text = q.ToString() })
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Service([FromServices] ApplicationDbContext context, EmployeeCreateOrderViewModel model, string submit, string check)
        {
            List<int> quantities = new List<int>();

            for (int i = 1; i <= 10; i++)
            {
                quantities.Add(i);
            }

            model.Sizes = _pizzeria.GetSizes().Select(s => new SelectListItem { Text = s.Type });
            model.Crusts = _pizzeria.GetCrusts().Select(c => new SelectListItem { Text = c.Type });
            model.Topping1 = _pizzeria.GetToppings().Select(t => new SelectListItem { Text = t.Type });
            model.Topping2 = _pizzeria.GetToppings().Select(t => new SelectListItem { Text = t.Type });
            model.Topping3 = _pizzeria.GetToppings().Select(t => new SelectListItem { Text = t.Type });
            model.Quantities = quantities.Select(q => new SelectListItem { Text = q.ToString() });
            model.EmployeeID = context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).EmployeeID;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else if (!string.IsNullOrEmpty(submit))
            {
                int confirmation = _order.CreateOrderConfirmation();

                // While the generated confirmation already exists, try to create another one.
                while (_order.GetOrderByConfirmation(_order.CreateOrderConfirmation()) != null)
                {
                    confirmation = _order.CreateOrderConfirmation();
                }

                model.ConfirmationNumber = confirmation;

                decimal subTotal = _order.CalculateSubTotalOrderCost(model);
                model.SubTotal = subTotal;

                TempData["Order"] = JsonConvert.SerializeObject(model);
                return RedirectToAction("Submission");
            }
            else
            {
                decimal subTotal = _order.CalculateSubTotalOrderCost(model);
                model.SubTotal = subTotal;

                return View(model);
            }
        }

        public IActionResult Submission()
        {
            EmployeeCreateOrderViewModel submission;

            try
            {
                submission = JsonConvert.DeserializeObject<EmployeeCreateOrderViewModel>(TempData["Order"].ToString());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Service");
            }

            var pizza = _order.GetPizza(submission);

            if (pizza == null)
            {
                _order.AddPizza(submission);
                pizza = _order.GetPizza(submission);
            }
            
            _order.SubmitInStoreOrder(submission, pizza, "Order Received", 1);

            var orderTime = _order.GetOrderByConfirmation(submission.ConfirmationNumber).OrderDate;

            try
            {
                // Convert the UTC time to local.
                orderTime.ToLocalTime();
            }
            catch (Exception ex) { }

            var model = new EmployeeOrderSubmissionViewModel
            {
                ConfirmationNumber = submission.ConfirmationNumber,
                Size = submission.PizzaSize,
                Crust = submission.PizzaCrust,
                Topping1 = submission.PizzaTopping1,
                Topping2 = submission.PizzaTopping2,
                Topping3 = submission.PizzaTopping3,
                Quantity = submission.PizzaQuantity,
                FirstName = submission.CustomerFirstName,
                LastName = submission.CustomerLastName,
                SubTotal = submission.SubTotal,
                Total = _order.CalculateOrderCost(pizza),
                EmployeeID = submission.EmployeeID,
                DateSubmitted = orderTime.ToString("MM/dd/yyyy"),
                TimeSubmitted = orderTime.ToString("hh:mm tt")
            };

            return View(model);
        }

        [Authorize("IsEmployee")]
        public IActionResult Statistics()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }

        public IActionResult History()
        {
            var orders = _orderHistory.GetAllOrdersByDefault();
           

            var model = new OrderHistoryViewModel
            {
                Orders = orders
            };

            return View(model);
        }

        public IActionResult Details(int confirmation)
        {

            return View(model);
        }
    }
}
