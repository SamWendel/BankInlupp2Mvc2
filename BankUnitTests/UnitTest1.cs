using BankInlupp2Mvc2.Controllers;
using BankInlupp2Mvc2.Data;
using BankInlupp2Mvc2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;

namespace BankUnitTests
{
    public class Tests
    {
        private ApplicationDbContext ctx;
        private AccountController sut;

        public Tests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(new Guid().ToString())
                 .EnableSensitiveDataLogging()
                 .Options;
            ctx = new ApplicationDbContext(dbContextOptions);
            sut = new AccountController(ctx);
            var account1 = new Accounts
            {
                AccountId = 1,
                Frequency = "Monthly",
                Created = DateTime.Now,
                Balance = 2000
            };
            var account2 = new Accounts
            {
                AccountId = 2,
                Frequency = "Monthly",
                Created = DateTime.Now,
                Balance = 2000
            };

            ctx.Add(account1);
            ctx.Add(account2);
            ctx.SaveChanges();
        }

        [Test]
        public void ValidateAWithdrawal()
        {
            var viewModel = new WithdrawViewModel
            {
                Amount = 20,
                Balance = 2000,
                Error = "Invalid amount"
            };
            var actionResult = sut.Withdraw(viewModel, 1, 20);
            var viewResult = actionResult as RedirectToActionResult;
            Assert.AreEqual("AccountIndex", viewResult.ActionName);
        }
        [Test]
        public void ValidateADeposit()
        {
            var viewModel = new DepositViewModel
            {
                Amount = 20,
                Balance = 2000,
                Error = "Invalid amount"
            };
            var actionResult = sut.Deposit(viewModel, 1, 20);
            var viewResult = actionResult as RedirectToActionResult;
            Assert.AreEqual("AccountIndex", viewResult.ActionName);
        }

        [Test]
        public void ValidateATransaction()
        {
            var viewModel = new TransactionViewModel
            {
                Amount = 20,
                Balance = 2000,
                Error = "Invalid amount or AccountID",
                RecieverId = 2
            };
            var actionResult = sut.Transaction(viewModel, 1, 2, 20);
            var viewResult = actionResult as RedirectToActionResult;
            Assert.AreEqual("AccountIndex", viewResult.ActionName);
        }

        [Test]
        public void ValidatexOverdrawnTransaction()
        {
            var viewModel = new TransactionViewModel
            {
                Amount = 2001,
                Balance = 2000,
                Error = "Invalid amount or AccountID",
                RecieverId = 2
            };
            var actionResult = sut.Transaction(viewModel, 1, 2, 2001);
            var viewResult = actionResult as ViewResult;
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
        }

        [Test]
        public void ValidatexOverdrawnWithdrawal()
        {
            var viewModel = new WithdrawViewModel
            {
                Amount = 2001,
                Balance = 2000,
                Error = "Invalid amount"
            };
            var actionResult = sut.Withdraw(viewModel, 1, 2001);
            var viewResult = actionResult as ViewResult;
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
        }

        [Test]
        public void ValidatexNegativeWithdrawal()
        {
            var viewModel = new WithdrawViewModel
            {
                Amount = -20,
                Balance = 2000,
                Error = "Invalid amount"
            };
            var actionResult = sut.Withdraw(viewModel, 1, -20);
            var viewResult = actionResult as ViewResult;
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
        }


        [Test]
        public void ValidatexNegativeDeposit()
        {
            var viewModel = new DepositViewModel
            {
                Amount = -20,
                Balance = 2000,
                Error = "Invalid amount"
            };
            var actionResult = sut.Deposit(viewModel, 1, -20);
            var viewResult = actionResult as ViewResult;
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
        }
    }
}