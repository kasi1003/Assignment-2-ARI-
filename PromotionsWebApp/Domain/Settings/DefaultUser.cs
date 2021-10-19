using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Settings
{
    public class DefaultUser
    {
        public DefaultUser(string name,string surname,UserRoleEnum role,DepartmentEnum dep, string email, string password)
        {
            FirstName = name;
            Surname = surname;
            Email = email;
            Password = password;
            Role = role;
            Department = dep;
        }
        public string FirstName { get; set; }
        public UserRoleEnum Role { get; set; }
        public DepartmentEnum Department { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
