using Microsoft.AspNetCore.Http;
using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models.Account
{
    public class CreateUserVM
    {
        public CreateUserVM()
        {
            Role = 0;
            FacultyId = 0;
            RankId = 0;
            DepartmentId = 0;
            StaffNr = null;
            DateEmployed = DateTime.Now;
        }
        public string Id { get; set; }
        [Required]
        public TitleEnum Title { get; set; }
        public IFormFile ProfileImage { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public UserRoleEnum Role { get; set; }
        [Required]
        public string Email { get; set; }
        public int? FacultyId { get; set; }
        public int? DepartmentId { get; set; }
        public int? RankId { get; set; }
        public string StaffNr { get; set; }
        public DateTime DateEmployed { get; set; }
        public override string ToString()
        {
            return FirstName + " " + Surname + " - " + Role.ToString();
        }
        public string ToUserString()
        {
            return Title.ToString() + ". " + FirstName + " " + Surname;
        }
    }
}
