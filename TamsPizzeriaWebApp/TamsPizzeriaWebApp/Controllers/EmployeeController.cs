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

        public IActionResult Details(int id)
        {
            var order = _orderHistory.GetOrderByConfirmation(id);

            var model = new OrderHistoryDetailsViewModel
            {
                ID = order.Id,
                Confirmation = order.Confirmation,
                FinalTotal = order.Total,
                SubTotal = order.Pizza.SubTotal,
                FirstName = order.FirstName,
                LastName = order.LastName,
                FulFilledByID = order.FulFilledById,
                OnlineOrder = order.OnlineOrder,
                OrderDate = order.OrderDate,
                Quantity = order.Pizza.Quantity,
                Size = order.Pizza.Size.Type,
                Crust = order.Pizza.Crust.Type,
                Topping1 = order.Pizza.Topping1,
                Topping2 = order.Pizza.Topping2,
                Topping3 = order.Pizza.Topping3,
                Status = order.Status.Type,
                StoreID = order.StorePickup.Id,
                StoreStreetAddress = order.StorePickup.StreetAddress,
                StoreCity = order.StorePickup.City,
                StoreState = order.StorePickup.State,
                StoreZipCode = Convert.ToInt32(order.StorePickup.ZipCode)
            };

            return View(model);
        }


        public IActionResult Update(int id)
        {
            var order = _orderHistory.GetOrderByConfirmation(id);

            var crusts = _pizzeria.GetCrusts().Select(crust => new SelectListItem
            {
                Text = crust.Type
            });

            var sizes = _pizzeria.GetSizes().Select(size => new SelectListItem
            {
                Text = size.Type
            });

            var toppings = _pizzeria.GetToppings().Select(topping => new SelectListItem
            {
                Text = topping.Type
            });

            List<int> numbers = new List<int>();

            for (int i = 1; i < 11; i++)
            {
                numbers.Add(i);
            }

            var quantities = numbers.Select(number => new SelectListItem
            {
                Text = number.ToString()
            });

            var statuses = _pizzeria.GetStatuses().Select(status => new SelectListItem
            {
                Text = status.Type.ToString()
            });

            var model = new OrderHistoryUpdateViewModel
            {
                ConfirmationNumber = order.Confirmation,
                CustomerFirstName = order.FirstName,
                CustomerLastName = order.LastName,
                PizzaCrust = order.Pizza.Crust.Type,
                PizzaSize = order.Pizza.Size.Type,
                PizzaTopping1 = order.Pizza.Topping1,
                PizzaTopping2 = order.Pizza.Topping2,
                PizzaTopping3 = order.Pizza.Topping3,
                Crusts = crusts,
                Sizes = sizes,
                Topping1 = toppings,
                Topping2 = toppings,
                Topping3 = toppings,
                Quantities = quantities,
                PizzaQuantity = order.Pizza.Quantity,
                Statuses = statuses,
                SubTotal = order.Pizza.SubTotal,
                Total = order.Total,
                Status = order.Status.Type
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateOrder(OrderHistoryUpdateViewModel model, int id)
        {
            var order = _orderHistory.GetOrderByConfirmation(id);
            var pizza = _orderHistory.GetPizza(order);

            pizza.Size = _orderHistory.GetSize(model.PizzaSize);
            pizza.Crust = _orderHistory.GetCrust(model.PizzaCrust);
            pizza.Topping1 = model.PizzaTopping1;
            pizza.Topping2 = model.PizzaTopping2;
            pizza.Topping3 = model.PizzaTopping3;
            pizza.Quantity = model.PizzaQuantity;

            order.Pizza = pizza;
            order.Status = _orderHistory.GetStatus(model.Status);

            _orderHistory.UpdatePizza(pizza, order.Status, pizza.Id, id);

            return RedirectToAction("Details", new { id });
        }

    }
}
