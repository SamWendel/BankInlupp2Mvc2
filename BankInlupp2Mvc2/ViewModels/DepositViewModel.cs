using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankInlupp2Mvc2.ViewModels
{
    public class DepositViewModel
    {
        [Required]
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string Error { get; set; }
    }
}
