using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models
{
    public class StaffVM
    {
        public StaffVM() { }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public byte[] ProfileImage { get; set; }
        public string StaffJob { get; set; }
        public DepartmentEnum Department { get; set; }
        public DateTime DateEmployed { get; set; }
        //public int ResumeId { get; set; }
        //public DocumentVM Resume { get; set; }
        //public List<QualificationVM> Qualifications { get; set; }
    }
}
