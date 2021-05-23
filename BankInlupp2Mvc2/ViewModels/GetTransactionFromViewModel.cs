using System;
using System.Collections.Generic;

namespace BankInlupp2Mvc2.ViewModels
{
    public class GetTransactionFromViewModel
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Operation { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string Symbol { get; set; }
        public string Bank { get; set; }
        public string Account { get; set; }
        public List<GetTransactionFromViewModel> TransactionList { get; set; } = new List<GetTransactionFromViewModel>();
    }
}