using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Settings
{
    public class DefaultUser
    {
        public DefaultUser(TitleEnum title, string name,string surname,UserRoleEnum role,DepartmentEnum dep, string email, string password)
        {
            Title = title;
            FirstName = name;
            LastName = surname;
            Email = email;
            Password = password;
            Role = role;
            Department = dep;
        }
        public TitleEnum Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DepartmentEnum Department { get; set; }
        public byte[] ProfileImage { get; set; }
        public UserRoleEnum Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
