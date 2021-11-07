using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models.Account
{
    public class UserVM
    {
        public UserVM() 
        {
            Role = 0;
        }
        public UserVM(string id,TitleEnum title,string name, string surname, UserRoleEnum role, string email,byte[] profileImage,string fac=null,string dep=null)
        {
            Id = id;
            Title = title;
            ProfileImage = profileImage;
            FirstName = name;
            Surname = surname;
            Role = role;
            Email = email;
            Faculty = fac;
            Department = dep;
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
        public string Email { get; set; }
        public string Password { get; set; }
        public int? FacultyId { get; set; }
        public string Faculty { get; set; }
        public int? DepartmentId { get; set; }
        public string Department { get; set; }

        public override string ToString()
        {
            return FirstName + " " + Surname + " - " + Role.ToString();
        }
        public string ToUserString()
        {
            return Title.ToString()+". "+ FirstName + " " + Surname;
        }
    }
}
