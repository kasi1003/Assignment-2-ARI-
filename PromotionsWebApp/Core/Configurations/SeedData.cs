using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Domain.Entities;
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
                new DefaultUser(TitleEnum.Mr,"System","Admin",UserRoleEnum.Admin,DepartmentEnum.ComputingAndInformatics,"admin@email.com","iAmSystemAdmin"),
                new DefaultUser(TitleEnum.Mr,"John","Snow",UserRoleEnum.HOD,DepartmentEnum.ComputingAndInformatics,"johnsnow@email.com","promotion1"),
                new DefaultUser(TitleEnum.Mr,"John","Wick",UserRoleEnum.Dean,DepartmentEnum.ComputingAndInformatics,"johnwick@email.com","promotion1"),
                new DefaultUser(TitleEnum.Mr,"Clark","Kent",UserRoleEnum.IFEC,DepartmentEnum.ComputingAndInformatics,"clarkkent@email.com","promotion1"),
                new DefaultUser(TitleEnum.Mr,"Barry","Allen",UserRoleEnum.PSPC,DepartmentEnum.ComputingAndInformatics,"barryallen@email.com","promotion1"),
                new DefaultUser(TitleEnum.Mr, "Bruce", "Wayne", UserRoleEnum.VC, DepartmentEnum.ComputingAndInformatics, "brucewayne@email.com", "promotion1")
            }; 
        }
        //public static List<Vendor> VendorSeed()
        //{
        //    return new List<Vendor>
        //    {
        //        new Vendor("Hishiko Technologies", "dhishiko@gmail.com", "Tulonga Hishiko","0815542840"),
        //        new Vendor("Tuliza Inc", "dhishiko@gmail.com", "Tulonga Hishiko","0815542840")
        //    };
        //}
       
    }
}
