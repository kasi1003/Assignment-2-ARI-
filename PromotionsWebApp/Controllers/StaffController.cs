using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PromotionsWebApp.Core.Interfaces;
using PromotionsWebApp.Domain.Entities;
using PromotionsWebApp.Models;
using PromotionsWebApp.Utilities;
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
        private readonly IUserRepository _userRepo;
        private readonly IRepository<Rank> _rankRepo;
        private readonly IRepository<Staff> _staffRepo;
        private readonly IRepository<StaffJob> _jobRepo;
        private readonly IRepository<Qualification> _qualificationRepo;
        private readonly ILogger<StaffController> _logger;
        private readonly IEmailSender _emailSender;
        public StaffController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<StaffController> logger, IUserRepository repo,
            IEmailSender emailSender, IRepository<Staff> staffRepo,
            IRepository<StaffJob> jobRepo, IRepository<Qualification> qualificationRepo,
            IRepository<Rank> rankRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _userRepo = repo;
            _staffRepo = staffRepo;
            _jobRepo = jobRepo;
            _rankRepo = rankRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string currentFilter = null,
                                                string searchString = null,
                                                int? pageNumber = null)
        {
            StaffsVM model = new StaffsVM();
            try
            {
                var staffList = await _staffRepo.AllIncluding(x => x.Jobs);
                foreach (Staff staff in staffList)
                {
                    staff.User = await _userManager.FindByIdAsync(staff.UserId);
                    staff.Jobs = await _jobRepo.FindByIncluding(x => x.StaffId == staff.Id, x => x.Rank);
                }

                if (searchString != null)
                {
                    pageNumber = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewData["CurrentFilter"] = searchString;
                if (!String.IsNullOrEmpty(searchString))
                {
                    staffList = staffList.Where(s => s.User.ToString().ToUpper().Contains(searchString.ToUpper())).ToList();
                }
                var list = new List<StaffVM>();
                foreach (Staff staff in staffList)
                {
                    var staffToAdd = new StaffVM
                    {
                        Id = staff.Id,
                        Username = staff.User.ToString(),
                        UserId = staff.UserId,
                        ProfileImage = staff.User.ProfileImage
                    };
                    foreach (StaffJob jobs in staff.Jobs)
                    {
                        if(jobs.IsCurrent)
                        {
                            staffToAdd.StaffJob = jobs.Rank.Name;
                            staffToAdd.Department = jobs.Department;
                            staffToAdd.DateEmployed = jobs.DateEmployed;
                            break;
                        }
                    }
                    list.Add(staffToAdd);
                }
                model.Staffs = new PaginatedList<StaffVM>(list.OrderBy(x => x.Username).ToList(), pageNumber ?? 1, 7);
                string toastMessage = "";
                if (TempData.ContainsKey("Toast"))
                    toastMessage = TempData["Toast"].ToString();
                ViewData["Toast"] = toastMessage;
            }
            catch(Exception ex)
            {

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateStaff(int staffId)
        {
            CreateStaffVM model = new CreateStaffVM();
            try
            {
                Staff staff = await _staffRepo.GetSingle(staffId);
                if(staff!=null)
                {
                    var user = await _userManager.FindByIdAsync(staff.UserId);
                    model.StaffId = staff.Id;
                    model.Username = user.Title.ToString() + ". " + user.FirstName + " " + user.LastName;
                    var rank = await _rankRepo.GetAll();
                    ViewData["Rank"] = new SelectList(rank, "Id", "Name");
                }
                
            }
            catch(Exception ex)
            {

            }
            
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateStaff(CreateStaffVM model)
        {
            try
            {
                bool didError = false;
                if(ModelState.IsValid)
                {
                    if ((int)model.RankId < 1)
                    {
                        ModelState.AddModelError(String.Empty, "Staff Rank is required");
                        didError = true;
                    }
                    if ((int)model.Department < 1)
                    {
                        ModelState.AddModelError(String.Empty, "Staff Department is required");
                        didError = true;
                    }
                    if (didError)
                    {
                        var rank = await _rankRepo.GetAll();
                        ViewData["Rank"] = new SelectList(rank, "Id", "Name");
                        return View(model);
                    }
                        

                    Staff staff = await _staffRepo.GetSingle(model.StaffId);
                    var user = await _userManager.FindByIdAsync(staff.UserId);
                    user.Department = model.Department;                    
                    StaffJob newJob = new StaffJob
                    {
                        DateEmployed = model.DateEmployed,
                        Department = model.Department,
                        RankId = model.RankId,
                        IsCurrent = true,
                        StaffId = staff.Id
                    };
                    await _jobRepo.Add(newJob);
                    await _userManager.UpdateAsync(user);
                    TempData["Toast"] = "Staff: " + model.Username + " has been successfully been added.";
                }
            }
            catch(Exception ex)
            {
                var rank = await _rankRepo.GetAll();
                ViewData["Rank"] = new SelectList(rank, "Id", "Name");
                return View(model);
            }
            return RedirectToAction("Index");
        }

        //Profile
        //Add Resume
        //Add Qualification
    }
}
