using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzeriaData.Models;
using TamsPizzeriaWebApp.Data;
using TamsPizzeriaWebApp.Models.CreateOrderViewModels;
using TamsPizzeriaWebApp.Models.EmployeeViewModels;

namespace TamsPizzeriaWebApp.Services
{
    public class Ordering : IOrder
    {
        private ApplicationDbContext _context;

        public Ordering(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CreateOrderConfirmation()
        {
            Random random = new Random();
            return random.Next(100, 9999999);
        }

        public Order GetOrderByConfirmation(int confirmation)
        {
            var order = _context.Orders
                .Include(c => c.Pizza.Crust)
                .Include(c => c.Pizza.Size)
                .Include(c => c.Status)
                .FirstOrDefault(c => c.Confirmation == confirmation);

            return order;
        }

        public int GetOrderCount()
        {
            return _context.Orders.Count();
        }

        public IEnumerable<Order> GetOrdersByFullName(string firstName, string lastName)
        {
            var orders = _context.Orders
                .Where(n => n.FirstName == firstName)
                .Where(n => n.LastName == lastName)
                .OrderByDescending(n => n.OrderDate);

            return orders;
        }

        public void SubmitInStoreOrder(EmployeeCreateOrderViewModel model, Pizza pizza, string status, int store)
        {
            decimal total = CalculateOrderCost(pizza);

            var order = new Order
            {
                Confirmation = model.ConfirmationNumber,
                FirstName = model.CustomerFirstName,
                LastName = model.CustomerLastName,
                OnlineOrder = false,
                OrderDate = DateTime.Now,
                Pizza = pizza,
                Status = GetStatus(status),
                StorePickup = GetStorePickup(store),
                Total = total,
                FulFilledById = model.EmployeeID
            };

            _context.Add(order);
            _context.SaveChanges();
        }

        public void SubmitOnlineOrder(CreateOrderViewModel model, Pizza pizza, string status, int store)
        {
            decimal total = CalculateOrderCost(pizza);

            var order = new Order
            {
                Confirmation = model.ConfirmationNumber,
                FirstName = model.UserFirstName,
                LastName = model.UserLastName,
                OnlineOrder = true,
                OrderDate = DateTime.Now,
                Pizza = pizza,
                Status = GetStatus(status),
                StorePickup = GetStorePickup(store),
                Total = total,
                CustomerID = model.CustomerID
            };

            _context.Add(order);
            _context.SaveChanges();
        }

        public decimal CalculateSubTotalOrderCost(CreateOrderViewModel order)
        {
            decimal subTotal = 0.0m;

            var sizeCost = GetSizeCost(order.PizzaSize);
            var crustCost = GetCrustCost(order.PizzaCrust);
            var topping1Cost = GetToppingCost(order.PizzaTopping1);
            var topping2Cost = GetToppingCost(order.PizzaTopping2);
            var topping3Cost = GetToppingCost(order.PizzaTopping3);

            subTotal = sizeCost + crustCost + topping1Cost + topping2Cost + topping3Cost;
            subTotal *= order.PizzaQuantity;

            return subTotal;
        }

        public decimal CalculateSubTotalOrderCost(EmployeeCreateOrderViewModel order)
        {
            decimal subTotal = 0.0m;

            var sizeCost = GetSizeCost(order.PizzaSize);
            var crustCost = GetCrustCost(order.PizzaCrust);
            var topping1Cost = GetToppingCost(order.PizzaTopping1);
            var topping2Cost = GetToppingCost(order.PizzaTopping2);
            var topping3Cost = GetToppingCost(order.PizzaTopping3);

            subTotal = sizeCost + crustCost + topping1Cost + topping2Cost + topping3Cost;
            subTotal *= order.PizzaQuantity;

            return subTotal;
        }

        // Helper method for calculating the total order cost.
        public decimal CalculateOrderCost(Pizza pizza)
        {
            decimal total = 0.0m;

            // TO-DO Implement the salesTax into the total cost.
            decimal salesTax = 0.08m;

            // TO-DO Business Logic for calculating cost of customer's pizza order.

            var sizeCost = pizza.Size.Cost;
            var crustCost = pizza.Crust.Cost;

            var topping1Cost = GetToppingCost(pizza.Topping1);
            var topping2Cost = GetToppingCost(pizza.Topping2);
            var topping3Cost = GetToppingCost(pizza.Topping3);

            total = sizeCost + crustCost + topping1Cost + topping2Cost + topping3Cost;
            total *= pizza.Quantity;

            salesTax *= total;

            return total + salesTax;
        }

        public void AddPizza(CreateOrderViewModel model)
        {
            Pizza pizza = new Pizza
            {
                Size = GetSize(model.PizzaSize),
                Crust = GetCrust(model.PizzaCrust),
                Quantity = model.PizzaQuantity,
                Topping1 = model.PizzaTopping1,
                Topping2 = model.PizzaTopping2,
                Topping3 = model.PizzaTopping3
            };

            decimal subTotal = CalculateSubTotalOrderCost(model);

            pizza.SubTotal = subTotal;

            // If this exact Pizza combination exists in the database, don't add. Else, add it to the database.
            if (GetPizza(model) != null)
                return;
            else
            {
                _context.Add(pizza);
                _context.SaveChanges();
            }
        }

        public void AddPizza(EmployeeCreateOrderViewModel model)
        {
            Pizza pizza = new Pizza
            {
                Size = GetSize(model.PizzaSize),
                Crust = GetCrust(model.PizzaCrust),
                Quantity = model.PizzaQuantity,
                Topping1 = model.PizzaTopping1,
                Topping2 = model.PizzaTopping2,
                Topping3 = model.PizzaTopping3
            };

            decimal subTotal = CalculateSubTotalOrderCost(model);

            pizza.SubTotal = subTotal;

            // If this exact Pizza combination exists in the database, don't add. Else, add it to the database.
            if (GetPizza(model) != null)
                return;
            else
            {
                _context.Add(pizza);
                _context.SaveChanges();
            }
        }

        public Pizza GetPizza(CreateOrderViewModel model)
        {
            var pizza = _context.Pizzas
                .Where(p => p.Size == GetSize(model.PizzaSize))
                .Where(p => p.Crust == GetCrust(model.PizzaCrust))
                .Where(p => p.Quantity == model.PizzaQuantity)
                .Where(p => p.Topping1 == model.PizzaTopping1)
                .Where(p => p.Topping2 == model.PizzaTopping2)
                .Where(p => p.Topping3 == model.PizzaTopping3)
                .Where(p => p.SubTotal == CalculateSubTotalOrderCost(model))
                .FirstOrDefault();

            if (pizza != null)
                return pizza;
            else
                return null;
        }

        public Pizza GetPizza(EmployeeCreateOrderViewModel model)
        {
            var pizza = _context.Pizzas
                .Where(p => p.Size == GetSize(model.PizzaSize))
                .Where(p => p.Crust == GetCrust(model.PizzaCrust))
                .Where(p => p.Quantity == model.PizzaQuantity)
                .Where(p => p.Topping1 == model.PizzaTopping1)
                .Where(p => p.Topping2 == model.PizzaTopping2)
                .Where(p => p.Topping3 == model.PizzaTopping3)
                .Where(p => p.SubTotal == CalculateSubTotalOrderCost(model))
                .FirstOrDefault();

            if (pizza != null)
                return pizza;
            else
                return null;
        }

        private Store GetStorePickup(int storeId)
        {
            return _context.Stores
                .FirstOrDefault(s => s.Id == storeId);
        }

        private Status GetStatus(string type)
        {
            return _context.Statuses
                .FirstOrDefault(s => s.Type == type);
        }

        private decimal GetSizeCost(string type)
        {
            return _context.Sizes
                .FirstOrDefault(s => s.Type == type)
                .Cost;
        }

        private decimal GetCrustCost(string type)
        {
            return _context.Crusts
                .FirstOrDefault(c => c.Type == type)
                .Cost;
        }

        private decimal GetToppingCost(string type)
        {
            return _context.Toppings
                .FirstOrDefault(t => t.Type == type)
                .Cost;
        }

        private Crust GetCrust(string type)
        {
            return _context.Crusts
                .FirstOrDefault(c => c.Type == type);
        }

        private Size GetSize(string type)
        {
            return _context.Sizes
                .First(s => s.Type == type);
        }

        private Topping GetTopping(string type)
        {
            return _context.Toppings
                .First(t => t.Type == type);
        }
    }
}
