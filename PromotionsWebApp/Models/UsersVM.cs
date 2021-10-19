using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models
{
    public class UsersVM
    {
        public UsersVM() { }

        public IEnumerable<UserVM> Users { get; set; }
    }
}
