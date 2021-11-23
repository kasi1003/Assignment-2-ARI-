using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models.Staff
{
    public class StaffVM
    {
        public StaffVM() { }
        public int Id { get; set; }
        public string StaffNr { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public TitleEnum Title { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; } 
        public byte[] ProfileImage { get; set; }
        public string StaffJob { get; set; }
        public string Department { get; set; }
        public string Faculty { get; set; }
        public DateTime DateEmployed { get; set; }
        //public int ResumeId { get; set; }
        //public DocumentVM Resume { get; set; }
        //public List<QualificationVM> Qualifications { get; set; }
    }
}
