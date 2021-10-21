using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models
{
    public class UserVM
    {
        public UserVM() 
        {
            Role = 0;
            Department = 0;
        }
        public UserVM(string id,TitleEnum title,string name, string surname, UserRoleEnum role,
            DepartmentEnum dep, string email,byte[] profileImage)
        {
            Id = id;
            Title = title;
            ProfileImage = profileImage;
            FirstName = name;
            Surname = surname;
            Role = role;
            Department = dep;
            Email = email;
        }
        public string Id { get; set; }
        [Required]
        public TitleEnum Title { get; set; }
        public byte[] ProfileImage { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public UserRoleEnum Role { get; set; }
        [Required]
        public DepartmentEnum Department { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return FirstName + " " + Surname + " - "+Role.ToString()+", "+Department.ToString();
        }
        public string ToUserString()
        {
            return Title.ToString()+". "+ FirstName + " " + Surname;
        }
    }
}
