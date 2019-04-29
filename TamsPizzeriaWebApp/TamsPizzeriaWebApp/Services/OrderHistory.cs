using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzeriaData.Models;
using TamsPizzeriaWebApp.Data;

namespace TamsPizzeriaWebApp.Services
{
    // TO-DO | Finish the implementation of all these service layer methods! 
    public class OrderHistory : IOrderHistory
    {
        private ApplicationDbContext _context;

        public OrderHistory(ApplicationDbContext context)
        {
            _context = context;
        }

        public void DeleteOrderByConfirmation(int confirmation)
        {
            var order = _context.Orders
                .FirstOrDefault(o => o.Confirmation == confirmation);

            _context.Remove(order);
            _context.SaveChanges();
        }

        public void DeleteOrderById(int id)
        {
            var order = _context.Orders
                .FirstOrDefault(o => o.Id == id);

            _context.Remove(order);
            _context.SaveChanges();
        }

        public IEnumerable<Order> GetAllOrdersByCustomerLastName(string lastName)
        {
            return _context.Orders
                .Include(o => o.Status)
                .Include(o => o.Pizza)
                .Include(o => o.StorePickup)
                .Where(o => o.LastName == lastName);
        }

        public IEnumerable<Order> GetAllOrdersByDefault()
        {
            return _context.Orders
                .Include(o => o.Status)
                .Include(o => o.Pizza)
                .Include(o => o.StorePickup)
                .OrderByDescending(o => o.OrderDate);
        }

        public IEnumerable<Order> GetAllOrdersByEmployee(int id)
        {
            return _context.Orders
                .Include(o => o.Status)
                .Include(o => o.Pizza)
                .Include(o => o.StorePickup)
                .Where(o => o.FulFilledById == id);
        }

        public IEnumerable<Order> GetAllOrdersByPizzaId(int id)
        {
            return _context.Orders
                .Include(o => o.Status)
                .Include(o => o.Pizza)
                .Include(o => o.StorePickup)
                .Where(o => o.Pizza.Id == id);
        }

        public IEnumerable<Order> GetAllOrdersByStatus(int id)
        {
            return _context.Orders
               .Include(o => o.Status)
               .Include(o => o.Pizza)
               .Include(o => o.StorePickup)
               .Where(o => o.Status.Id == id);
        }

        public IEnumerable<Order> GetAllOrdersByStorePickupId(int id)
        {
            return _context.Orders
                .Include(o => o.Status)
                .Include(o => o.Pizza)
                .Include(o => o.StorePickup)
                .Where(o => o.StorePickup.Id == id);
        }

        public Order GetOrderByConfirmation(int confirmation)
        {
            return _context.Orders
                .Include(o => o.Status)
                .Include(o => o.Pizza)
                .Include(o => o.StorePickup)
                .FirstOrDefault(o => o.Confirmation == confirmation);
        }

        public Order GetOrderById(int id)
        {
            return _context.Orders
                .Include(o => o.Status)
                .Include(o => o.Pizza)
                .Include(o => o.StorePickup)
                .FirstOrDefault(o => o.Id == id);
        }

        public Crust GetTopCrust()
        {
            return _context.Crusts.Max();
        }

        public int GetTotalOrdersCreatedAtCompany()
        {
            return _context.Orders.Count();
        }

        public int GetTotalOrdersCreatedAtStore(int id)
        {
            return _context.Orders
                .Where(s => s.StorePickup.Id == id).Count();
        }

        public decimal GetTotalRevenueByFinalTotal()
        {
            decimal total = 0;
            var orders = _context.Orders;

            // Iterate through the collection add and up the total 
            // to calculate total revenue made by "final" total;
            foreach (Order order in orders)
            {
                total += order.Total;
            }

            return total;
        }

        public decimal GetTotalRevenueBySubTotal()
        {
            decimal total = 0;

            // Have to use algebraic formula to get the subtotal from the given final total in the Orders table.
            // Using the S = T / (1 + r) formula, where S is subtotal, T is total, r is the tax rate.
            var orders = _context.Orders;

            foreach (Order order in orders)
            {
                decimal temp = order.Total / (1.0m + 0.07m);
                total += temp;
            }

            return total;
        }

        public void UpdateOrder(int confirmationNumber, Order order)
        {
            var trackedOrder = _context.Orders.FirstOrDefault(o => o.Confirmation == confirmationNumber);
            _context.Update(order);

            // Updates the order. Only the pizza, status, and total can be changed.
            trackedOrder.Pizza = order.Pizza;
            trackedOrder.Status = order.Status;
            trackedOrder.Total = order.Total;

            _context.SaveChanges();
        }
    }
}
