using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Entities
{
    public class Staff:BaseEntity
    {
        public Staff() { }
        public Staff(string id, User user)
        {
            UserId = id;
            User = user; 
        }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual IEnumerable<StaffJob> Jobs { get; set; }
        public virtual IEnumerable<Qualification> Qualifications { get; set; }
        public int? ResumeId { get; set; }
        public Document Resume { get; set; }
    }
}
