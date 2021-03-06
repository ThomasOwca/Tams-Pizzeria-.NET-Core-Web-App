﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.OData.Query.SemanticAst;
using PizzeriaData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TamsPizzeriaWebApp.Models.CreateOrderViewModels
{
    public class CreateOrderViewModel
    {
        public IEnumerable<SelectListItem> Sizes { get; set; }

        public IEnumerable<SelectListItem> Crusts { get; set; }
        public IEnumerable<SelectListItem> Topping1 { get; set; }
        public IEnumerable<SelectListItem> Topping2 { get; set; }
        public IEnumerable<SelectListItem> Topping3 { get; set; }
        public IEnumerable<SelectListItem> Quantities { get; set; }

        [Required]
        [Display(Name = "Pizza Size")]
        public string PizzaSize { get; set; }

        [Required]
        [Display(Name = "Pizza Crust")]
        public string PizzaCrust { get; set; }

        [Required]
        [Display(Name = "First Topping")]
        public string PizzaTopping1 { get; set; }

        [Required]
        [Display(Name = "Second Topping")]
        public string PizzaTopping2 { get; set; }

        [Required]
        [Display(Name = "Third Topping")]
        public string PizzaTopping3 { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int PizzaQuantity { get; set; }

        public string UserEmailAddress { get; set; }

        [Required]
        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        [Required]
        public int ConfirmationNumber { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public int CustomerID { get; set; }
    }
}
