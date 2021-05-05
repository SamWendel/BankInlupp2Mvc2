using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankInlupp2Mvc2.Data;
using BankInlupp2Mvc2.ViewModels;
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

            if (sortField == "CustomerID")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.CustomerId);
                else
                    query = query.OrderByDescending(y => y.CustomerId);
            }

            if (sortField == "City")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(y => y.City);
                else
                    query = query.OrderByDescending(y => y.City);
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

        public IActionResult CustomerSearch(string q)
        {
            var viewModel = new CustomerSearchViewModel();

            viewModel.ResultList = _dbContext.Customers
                .Where(r => q == null || r.Givenname.Contains(q) || r.Surname.Contains(q) || r.City.Contains(q))
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

        public IActionResult CustomerDetails([FromRoute]int id)
        {
            var viewModel = new CustomerDetailsViewModel();
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

           }).ToList();

            return View(viewModel);
        }
    }
}