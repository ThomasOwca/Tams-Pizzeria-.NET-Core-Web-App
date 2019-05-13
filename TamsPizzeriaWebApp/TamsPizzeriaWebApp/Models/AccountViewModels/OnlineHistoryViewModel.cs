using PizzeriaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsPizzeriaWebApp.Models.AccountViewModels
{
    public class OnlineHistoryViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
    }
}
