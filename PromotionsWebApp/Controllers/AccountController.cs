using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromotionsWebApp.Core.Interfaces;
using PromotionsWebApp.Domain.Entities;
using PromotionsWebApp.Domain.Settings;
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
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;
        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountController> logger, IUserRepository repo,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _repo = repo;
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
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Master"))
            {
                IEnumerable<User> users = _userManager.Users.AsEnumerable();
                List<UserVM> userList = new List<UserVM>();
                foreach (User user in users)
                {
                    if (!user.isDeleted)
                    {
                        var substitution = new SubstitutionVM();
                        if (user.SubId > 0)
                        {
                            var sub = await _repo.GetSingle(user.SubId);
                            substitution = new SubstitutionVM(sub.UserSubFromId, sub.UserSubToId, sub.UserSubFrom.ToString(), sub.UserSubTo.ToString(), sub.DateFrom, sub.DateTo, sub.Active);
                        }

                        userList.Add(new UserVM(user.Id, user.FirstName, user.Surname, user.Role, user.Department, user.Email, user.Substituted, user.SubId, substitution));
                    }

                }
                UsersVM model = new UsersVM();
                model.Users = userList.AsEnumerable();
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
                var username = model.FirstName.Substring(0, 1) + model.Surname;
                var user = new User
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    Surname = model.Surname,
                    Role = model.Role,
                    Department = model.Department,
                    EmailConfirmed = true,
                    UserName = username.ToUpper(),
                    SubId = 0,
                    Substituted = false,
                    PasswordReset = true
                };
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
                            //Todo: send email here to user with details
                            var token = await _userManager.GeneratePasswordResetTokenAsync(createdUser);
                            var link = Url.Action("Login", "Account", new { }, Request.Scheme);
                            await _emailSender.SendNewUserDetails(user.Email, user.UserName, model.Password, link);
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
                    FirstName = user.FirstName,
                    Surname = user.Surname,
                    Email = user.Email,
                    Role = user.Role,
                    Department = user.Department,
                    Username = user.UserName,
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
                user.Surname = model.Surname;
                if (User.IsInRole("Master"))
                {
                    user.Role = model.Role;
                    user.Department = model.Department;
                }
                user.Email = model.Email;
                var username = model.FirstName.Substring(0, 1) + model.Surname;
                user.UserName = username.ToUpper();
                var users = await _userManager.FindByEmailAsync(user.Email);
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
                user.Surname = "Deleted";
                user.isDeleted = true;
                await _userRepository.Delete(user);
                TempData["Toast"] = "User has been successfully been deleted.";
            }
            catch (Exception ex)
            {
                TempData["Toast"] = "An error occured, if the issue persists please contact the developer";
                return RedirectToAction(nameof(AccountController.Index), "Account");
            }
            return RedirectToAction(nameof(AccountController.Index), "Account");
        }

        [AllowAnonymous]
        public IActionResult ForgotUsername()
        {
            ForgotLoginVM model = new ForgotLoginVM();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotUsername(ForgotLoginVM model)
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
                await _emailSender.SendLoginDetails(model.EmailAddress, user.UserName, link);
                return RedirectToAction("ForgotUsernameConfirmation");
            }
            catch (Exception ex)
            {
                if (!ModelState.IsValid)
                    return View(model);
                return RedirectToAction("ForgotUsernameConfirmation");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotUsernameConfirmation()
        {
            return View();
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
        #region Helpers
        private Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
