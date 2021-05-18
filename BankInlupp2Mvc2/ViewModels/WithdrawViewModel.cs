using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankInlupp2Mvc2.ViewModels
{
    public class WithdrawViewModel
    {
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string Error { get; set; }
    }
}
