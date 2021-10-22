using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromotionsWebApp.Core.Interfaces;
using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Domain.Entities;
using PromotionsWebApp.Domain.Settings;
using PromotionsWebApp.Models;
using PromotionsWebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _repo;
        private readonly IRepository<Staff> _staffRepo;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;
        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountController> logger, IUserRepository repo,
            IEmailSender emailSender, IRepository<Staff> staffRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _repo = repo;
            _staffRepo = staffRepo;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = "")
        {
            await _signInManager.SignOutAsync();
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind("Username,Password,ReturnUrl")] LoginVM model)
        {
            if (ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = model.ReturnUrl;
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var user = await _userManager.FindByNameAsync(model.Username.ToUpper());
                if (user != null)
                {
                    if (user.isDeleted)
                    {
                        ModelState.AddModelError(string.Empty, "User Deleted");
                        return View(model);
                    }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.Username.ToUpper(), model.Password, true, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation(1, "User logged in.");

                            if (user.PasswordReset)
                            {
                                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                                return RedirectToAction("ResetPassword", "Account", new { token, email = user.Email });
                            }

                            return RedirectToAction("Index", "Home", new { area = "" });

                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return View(model);
                        }
                        if (result.IsLockedOut)
                        {
                            _logger.LogWarning(2, "User account locked out.");
                            return View("Lockout");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return View(model);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }

        // POST: /Account/LogOff
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Index(string currentFilter = null,
                                                string searchString = null,
                                                int? pageNumber = null)
        {
            if (User.IsInRole("Admin"))
            {
                IEnumerable<User> users = _userManager.Users.AsEnumerable();
                List<UserVM> userList = new List<UserVM>();

                if (searchString != null)
                {
                    pageNumber = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewData["CurrentFilter"] = searchString;               
                foreach (User user in users)
                {
                    if (!user.isDeleted)
                    {
                        userList.Add(new UserVM(user.Id,user.Title, user.FirstName, user.LastName, user.Role, 
                            user.Department, user.Email,user.ProfileImage));
                    }
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    userList = userList.Where(s => s.ToUserString().ToUpper().Contains(searchString.ToUpper())).ToList();
                }
                UsersVM model = new UsersVM();
                model.Users = new PaginatedList<UserVM>(userList.OrderBy(x => x.ToUserString()).ToList(), pageNumber ?? 1, 7);
                string toastMessage = "";
                if (TempData.ContainsKey("Toast"))
                    toastMessage = TempData["Toast"].ToString();
                ViewData["Toast"] = toastMessage;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            UserVM model = new UserVM();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                bool didError = false;
                if ((int)model.Role < 1)
                {
                    ModelState.AddModelError(String.Empty, "User Role is required");
                    didError = true;
                }
                if ((int)model.Department < 1)
                {
                    ModelState.AddModelError(String.Empty, "User Department is required");
                    didError = true;
                }
                if (didError)
                    return View(model);

                //add user to system
                var user = new User(model.Title, model.FirstName, model.Surname,
                                model.Role,model.Department, model.Email);
                var users = await _userManager.FindByEmailAsync(user.Email);
                if (users != null)
                {
                    if (!users.isDeleted)
                    {
                        ModelState.AddModelError(string.Empty, "Email address is already assigned to another account");
                        return View(model);
                    }

                }
                var ir = await _userManager.CreateAsync(user);
                var createdUser = await _userManager.FindByNameAsync(user.UserName);
                if (ir.Succeeded)
                {
                    //logger.LogDebug($"Created default user `{defaultUser.Email}` successfully");
                    //add password to user


                    var io = await _userManager.AddPasswordAsync(user, model.Password);
                    if (io.Succeeded)
                    {
                        var im = await _userManager.AddToRoleAsync(user, user.Role.ToString());
                        if (im.Succeeded)
                        {
                            //send email here to user with details
                            var token = await _userManager.GeneratePasswordResetTokenAsync(createdUser);
                            var link = Url.Action("Login", "Account", new { }, Request.Scheme);
                            await _emailSender.SendNewUserDetails(user.Email, user.UserName, model.Password, link);
                            if(user.Role == UserRoleEnum.Staff)
                            {
                                Staff newStaff = new Staff(user.Id,user);
                                await _staffRepo.Add(newStaff);
                                return RedirectToAction("CreateStaff", "Staff", new { staffId = newStaff.Id });
                            }                            
                        }
                        else
                        {
                            var exception = new ApplicationException($"The role `{user.Role.ToString()}` cannot be set for the user `{user.Email}`");
                            //logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                            ModelState.AddModelError(string.Empty, "The user role cannot be set");
                            throw exception;
                        }
                    }
                    else
                    {
                        var exception = new ApplicationException($"Password for the user `{user.Email}` cannot be set");
                        //logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                        ModelState.AddModelError(string.Empty, "Something wrong with password entry");
                        throw exception;
                    }

                }
                else
                {
                    var exception = new ApplicationException($"New User `{user.UserName}` cannot be created");
                    //logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                    ModelState.AddModelError(string.Empty, "User can not be created, please try again.");
                    throw exception;
                }


                TempData["Toast"] = "User: " + user.UserName + " has been successfully been added.";
            }
            catch (Exception ex)
            {
                if (!ModelState.IsValid)
                    return View(model);
                TempData["Toast"] = "An error occured, if the issue persists please contact the developer";
                return RedirectToAction(nameof(AccountController.Index), "Account");
            }
            return RedirectToAction(nameof(AccountController.Index), "Account");
        }
        

        [HttpGet]
        public async Task<IActionResult> Profile([FromQuery] string userId)
        {
            UserVM model = new UserVM();
            try
            {
                User user = null;
                user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    user = await _userManager.FindByNameAsync(userId);
                    if (user == null)
                    {
                        var exception = new ApplicationException($"User does not exist in system");
                        //logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                        throw exception;
                    }
                }
                model = new UserVM
                {
                    Id = user.Id,
                    Title = user.Title,
                    ProfileImage = user.ProfileImage,
                    FirstName = user.FirstName,
                    Surname = user.LastName,
                    Email = user.Email,
                    Role = user.Role,
                    Department = user.Department,
                    Password = null
                };

            }
            catch (Exception ex)
            {

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Profile(UserVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                bool didError = false;
                if ((int)model.Role < 1)
                {
                    ModelState.AddModelError(String.Empty, "User Role is required");
                    didError = true;
                }
                if ((int)model.Department < 1)
                {
                    ModelState.AddModelError(String.Empty, "User Department is required");
                    didError = true;
                }
                if (didError)
                    return View(model);

                User user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    var exception = new ApplicationException($"User does not exist in system");
                    //logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                    throw exception;
                }
                user.FirstName = model.FirstName;
                user.LastName = model.Surname;
                if (User.IsInRole("Admin"))
                {
                    user.Role = model.Role;
                    user.Department = model.Department;
                }
                user.Email = model.Email;
                user.UserName = model.Email;
                var users = await _userManager.FindByEmailAsync(user.Email);
                if (user.Role == UserRoleEnum.Staff)
                {
                    var staffList = await _staffRepo.GetAll();
                    var staff = staffList.Where(x => x.UserId == user.Id).First();
                    await _staffRepo.Update(staff);
                }
                if (users != null && users.Id != user.Id)
                {
                    if (!users.isDeleted)
                    {
                        ModelState.AddModelError(string.Empty, "Email address is already assigned to another account");
                        return View(model);
                    }

                }
                var ir = await _userManager.UpdateAsync(user);
                if (ir.Succeeded)
                {

                    //logger.LogDebug($"Created default user `{defaultUser.Email}` successfully");
                    if (model.Password != null)
                    {
                        var createdUser = await _userManager.FindByEmailAsync(user.Email);
                        var io = await _userManager.RemovePasswordAsync(createdUser);
                        if (io.Succeeded)
                        {
                            //logger.LogTrace($"Set password `{defaultUser.Password}` for default user `{defaultUser.Email}` successfully");
                            await _userManager.AddPasswordAsync(createdUser, model.Password);
                        }
                        else
                        {
                            var exception = new ApplicationException($"The role `{user.Role.ToString()}` cannot be set for the user `{user.Email}`");
                            ModelState.AddModelError(string.Empty, "User can not be assigned to role");
                            //logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                            throw exception;
                        }
                    }

                }
                else
                {
                    var exception = new ApplicationException($"New User `{user.UserName}` cannot be created");
                    ModelState.AddModelError(string.Empty, "User cannot be created, please try again.");
                    //logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                    throw exception;
                }
                TempData["Toast"] = "User: " + user.UserName + " has been successfully been modified.";
            }
            catch (Exception ex)
            {
                if (!ModelState.IsValid)
                    return View(model);
                TempData["Toast"] = "An error occured, if the issue persists please contact the developer";
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            if (User.IsInRole("Master"))
                return RedirectToAction(nameof(AccountController.Index), "Account");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);              
                user.Email = "email@email.com";
                user.UserName = "Deleted";
                user.FirstName = "Deleted";
                user.LastName = "Deleted";
                user.isDeleted = true;
                await _repo.Delete(user);
                if (user.Role == UserRoleEnum.Staff)
                {
                    Staff staff = await _staffRepo.GetSingle(x => x.UserId == userId);
                    await _staffRepo.Delete(staff.Id);
                }
                TempData["Toast"] = "User has been successfully been deleted.";
            }
            catch (Exception ex)
            {
                TempData["Toast"] = "An error occured, if the issue persists please contact the developer";
                return RedirectToAction(nameof(AccountController.Index), "Account");
            }
            return RedirectToAction(nameof(AccountController.Index), "Account");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            ForgotLoginVM model = new ForgotLoginVM();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotLoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User does not exist in system.");
                    return View(model);
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var link = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);
                await _emailSender.SendEmailPasswordReset(user.Email, link);
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            catch (Exception ex)
            {
                if (!ModelState.IsValid)
                    return View(model);
                return RedirectToAction("ForgotPasswordConfirmation");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordVM(token, email);
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword)
        {
            if (!ModelState.IsValid)
                return View(resetPassword);
            try
            {
                var user = await _userManager.FindByEmailAsync(resetPassword.Email);
                if (user == null)
                    RedirectToAction("ResetPasswordConfirmation");

                var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (!resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                        ModelState.AddModelError(error.Code, error.Description);
                    return View();
                }
                user.PasswordReset = false;
                await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("ResetPasswordConfirmation");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //change profile image
        
    }
}
