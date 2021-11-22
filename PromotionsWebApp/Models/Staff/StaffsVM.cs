using PromotionsWebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models.Staff
{
    public class StaffsVM
    {
        public StaffsVM() { }
        public PaginatedList<StaffVM> Staffs { get; set; }
    }
}
