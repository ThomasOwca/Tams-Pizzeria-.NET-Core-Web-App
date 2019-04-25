using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzeriaData.Models;
using TamsPizzeriaWebApp.Data;

namespace TamsPizzeriaWebApp.Services
{
    public class PizzeriaMenu : IPizzeriaMenu
    {
        private ApplicationDbContext _context;

        public PizzeriaMenu(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Crust> GetCrusts()
        {
            return _context.Crusts;
        }

        public IEnumerable<Size> GetSizes()
        {
            return _context.Sizes;
        }

        public IEnumerable<Topping> GetToppings()
        {
            return _context.Toppings;
        }
    }
}
