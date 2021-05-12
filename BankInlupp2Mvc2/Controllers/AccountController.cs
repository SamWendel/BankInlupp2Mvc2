using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankInlupp2Mvc2.Data;
using BankInlupp2Mvc2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankInlupp2Mvc2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Admin, Cashier")]
        public IActionResult AccountIndex()
        {
            var viewModel = new AccountIndexViewModel();

            viewModel.TotalAccounts = _dbContext.Accounts.Count();
            viewModel.TotalCustomers = _dbContext.Customers.Count();
            viewModel.TotalBalance = _dbContext.Accounts.Sum(r => r.Balance);

            return View(viewModel);
        }
    }
}