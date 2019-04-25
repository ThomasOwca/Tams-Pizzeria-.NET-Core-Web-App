using PizzeriaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TamsPizzeriaWebApp.Models.CreateOrderViewModels;
using TamsPizzeriaWebApp.Models.EmployeeViewModels;

namespace TamsPizzeriaWebApp.Services
{
    public interface IOrder
    {
        void SubmitOnlineOrder(CreateOrderViewModel model, Pizza pizza, string status, int store);
        void SubmitInStoreOrder(EmployeeCreateOrderViewModel model, Pizza pizza, string status, int store);
        Order GetOrderByConfirmation(int confirmation);
        IEnumerable<Order> GetOrdersByFullName(string firstName, string lastName);
        int GetOrderCount();
        int CreateOrderConfirmation();
        decimal CalculateSubTotalOrderCost(CreateOrderViewModel order);
        decimal CalculateSubTotalOrderCost(EmployeeCreateOrderViewModel order);
        decimal CalculateOrderCost(Pizza pizza);
        void AddPizza(CreateOrderViewModel model);
        void AddPizza(EmployeeCreateOrderViewModel model);
        Pizza GetPizza(CreateOrderViewModel model);
        Pizza GetPizza(EmployeeCreateOrderViewModel model);
    }
}
