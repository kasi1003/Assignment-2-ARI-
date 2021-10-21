using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromotionsWebApp.Core.Interfaces;
using PromotionsWebApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Controllers
{
    public class StaffController:Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _repo;
        private readonly ILogger<StaffController> _logger;
        private readonly IEmailSender _emailSender;
        public StaffController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<StaffController> logger, IUserRepository repo,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
