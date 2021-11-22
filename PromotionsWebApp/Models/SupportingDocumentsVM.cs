using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models
{
    public class SupportingDocumentsVM
    {
        public SupportingDocumentsVM() { }
        public int StaffId { get; set; }

        public int? IdentityDocumentId { get; set; }
        public int? CVId { get; set; }
        public int? QualificationsDocumentId { get; set; }
        public int? StudentEvalFormId { get; set; }
        public int? PeerEvalFormId { get; set; }
        public int? CommunityServiceFormId { get; set; }
        public int? ScholarshipInTeachingFormId { get; set; }
        public int? SelfScoredEvalutionFormId { get; set; }
    }
    public class DocumentVM
    {
        public DocumentVM(int id,string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
      
    }
}
