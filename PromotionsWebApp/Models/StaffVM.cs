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
        public UserVM User { get; set; }
        public int ResumeId { get; set; }
        public DocumentVM Resume { get; set; }
        public List<StaffJobVM> Jobs { get; set; }
        public List<QualificationVM> Qualifications { get; set; }
    }
}
