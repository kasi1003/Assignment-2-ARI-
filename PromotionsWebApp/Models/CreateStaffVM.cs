using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models
{
    public class CreateStaffVM
    {
        public CreateStaffVM()
        {
            DateEmployed = DateTime.Today;
        }

        [Required]
        public int StaffId { get; set; }
        public string Username { get; set; }
        [Required]
        public int RankId { get; set; }
        [Required]
        public DepartmentEnum Department { get; set; }
        [Required]
        public DateTime DateEmployed { get; set; }
    }
}
