using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Domain.Entities;
using PromotionsWebApp.Domain.Entities.Procurement;
using PromotionsWebApp.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Core.Configurations
{
    public static class SeedData
    {
        public static List<DefaultUser> DefaultUserSeed()
        {
            return new List<DefaultUser>
            {
                new DefaultUser("SystemAdmin","",UserRoleEnum.Master,DepartmentEnum.IT,"master@email.com","iamsystemadmin")
            };
        }
        public static List<Vendor> VendorSeed()
        {
            return new List<Vendor>
            {
                new Vendor("Hishiko Technologies", "dhishiko@gmail.com", "Tulonga Hishiko","0815542840"),
                new Vendor("Tuliza Inc", "dhishiko@gmail.com", "Tulonga Hishiko","0815542840")
            };
        }
       
    }
}
