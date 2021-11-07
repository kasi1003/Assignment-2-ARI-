using PromotionsWebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models.Account
{
    public class UsersVM
    {
        public UsersVM() { }

        public PaginatedList<UserVM> Users { get; set; }
    }
}
