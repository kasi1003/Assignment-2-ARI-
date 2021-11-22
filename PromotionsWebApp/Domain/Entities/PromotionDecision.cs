using PromotionsWebApp.Domain.Abstract;

namespace PromotionsWebApp.Domain.Entities
{
    public class PromotionDecision:BaseEntity
    {
        public int PromotionId { get; set; }
        public PromotionStageApprovalEnum Decision { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Remarks { get; set; }
        public int? SubmissionDocumentId { get; set; }
        public virtual Document SubmissionDocument { get; set; }
    }
}
