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
        public Staff(string id)
        {
            Jobs = new List<StaffJob>();
            UserId = id;
        }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string StaffNr { get; set; }
        public virtual List<StaffJob> Jobs { get; set; }
        public virtual List<Qualification> Qualifications { get; set; }
        public virtual List<Publication> Publications { get; set; }
        public int SupportDocumentsId { get; set; }
        public virtual SupportingDocuments SupportingDocuments { get; set; }
    }
}
