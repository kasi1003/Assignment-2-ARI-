using PromotionsWebApp.Models.Qualification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models.Staff
{
    public class StaffProfileVM
    {
        public int Id { get; set; }
        public byte[] ProfileImage { get; set; }
        public string StaffNr { get; set; }
        public string Rank { get; set; }
        public string Name { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
        public List<QualificationVM> Qualifications { get; set; }
        public List<PublicationVM> Publications { get; set; }
        public List<StaffJobVM> Jobs { get; set; }
        public int SupportingDocumentsId { get; set; }
        public SupportingDocumentsVM SupportingDocuments { get; set; }
    }
}
