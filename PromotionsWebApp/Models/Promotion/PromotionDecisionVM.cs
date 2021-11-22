using PromotionsWebApp.Domain.Abstract;

namespace PromotionsWebApp.Models.Promotion
{
    public class PromotionDecisionVM
    {
        public int Id { get; set; }
        public int PromotionId { get; set; }
        public PromotionStageApprovalEnum Decision { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string Remarks { get; set; }
        public int? SubmissionDocumentId { get; set; }
    }
}
