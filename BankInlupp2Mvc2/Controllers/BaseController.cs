using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankInlupp2Mvc2.Data;
using Microsoft.AspNetCore.Mvc;

namespace BankInlupp2Mvc2.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _dbContext;

        public BaseController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}