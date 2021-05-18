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
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        public IActionResult AccountIndex()
        {
            var viewModel = new AccountIndexViewModel();

            viewModel.TotalAccounts = _dbContext.Accounts.Count();
            viewModel.TotalCustomers = _dbContext.Customers.Count();
            viewModel.TotalBalance = _dbContext.Accounts.Sum(r => r.Balance);

            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Cashier")]
        public IActionResult Deposit([FromRoute] int id)
        {
            var viewModel = new DepositViewModel();
            var account = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == id);
            viewModel.Balance = account.Balance;

            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Cashier")]
        [HttpPost]
        public IActionResult Deposit(DepositViewModel viewModel, int id, decimal amount)
        {
            if (ModelState.IsValid && amount > 0)
            {
                var dbTransaction = new Transactions();
                _dbContext.Transactions.Add(dbTransaction);

                var account = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == id);
                var currentBalance = account.Balance;

                dbTransaction.AccountId = id;
                dbTransaction.Amount = amount;
                dbTransaction.Date = DateTime.Now;
                dbTransaction.Balance = currentBalance + amount;
                dbTransaction.Operation = "Deposit";
                dbTransaction.Type = "Credit";
                account.Balance = currentBalance + amount;
                _dbContext.SaveChanges();
                return RedirectToAction("AccountIndex", "Account");
            }

            var accounts = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == id);
            viewModel.Balance = accounts.Balance;
            ModelState.AddModelError("Error", "Invalid amount");
            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Cashier")]
        public IActionResult Transaction([FromRoute] int id)
        {
            var viewModel = new TransactionViewModel();
            var accounts = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == id);
            viewModel.Balance = accounts.Balance;

            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Cashier")]
        [HttpPost]
        public IActionResult Transaction([FromRoute] TransactionViewModel viewModel, int id, int recieverId, decimal amount)
        {
            var balanceCheck = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == id);

            if (ModelState.IsValid && amount > 0 && amount < balanceCheck.Balance)
            {
                var dbTransactionSender = new Transactions();
                var dbTransactionReciever = new Transactions();
                _dbContext.Transactions.Add(dbTransactionSender);
                _dbContext.Transactions.Add(dbTransactionReciever);

                var senderAccount = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == id);
                var currentSenderBalance = senderAccount.Balance;
                var recieverAccount = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == recieverId);
                var currentRecieverBalance = recieverAccount.Balance;

                dbTransactionSender.AccountId = id;
                dbTransactionSender.Amount = amount - (amount * 2);
                dbTransactionSender.Date = DateTime.Now;
                dbTransactionSender.Balance = currentSenderBalance - amount;
                dbTransactionSender.Operation = "Transfer to another account";
                dbTransactionSender.Type = "Credit";
                senderAccount.Balance = currentSenderBalance - amount;

                dbTransactionReciever.AccountId = recieverId;
                dbTransactionReciever.Amount = amount;
                dbTransactionReciever.Date = DateTime.Now;
                dbTransactionReciever.Balance = currentRecieverBalance + amount;
                dbTransactionReciever.Operation = "Transfer";
                dbTransactionReciever.Type = "Debit";
                recieverAccount.Balance = currentRecieverBalance + amount;

                _dbContext.SaveChanges();
                return RedirectToAction("AccountIndex", "Account");
            }
            var accounts = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == id);
            viewModel.Balance = accounts.Balance;
            ModelState.AddModelError("Error", "Invalid amount");
            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Cashier")]
        public IActionResult Withdraw([FromRoute] int id)
        {
            var viewModel = new WithdrawViewModel();
            var accounts = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == id);
            viewModel.Balance = accounts.Balance;

            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Cashier")]
        [HttpPost]
        public IActionResult Withdraw(WithdrawViewModel viewModel, int id, decimal amount)
        {
            var balanceCheck = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == id);

            if (ModelState.IsValid && amount > 0 && amount < balanceCheck.Balance)
            {
                var dbTransaction = new Transactions();
                _dbContext.Transactions.Add(dbTransaction);

                var account = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == id);
                var currentBalance = account.Balance;

                dbTransaction.AccountId = id;
                dbTransaction.Amount = amount - (amount * 2);
                dbTransaction.Date = DateTime.Now;
                dbTransaction.Balance = currentBalance - amount;
                dbTransaction.Operation = "Withdraw";
                dbTransaction.Type = "Debit";
                account.Balance = currentBalance - amount;
                _dbContext.SaveChanges();
                return RedirectToAction("AccountIndex", "Account");
            }

            var accounts = _dbContext.Accounts.FirstOrDefault(r => r.AccountId == id);
            viewModel.Balance = accounts.Balance;
            ModelState.AddModelError("Error", "Invalid amount");
            return View(viewModel);
        }
    }
}