using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromotionsWebApp.Domain.Abstract;
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
            public PersonalDetailViewModel(int staffId)
            {
                StaffId = staffId;
                Title = 0;
            }
            public int StaffId { get; set; }
 
            public TitleEnum Title { get; set; }
           
            public string FirstName { get; set; }
        
            public string Surname { get; set; }
           
            public string StaffNr { get; set; }
           
            [EmailAddress]
            public string Email { get; set; }
         
            public IFormFile ProfileImage { get; set; }
        }
    }
}
