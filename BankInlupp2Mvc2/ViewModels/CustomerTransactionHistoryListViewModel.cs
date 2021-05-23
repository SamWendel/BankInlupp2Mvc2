using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankInlupp2Mvc2.ViewModels
{
    public class CustomerTransactionHistoryListViewModel
    {
        public List<CustomerTransactionHistoryViewModel> TransactionList = new List<CustomerTransactionHistoryViewModel>();
        public int AccountId { get; set; }
        public string q { get; internal set; }
    }
}
