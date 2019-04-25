using PizzeriaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsPizzeriaWebApp.Models.MenuViewModels
{
    public class OurMenuViewModel
    {
        public IEnumerable<Size> Sizes { get; set; }
        public IEnumerable<Crust> Crusts { get; set; }
        public IEnumerable<Topping> Toppings { get; set; }
    }
}
