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
    public class UploadDocumentsViewComponent:ViewComponent
    {
        public UploadDocumentsViewComponent()
        { 
        
        }

        public async Task<IViewComponentResult> InvokeAsync(UploadDocumentsViewModel model)
        {

            return View(model);

        }
        public class UploadDocumentsViewModel
        {
            public UploadDocumentsViewModel() { }
            public UploadDocumentsViewModel(int id,UploadTypeEnum uploadType)
            {
                SupportingsDocumentId = id;
                UploadType = uploadType;
            }
            [Required]
            public int SupportingsDocumentId { get; set; }
            [Required]
            public IFormFile UploadFile { get; set; }
            public UploadTypeEnum UploadType { get; set; }

        }
    }
}
