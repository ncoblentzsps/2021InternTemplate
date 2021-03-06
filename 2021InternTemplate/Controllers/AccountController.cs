﻿using _2021InternTemplate.Models;
using _2021InternTemplate.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace _2021InternTemplate.Controllers
{    
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<MoneroUser> _userManager;
        private readonly SignInManager<MoneroUser> _loginManager;
        private readonly IMoneroEmailService _emailService;
        public AccountController(ILogger<AccountController> logger,
            UserManager<MoneroUser> userManager,
            SignInManager<MoneroUser> loginManager,
            ApplicationDBContext dbContext,
            IMoneroEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _loginManager = loginManager;
            _dbContext = dbContext;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            Wallet w = new Wallet() { Balance = 100 };
            _dbContext.Add(w);
            await _dbContext.SaveChangesAsync();

            WalletTransactions t = new WalletTransactions() { Name = "Initial", Amount = 100 };
            t.WalletId = w.Id;
            _dbContext.Add(t);
            await _dbContext.SaveChangesAsync();
            //w.Transactions = new List<WalletTransactions>();
            //w.Transactions.Add(t);
            
            w.Balance = 150;
            _dbContext.Update(w);            

            WalletTransactions transaction2 = new WalletTransactions() { Name = "Add Fifty", Amount = 50,WalletId=w.Id };
            _dbContext.Add(transaction2);
            await _dbContext.SaveChangesAsync();



            MoneroUser user = new MoneroUser()
            {
                Email = viewModel.Email,
                UserName = viewModel.Username,
                WalletId = w.Id
            };
            IdentityResult result = await _userManager.CreateAsync(user, viewModel.Password);


            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            MoneroUser user = await _userManager.FindByNameAsync(viewModel.Username);
            var result = await _loginManager.CheckPasswordSignInAsync(user, viewModel.Password,false);            

            if (result.Succeeded)
            {
                string code = new Random().Next(0, 999999).ToString("000000");
                _emailService.SendLoginCode(user.Email, code);
                user.Code = code;
                await _userManager.UpdateAsync(user);
                HttpContext.Session.SetString("UserId", user.Id);
                return RedirectToAction("ConfirmCode", "Account");
            }
            return View(viewModel);
        }

        
    }
}
