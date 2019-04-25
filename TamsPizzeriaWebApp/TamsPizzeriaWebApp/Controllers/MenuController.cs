using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TamsPizzeriaWebApp.Models.MenuViewModels;
using TamsPizzeriaWebApp.Services;

namespace TamsPizzeriaWebApp.Controllers
{
    public class MenuController : Controller
    {
        private IPizzeriaMenu _pizzeriaMenu;

        public MenuController(IPizzeriaMenu pizzeriaMenu)
        {
            _pizzeriaMenu = pizzeriaMenu;
        }

        public object OurPizzasViewModel { get; private set; }

        public IActionResult Index()
        {
            OurMenuViewModel model = new OurMenuViewModel
            {
                Crusts = _pizzeriaMenu.GetCrusts(),
                Sizes = _pizzeriaMenu.GetSizes(),
                Toppings = _pizzeriaMenu.GetToppings()
            };

            return View(model);
        }
    }
}