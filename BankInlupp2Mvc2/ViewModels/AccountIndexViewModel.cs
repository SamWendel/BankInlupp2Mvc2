using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankInlupp2Mvc2.ViewModels
{
    public class AccountIndexViewModel
    {
        public int TotalAccounts { get; set; }
        public decimal TotalBalance { get; set; }
        public int TotalCustomers { get; set; }
    }
}
