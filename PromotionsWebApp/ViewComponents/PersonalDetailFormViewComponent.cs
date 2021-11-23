using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Models.Staff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.ViewComponents
{
    public class PersonalDetailFormViewComponent:ViewComponent
    {
        public PersonalDetailFormViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(PersonalDetailViewModel model)
        {

            return View(model);

        }
        public class PersonalDetailViewModel
        {
            public PersonalDetailViewModel() { }
            public PersonalDetailViewModel(StaffProfileVM model)
            {
                StaffId = model.Id;
                Title = model.Title;
                FirstName = model.Firstname;
                Surname = model.Surname;
                StaffNr = model.StaffNr;
            }
            public int StaffId { get; set; }
 
            public TitleEnum Title { get; set; }
           
            public string FirstName { get; set; }
        
            public string Surname { get; set; }
           
            public string StaffNr { get; set; }                 
         
            public IFormFile ProfileImage { get; set; }
        }
    }
}
