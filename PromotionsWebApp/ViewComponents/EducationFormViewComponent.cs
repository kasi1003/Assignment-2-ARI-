using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.ViewComponents
{
    public class EducationFormViewComponent:ViewComponent
    {
        public EducationFormViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(EducationViewModel model)
        {

            return View(model);

        }
        public class EducationViewModel
        {
            public EducationViewModel() { }
            public EducationViewModel(int staffId)
            {
                YearObtained = DateTime.Now.Year;
                NQFLevel = 7;
                StaffId = staffId;
            }
            [Required]
            public int StaffId { get; set; }
            [Required]
            public int NQFLevel { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string Institution { get; set; }
            [Required]
            public int YearObtained { get; set; }
            [Required]
            public IFormFile QualificationDocument { get; set; }
        }
    }
}
