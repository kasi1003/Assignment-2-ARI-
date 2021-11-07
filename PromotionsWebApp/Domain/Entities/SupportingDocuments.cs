
using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Entities
{
    public class SupportingDocuments : BaseEntity
    {
        public SupportingDocuments() : base() 
        {

        }

        public int StaffId { get; set; }

        public int? IdentityDocumentId { get; set; }
        public virtual Document IdentityDocument { get; set; }
        public int? CVId { get; set; }
        public virtual Document CV { get; set; }
        public int? StudentEvalFormId { get; set; }
        public virtual Document StudentEvalForm { get; set; }
        public int? PeerEvalFormId { get; set; }
        public virtual Document PeerEvalForm { get; set; }
        public int? CommunityServiceFormId { get; set; }
        public virtual Document CommunityServiceForm { get; set; }
        public int? ScholarshipInTeachingFormId { get; set; }
        public virtual Document ScholarshipInTeachingForm { get; set; }


    }
}
