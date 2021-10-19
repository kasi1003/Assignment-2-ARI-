using Microsoft.AspNetCore.Identity;
using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Entities
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRoleEnum Role { get; set; }
        public bool PasswordReset { get; set; }
        public bool isDeleted { get; set; }
    }
}
