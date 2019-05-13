using PizzeriaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsPizzeriaWebApp.Services
{
    public interface IOrderHistory
    {
        // Different varieties on getting a list of orders at the business.
        IEnumerable<Order> GetAllOrdersByDefault();
        IEnumerable<Order> GetAllOrdersByEmployee(int id);
        IEnumerable<Order> GetAllOrdersByCustomerLastName(string lastName);
        IEnumerable<Order> GetAllOrdersByStatus(int id);
        IEnumerable<Order> GetAllOrdersByPizzaId(int id);
        IEnumerable<Order> GetAllOrdersByStorePickupId(int id);
        IEnumerable<Order> GetAllOrdersByOnlineCustomerID(int id);

        // Two different ways to get one specific order from history.
        Order GetOrderByConfirmation(int confirmation);
        Order GetOrderById(int id);

        // Delete and update operations for existing orders in history.
        void DeleteOrderById(int id);
        void DeleteOrderByConfirmation(int id);
        void UpdateOrder(int confirmationNumber, Order order);
        void UpdatePizza(Pizza pizza, Status status, int id, int confirmation);

        // Get various business statistics for managers/admins.
        decimal GetTotalRevenueBySubTotal();
        decimal GetTotalRevenueByFinalTotal();

        int GetTotalOrdersCreatedAtCompany();
        int GetTotalOrdersCreatedAtStore(int id);
        Crust GetTopCrust();

        Store GetStorePickup(int storeId);
        Status GetStatus(string type);
        decimal GetSizeCost(string type);
        decimal GetCrustCost(string type);
        decimal GetToppingCost(string type);
        Crust GetCrust(string type);
        Size GetSize(string type);
        Topping GetTopping(string type);
        Pizza GetPizza(Order order);
        Pizza GetPizza(int pizzaID);
    }
}
