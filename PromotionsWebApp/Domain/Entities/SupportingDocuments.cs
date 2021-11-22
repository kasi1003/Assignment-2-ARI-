
using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey("IdentityDocumentId")]
        public virtual Document IdentityDocument { get; set; }
        public int? CVId { get; set; }
        [ForeignKey("CVId")]
        public virtual Document CV { get; set; }
        public int? QualificationsDocumentId { get; set; }
        [ForeignKey("QualificationsDocumentId")]
        public virtual Document QualificationsDocument { get; set; }
        public int? StudentEvalFormId { get; set; }
        [ForeignKey("StudentEvalFormId")]
        public virtual Document StudentEvalForm { get; set; }
        public int? PeerEvalFormId { get; set; }
        [ForeignKey("PeerEvalFormId")]
        public virtual Document PeerEvalForm { get; set; }
        public int? CommunityServiceFormId { get; set; }
        [ForeignKey("CommunityServiceFormId")]
        public virtual Document CommunityServiceForm { get; set; }
        public int? ScholarshipInTeachingFormId { get; set; }
        [ForeignKey("ScholarshipInTeachingFormId")]
        public virtual Document ScholarshipInTeachingForm { get; set; }
        public int? SelfScoredEvaluationFormId { get; set; }
        [ForeignKey("SelfScoredEvaluationFormId")]
        public virtual Document SelfScoredEvaluationForm { get; set; }


    }
}
