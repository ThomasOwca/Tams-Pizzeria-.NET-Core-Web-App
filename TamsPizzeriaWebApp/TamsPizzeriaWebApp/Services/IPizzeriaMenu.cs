using PizzeriaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsPizzeriaWebApp.Services
{
    public interface IPizzeriaMenu
    {
        IEnumerable<Crust> GetCrusts();
        IEnumerable<Size> GetSizes();
        IEnumerable<Topping> GetToppings();
        IEnumerable<Status> GetStatuses();
    }
}
