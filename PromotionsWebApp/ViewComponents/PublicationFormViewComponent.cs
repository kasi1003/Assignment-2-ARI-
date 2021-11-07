using Microsoft.AspNetCore.Mvc;
using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.ViewComponents
{
    public class PublicationFormViewComponent:ViewComponent
    {
        public PublicationFormViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(PublicationViewModel model)
        {

            return View(model);

        }
        public class PublicationViewModel
        {
            public PublicationViewModel() { }
            public PublicationViewModel(int staffId)
            {
                YearObtained = DateTime.Now.Year;
                StaffId = staffId;
            }
            [Required]
            public int StaffId { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string Authors { get; set; }
            [Required]
            public int YearObtained { get; set; }
            [Required]
            public PublicationType PublicationType { get; set; }
        }
    }
}
