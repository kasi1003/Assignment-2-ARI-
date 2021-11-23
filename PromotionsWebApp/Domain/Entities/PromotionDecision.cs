using PromotionsWebApp.Domain.Abstract;

namespace PromotionsWebApp.Domain.Entities
{
    public class PromotionDecision:BaseEntity
    {
        public int PromotionId { get; set; }
        public PromotionStageApprovalEnum Decision { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Remarks { get; set; }
        public int? SubmissionDocumentId { get; set; }
        public virtual Document SubmissionDocument { get; set; }
    }
}
