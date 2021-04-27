using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankInlupp2Mvc2.ViewModels
{
    public class CustomerListViewModel
    {
        public List<CustomerViewModel> Customers { get; set; }
        public string q { get; internal set; }
        public string SortOrder { get; internal set; }
        public string SortField { get; internal set; }
        public string OppositeSortOrder { get; internal set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}
