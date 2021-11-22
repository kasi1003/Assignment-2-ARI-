using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PromotionsWebApp.Core.Interfaces;
using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Domain.Entities;
using PromotionsWebApp.Domain.Settings;
using PromotionsWebApp.Models.Account;
using PromotionsWebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        ILogger<AccountController> _logger;
        public AccountController(ILogger<AccountController> logger, IServiceScopeFactory factory) : base(factory)
        {
            _logger = logger;
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
                IEnumerable<User> users = _userManager.Users.Where(x=>x.Role!=UserRoleEnum.Staff).AsEnumerable();
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
                    if (user.FacultyId != null)
                        user.Faculty = await _facultyRepo.GetSingle(user.FacultyId.Value);
                    if (user.DepartmentId != null)
                        user.Department = await _departmentRepo.GetSingle(user.DepartmentId.Value);

                    if (!user.isDeleted)
                    {
                        UserVM item = new UserVM(user.Id, user.Title, user.FirstName, user.LastName, user.Role, user.Email, user.ProfileImage);
                        if (user.Faculty != null)
                            item.Faculty = user.Faculty.Name;
                        if (user.Department != null)
                            item.Department = user.Department.Name;
                        userList.Add(item);
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
            CreateUserVM model = new CreateUserVM();
            await GetFacultySelect();
            await GetRankSelect();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await GetFacultySelect();
                    await GetRankSelect();
                    return View(model);
                }
                bool didError = false;

                //Check if Dropdowns are selected
                if ((int)model.Role < 1)
                {
                    ModelState.AddModelError(String.Empty, "User Role is required");
                    didError = true;
                }
                else if ((int)model.Role > 5)
                {
                    if (model.FacultyId < 1)
                    {
                        ModelState.AddModelError(String.Empty, "User Faculty is required");
                        didError = true;
                    }
                    if ((int)model.Role > 6)
                    {
                        if (model.DepartmentId < 1)
                        {
                            ModelState.AddModelError(String.Empty, "User Department is required");
                            didError = true;
                        }
                        if (model.Role == UserRoleEnum.Staff)
                        {
                            if(model.StaffNr == ""||model.StaffNr==null)
                            {
                                ModelState.AddModelError(String.Empty, "Staff number is required");
                                didError = true;
                            }
                            if (model.RankId < 1)
                            {
                                ModelState.AddModelError(String.Empty, "Staff Job Title is required");
                                didError = true;
                            }
                            if (model.DateEmployed > DateTime.Now)
                            {
                                ModelState.AddModelError(String.Empty, "Staff Date Employed cannot be in the future");
                                didError = true;
                            }

                        }
                    }


                }
                if (didError)
                {
                    await GetFacultySelect();
                    await GetRankSelect();
                    return View(model);
                }
                   

                //add user to system
                var user = new User(model.Title, model.FirstName, model.Surname,
                                model.Role, model.Email, model.FacultyId.Value, model.DepartmentId.Value);
                var users = await _userManager.FindByEmailAsync(user.Email);
                if (users != null)
                {
                    if (!users.isDeleted)
                    {
                        ModelState.AddModelError(string.Empty, "Email address is already assigned to another account");
                        await GetFacultySelect();
                        await GetRankSelect();
                        return View(model);
                    }

                }
                var ir = await _userManager.CreateAsync(user);
                var createdUser = await _userManager.FindByNameAsync(user.UserName);
                if (ir.Succeeded)
                {
                    //_logger.LogDebug($"Created default user `{defaultUser.Email}` successfully");
                    //add password to user
                    var io = await _userManager.AddPasswordAsync(user, "NewPassword1");
                    if (io.Succeeded)
                    {
                        var im = await _userManager.AddToRoleAsync(user, user.Role.ToString());
                        if (im.Succeeded)
                        {
                            //send email here to user with details
                            
                            var token = await _userManager.GeneratePasswordResetTokenAsync(createdUser);
                            var link = Url.Action("Login", "Account", new { }, Request.Scheme);
                            await _emailSender.SendNewUserDetails(user.Email, user.UserName, "NewPassword1", link);
                            if (user.Role == UserRoleEnum.Staff)
                            {
                                Staff newStaff = new Staff(user.Id);
                                var faculty = await _facultyRepo.GetSingle(user.FacultyId.Value);
                                var department = await _departmentRepo.GetSingle(user.DepartmentId.Value);
                                newStaff.StaffNr = model.StaffNr;
                                
                                newStaff.Jobs.Add(new StaffJob
                                {
                                    DateEmployed = model.DateEmployed,
                                    RankId = model.RankId.Value,
                                    IsCurrent = true,
                                    Faculty = faculty.Name,
                                    Department = department.Name
                                });
                                await _staffRepo.Add(newStaff);
                                var supDoc = new SupportingDocuments
                                {
                                    StaffId = newStaff.Id,
                                    AddedDate = DateTime.Now,
                                    ModifiedDate = DateTime.Now,
                                    isDeleted = false
                                };
                                await _supportDocumentsRepo.Add(supDoc);
                                newStaff.SupportDocumentsId = supDoc.Id;
                                await _staffRepo.Update(newStaff);
                                TempData["Toast"] = "Staff: " + user.ToString() + " has been successfully been added.";
                                return RedirectToAction("Index", "Staff");
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


                TempData["Toast"] = "User: " + user.ToString() + " has been successfully been added.";
            }
            catch (Exception ex)
            {
                if (!ModelState.IsValid)
                {
                    await GetFacultySelect();
                    await GetRankSelect();
                    return View(model);
                }
                    
                TempData["Toast"] = "An error occured, if the issue persists please contact the developer";
                return RedirectToAction(nameof(AccountController.Index), "Account");
            }
            return RedirectToAction(nameof(AccountController.Index), "Account");
        }


        [HttpGet]
        public async Task<IActionResult> Profile([FromQuery] string userId)
        {
            CreateUserVM model = new CreateUserVM();
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
                model = new CreateUserVM();
                model.Id = user.Id;
                model.Title = user.Title;
                model.FirstName = user.FirstName;
                model.Surname = user.LastName;
                model.Email = user.Email;
                model.Role = user.Role;
                if (user.FacultyId != null)
                    model.FacultyId = user.FacultyId;
                if (user.DepartmentId != null)
                    model.DepartmentId = user.DepartmentId;
                

            }
            catch (Exception ex)
            {

            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Profile(CreateUserVM model)
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
                if (model.Role ==UserRoleEnum.Staff)
                {
                    ModelState.AddModelError(String.Empty, "Cannot change existing users to staff, create new user instead");
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
                user.Title = model.Title;
                user.FirstName = model.FirstName;
                user.LastName = model.Surname;
                if (User.IsInRole("Admin"))
                {
                    user.Role = model.Role;
                    user.DepartmentId = model.DepartmentId;
                    user.FacultyId = model.FacultyId;
                }
                if(user.Email != model.Email)
                {
                    var users = await _userManager.FindByEmailAsync(model.Email);
                    if(users!=null)
                    {
                        ModelState.AddModelError(string.Empty, "Email address is already assigned to another account");
                        return View(model);
                    }
                    else
                    {
                        user.Email = model.Email;
                        user.UserName = model.Email;
                    }
                }
                
                var ir = await _userManager.UpdateAsync(user);
                if (!ir.Succeeded)
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
                await _userRepo.Delete(user);
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
