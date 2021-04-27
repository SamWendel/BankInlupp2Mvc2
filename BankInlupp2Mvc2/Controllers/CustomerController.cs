using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankInlupp2Mvc2.Data;
using BankInlupp2Mvc2.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankInlupp2Mvc2.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomerController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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

            if (sortField == "Surname")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.Surname);
                else
                    query = query.OrderByDescending(y => y.Surname);

            }


            int pageSize = 20;
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
    }
}