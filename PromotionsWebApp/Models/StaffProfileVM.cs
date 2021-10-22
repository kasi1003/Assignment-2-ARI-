using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models
{
    public class StaffProfileVM
    {
        public StaffProfileVM() { }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public byte[] ProfileImage { get; set; }
        public List<QualificationVM> Qualifications { get; set; }
        public int? ResumeId { get; set; }
        public List<StaffJobVM> Jobs { get; set; }
    }
}
