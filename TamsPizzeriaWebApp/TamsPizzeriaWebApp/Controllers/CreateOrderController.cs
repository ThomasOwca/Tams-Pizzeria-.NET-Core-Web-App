using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using PizzeriaData.Models;
using TamsPizzeriaWebApp.Data;
using TamsPizzeriaWebApp.Models;
using TamsPizzeriaWebApp.Models.CreateOrderViewModels;
using TamsPizzeriaWebApp.Services;

namespace TamsPizzeriaWebApp.Controllers
{
    [Authorize]
    public class CreateOrderController : Controller
    {
        private IPizzeriaMenu _pizzeriaMenu;
        private IOrder _order;

        public CreateOrderController(IPizzeriaMenu pizzeriaMenu, IOrder order)
        {
            _pizzeriaMenu = pizzeriaMenu;
            _order = order;
        }


        public IActionResult Index([FromServices] ApplicationDbContext db)
        {
            // Query the user from the Identity database.
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            // Pass the logged in user's first name into the view.
            ViewData["FirstName"] = user.FirstName;
            ViewData["LastName"] = user.LastName;

            return View();
        }

        public IActionResult Order([FromServices] ApplicationDbContext db)
        {
            List<int> quantities = new List<int>();

            var crusts = _pizzeriaMenu.GetCrusts().Select(c => new SelectListItem()
            {
                Text = c.Type
            });

            var sizes = _pizzeriaMenu.GetSizes().Select(c => new SelectListItem()
            {
                Text = c.Type
            });

            var toppings = _pizzeriaMenu.GetToppings().Select(c => new SelectListItem()
            {
                Text = c.Type
            });

            for (int i = 1; i <= 10; i++)
            {
                quantities.Add(i);
            }

            var quantityItems = quantities.Select(q => new SelectListItem()
            {
                Text = q.ToString()
            });

            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            var model = new CreateOrderViewModel
            {
                Crusts = crusts,
                Sizes = sizes,
                Topping1 = toppings,
                Topping2 = toppings,
                Topping3 = toppings,
                Quantities = quantityItems,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                UserEmailAddress = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Order(CreateOrderViewModel model, string submit, string check)
        {
            List<int> quantities = new List<int>();

            var sizes = _pizzeriaMenu.GetSizes().Select(c => new SelectListItem
            {
                Text = c.Type
            });

            var crusts = _pizzeriaMenu.GetCrusts().Select(c => new SelectListItem
            {
                Text = c.Type
            });

            var toppings = _pizzeriaMenu.GetToppings().Select(c => new SelectListItem
            {
                Text = c.Type
            });

            for (int i = 1; i <= 10; i++)
            {
                quantities.Add(i);
            }

            var quantityItems = quantities.Select(q => new SelectListItem()
            {
                Text = q.ToString()
            });

            int generateConfirmationNumber = _order.CreateOrderConfirmation();

            while (_order.GetOrderByConfirmation(generateConfirmationNumber) != null)
            {
                generateConfirmationNumber = _order.CreateOrderConfirmation();
            }

            model.Sizes = sizes;
            model.Crusts = crusts;
            model.Topping1 = toppings;
            model.Topping2 = toppings;
            model.Topping3 = toppings;
            model.Quantities = quantityItems;
            model.ConfirmationNumber = generateConfirmationNumber;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else if (!string.IsNullOrEmpty(submit))
            {
                TempData["SubmittedOrder"] = JsonConvert.SerializeObject(model);
                return RedirectToAction("Confirmation", new { id = model.ConfirmationNumber });
            }
            else
            {   
                model.SubTotal = _order.CalculateSubTotalOrderCost(model);
                return View(model);
            }
        }

        public IActionResult Confirmation(int id, [FromServices] ApplicationDbContext db)
        {
            // Query the user from the Identity database.
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            CreateOrderViewModel submittedOrder = null;

            try
            {
                submittedOrder = JsonConvert.DeserializeObject<CreateOrderViewModel>(TempData["SubmittedOrder"].ToString());
            }
            catch (Exception ex)
            {
                var order = _order.GetOrderByConfirmation(id);

                try
                {
                    submittedOrder = new CreateOrderViewModel
                    {
                        ConfirmationNumber = id,
                        PizzaCrust = order.Pizza.Crust.Type,
                        PizzaQuantity = order.Pizza.Quantity,
                        PizzaSize = order.Pizza.Size.Type,
                        PizzaTopping1 = order.Pizza.Topping1,
                        PizzaTopping2 = order.Pizza.Topping2,
                        PizzaTopping3 = order.Pizza.Topping3,
                        UserFirstName = order.FirstName,
                        UserLastName = order.LastName,
                    };
                }
                catch (Exception ex2)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            if (user.FirstName != submittedOrder.UserFirstName || user.LastName != submittedOrder.UserLastName)
            {
                return RedirectToAction("Index", "Home");
            }

            _order.AddPizza(submittedOrder);
            var pizza = _order.GetPizza(submittedOrder);

            var model = new OrderConfirmationViewModel
            {
                UserFirstName = submittedOrder.UserFirstName,
                UserLastName = submittedOrder.UserLastName,
                PickUpLocation = "Main Location",
                Quantity = submittedOrder.PizzaQuantity,
                PizzaSize = submittedOrder.PizzaSize,
                PizzaCrust = submittedOrder.PizzaCrust,
                Topping1 = submittedOrder.PizzaTopping1,
                Topping2 = submittedOrder.PizzaTopping2,
                Topping3 = submittedOrder.PizzaTopping3,
                SubTotal = pizza.SubTotal,
                Total = _order.CalculateOrderCost(pizza),
                ConfirmationNumber = id
            };

            if (_order.GetOrderByConfirmation(id) == null)
                _order.SubmitOnlineOrder(submittedOrder, pizza, "Order Received", 1);

            var orderDateTime = _order.GetOrderByConfirmation(id).OrderDate;

            try
            {
                // Convert the UTC time to local.
                orderDateTime.ToLocalTime();
                model.DateSubmitted = orderDateTime.ToString("MM/dd/yyyy");
                model.TimeSubmitted = orderDateTime.ToString("hh:mm tt");
            }
            catch (Exception ex) { }

            return View(model);
        }
    }
}