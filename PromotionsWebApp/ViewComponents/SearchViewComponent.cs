using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.ViewComponents
{
    public class SearchViewComponent:ViewComponent
    {

        public SearchViewComponent()
        {
            
        }

        public async Task<IViewComponentResult> InvokeAsync(SearchViewModel model)
        {

            return View(model);

        }

        public class SearchViewModel
        {
            public SearchViewModel(string search,string link,string area)
            {
                searchFilter = search;
                linkUrl = link;
                linkArea = area;
            }
            public string searchFilter { get; set; }
            public string linkUrl { get; set; }
            public string linkArea { get; set; }
        }
    }
}
