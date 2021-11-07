using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using PromotionsWebApp.Core.Interfaces;
using PromotionsWebApp.Domain.Entities;
using PromotionsWebApp.Domain.Settings;
using PromotionsWebApp.Models.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Controllers
{
    public abstract class BaseController:Controller
    {
        protected readonly UserManager<User> _userManager;
        protected readonly SignInManager<User> _signInManager;
        protected readonly IRepository<Staff> _staffRepo;
        protected readonly IEmailSender _emailSender;
        protected readonly IUserRepository _userRepo;
        protected readonly IRepository<Rank> _rankRepo;
        protected readonly IRepository<StaffJob> _jobRepo;
        protected readonly IRepository<Department> _departmentRepo;
        protected readonly IRepository<Document> _documentRepo;
        protected readonly IRepository<Publication> _publicationRepo;
        protected readonly IRepository<Faculty> _facultyRepo;
        protected readonly IRepository<Qualification> _qualificationRepo;
        protected readonly IRepository<SupportingDocuments> _supportDocumentsRepo;
        public BaseController(IServiceScopeFactory factory)
        {
            _departmentRepo = factory.CreateScope().ServiceProvider.GetRequiredService<IRepository<Department>>();
            _documentRepo = factory.CreateScope().ServiceProvider.GetRequiredService<IRepository<Document>>();
            _facultyRepo = factory.CreateScope().ServiceProvider.GetRequiredService<IRepository<Faculty>>();
            _userRepo = factory.CreateScope().ServiceProvider.GetRequiredService<IUserRepository>();
            _staffRepo= factory.CreateScope().ServiceProvider.GetRequiredService<IRepository<Staff>>();
            _rankRepo= factory.CreateScope().ServiceProvider.GetRequiredService<IRepository<Rank>>();
            _jobRepo=factory.CreateScope().ServiceProvider.GetRequiredService<IRepository<StaffJob>>();
            _qualificationRepo=factory.CreateScope().ServiceProvider.GetRequiredService<IRepository<Qualification>>();
            _emailSender= factory.CreateScope().ServiceProvider.GetRequiredService<IEmailSender>();
            _publicationRepo = factory.CreateScope().ServiceProvider.GetRequiredService<IRepository<Publication>>();
            _supportDocumentsRepo = factory.CreateScope().ServiceProvider.GetRequiredService<IRepository<SupportingDocuments>>();
            _userManager = _userRepo.GetUserManager();
            _signInManager = _userRepo.GetSignInManager();
        }

        [HttpGet]
        public async Task<JsonResult> GetDepartmentsJson([FromQuery]int facultyId)
        {

            List<DepartmentVM> model = new List<DepartmentVM>();

            try
            {
                IEnumerable<Department> departments = await _departmentRepo.FindBy(x => x.FacultyId == facultyId);
                foreach (Department dep in departments)
                {
                    model.Add(new DepartmentVM(dep.Id,dep.Name));
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(model);
        }

        protected async Task GetFacultySelect()
        {
            try
            {
                IEnumerable<Faculty> model = await _facultyRepo.GetAll();
                ViewData["Faculties"] = new SelectList(model, "Id", "Name");
            }
            catch(Exception ex)
            {

            }

        }
        protected async Task GetRankSelect()
        {
            try
            {
                IEnumerable<Rank> model = await _rankRepo.GetAll();
                ViewData["Ranks"] = new SelectList(model, "Id", "Name");
            }
            catch (Exception ex)
            {

            }

        }
    }
}
