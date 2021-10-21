using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.ViewComponents
{
    public class PagerViewComponent : ViewComponent
    {

        public PagerViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(PagerViewModel model)
        {

            return View(model);

        }

        public class PagerViewModel
        {
            public PagerViewModel(int prevpageNr,int nextpageNr, string search,string link, string area, string next,string prev)
            {
                prevPageNumber = prevpageNr;
                nextPageNumber = nextpageNr;
                searchFilter = search;
                linkUrl = link;
                linkArea = area;
                prevDisabled = prev;
                nextDisabled = next;
            }
            public int prevPageNumber { get; set; }
            public int nextPageNumber { get; set; }
            public string searchFilter { get; set; }
            public string linkUrl { get; set; }
            public string linkArea { get; set; }
            public string prevDisabled { get; set; }
            public string nextDisabled { get; set; }
        }
    }
}
