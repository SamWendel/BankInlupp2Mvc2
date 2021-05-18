using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankInlupp2Mvc2.Data;
using BankInlupp2Mvc2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankInlupp2Mvc2.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomerController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Admin, Cashier")]
        public IActionResult CustomerList(string q, string sortField, string sortOrder, int page = 1)
        {
            var viewModel = new CustomerListViewModel();

            var query = _dbContext.Customers
                .Where(r => q == null || r.Givenname.Contains(q) || r.Surname.Contains(q));

            if (string.IsNullOrEmpty(sortField))
                sortField = "Givenname";
            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "asc";

            if (sortField == "Givenname")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.Givenname);
                else
                    query = query.OrderByDescending(y => y.Givenname);
            }


            if (sortField == "CustomerID")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.CustomerId);
                else
                    query = query.OrderByDescending(y => y.CustomerId);
            }

            //if (sortField == "AccountID")
            //{
            //    if (sortOrder == "asc")
            //        query = query.OrderBy(y => y.AccountId);
            //    else
            //        query = query.OrderByDescending(y => y.AccountId);
            //}

            if (sortField == "City")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.City);
                else
                    query = query.OrderByDescending(y => y.City);
            }

            if (sortField == "Streetaddress")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.Streetaddress);
                else
                    query = query.OrderByDescending(y => y.Streetaddress);

            }

            if (sortField == "Birthday")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.Birthday);
                else
                    query = query.OrderByDescending(y => y.Birthday);
            }



            int pageSize = 30;
            int howManyItemsToSkip = (page - 1) * pageSize;
            query = query.Skip(howManyItemsToSkip).Take(pageSize);

            viewModel.Customers = query
                .Select(customer => new CustomerViewModel
                {
                    CustomerId = customer.CustomerId,
                    Gender = customer.Gender,
                    Givenname = customer.Givenname,
                    Surname = customer.Surname,
                    Streetaddress = customer.Streetaddress,
                    City = customer.City,
                    Zipcode = customer.Zipcode,
                    Country = customer.Country,
                    CountryCode = customer.CountryCode,
                    Birthday = customer.Birthday,
                    NationalId = customer.NationalId,
                    Telephonecountrycode = customer.Telephonecountrycode,
                    Telephonenumber = customer.Telephonenumber,
                    Emailaddress = customer.Emailaddress
                }).ToList();

            viewModel.q = q;
            viewModel.SortOrder = sortOrder;
            viewModel.SortField = sortField;
            viewModel.Page = page;
            viewModel.OppositeSortOrder = sortOrder == "asc" ? "desc" : "asc";

            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Cashier")]
        public IActionResult CustomerSearch(string q)
        {
            var viewModel = new CustomerSearchViewModel();

            viewModel.ResultList = _dbContext.Customers
                .Where(r => q == null || r.Givenname.Contains(q) || r.Surname.Contains(q) || r.City.Contains(q) || r.CustomerId.ToString().Equals(q))
                .Select(customer => new CustomerViewModel()
                {
                    CustomerId = customer.CustomerId,
                    Gender = customer.Gender,
                    Givenname = customer.Givenname,
                    Surname = customer.Surname,
                    Streetaddress = customer.Streetaddress,
                    City = customer.City,
                    Zipcode = customer.Zipcode,
                    Country = customer.Country,
                    CountryCode = customer.CountryCode,
                    Birthday = customer.Birthday,
                    NationalId = customer.NationalId,
                    Telephonecountrycode = customer.Telephonecountrycode,
                    Telephonenumber = customer.Telephonenumber,
                    Emailaddress = customer.Emailaddress
                }).ToList();
            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Cashier")]
        public IActionResult CustomerDetails([FromRoute]int id)
        {
            var viewModel = new CustomerDetailsViewModel();
            var disposition = _dbContext.Dispositions.FirstOrDefault(r => r.CustomerId == id);
            var accounts = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == disposition.AccountId);

            var accountId = disposition.AccountId;
            var created = accounts.Created;
            decimal accountBalance = 0;
            if (disposition != null)
            {
                var account = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == disposition.AccountId);
                if (account != null) 
                {
                    accountBalance = account.Balance;
                    accountId = account.AccountId;
                }
            }

            viewModel.Customers = _dbContext.Customers
           .Where(r => r.CustomerId == id)
           .Select(customer => new CustomerViewModel
           {
               CustomerId = customer.CustomerId,
               Gender = customer.Gender,
               Givenname = customer.Givenname,
               Surname = customer.Surname,
               Streetaddress = customer.Streetaddress,
               City = customer.City,
               Zipcode = customer.Zipcode,
               Country = customer.Country,
               CountryCode = customer.CountryCode,
               Birthday = customer.Birthday,
               NationalId = customer.NationalId,
               Telephonecountrycode = customer.Telephonecountrycode,
               Telephonenumber = customer.Telephonenumber,
               Emailaddress = customer.Emailaddress,
               Balance = accountBalance,
               AccountId = accountId,
               Created = created
           }).ToList();

            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Cashier")]
        public IActionResult CustomerTransactionHistory([FromRoute]int id, string q, string sortField, string sortOrder)
        {
            var viewModel = new CustomerTransactionHistoryListViewModel();
            var query = _dbContext.Transactions
               .Where(r => q == null);

            if (string.IsNullOrEmpty(sortField))
                sortField = "TransactionId";
            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "desc";

            if (sortField == "TransactionId")
            { 
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.TransactionId);
                else
                    query = query.OrderByDescending(y => y.TransactionId);
            }

            if (sortField == "Amount")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.Amount);
                else
                    query = query.OrderByDescending(y => y.Amount);

            }

            if (sortField == "Date")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.Date);
                else
                    query = query.OrderByDescending(y => y.Date);
            }


            if (sortField == "Balance")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.Balance);
                else
                    query = query.OrderByDescending(y => y.Balance);
            }

            if (sortField == "Operation")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.Operation);
                else
                    query = query.OrderByDescending(y => y.Operation);
            }


            viewModel.TransactionList = query
                .Where(r => r.AccountId == id)
                .Select(transactions => new CustomerTransactionHistoryViewModel
                {
                    AccountId = id,
                    TransactionId = transactions.TransactionId,
                    Date = transactions.Date,
                    Type = transactions.Type,
                    Operation = transactions.Operation,
                    Amount = transactions.Amount,
                    Balance = transactions.Balance,
                    Symbol = transactions.Symbol,
                    Bank = transactions.Bank, 
                    Account = transactions.Account
                }).ToList();

            viewModel.q = q;
            viewModel.SortOrder = sortOrder;
            viewModel.SortField = sortField;
            viewModel.OppositeSortOrder = sortOrder == "asc" ? "desc" : "asc";

            return View(viewModel);
        }
    }
}