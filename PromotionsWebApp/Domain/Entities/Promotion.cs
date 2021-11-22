using PromotionsWebApp.Domain.Abstract;
using System.Collections.Generic;

namespace PromotionsWebApp.Domain.Entities
{
    public class Promotion:BaseEntity
    {
        public int StaffId { get; set; }
        public virtual Staff Staff { get; set; }
        public PromotionStatusEnum Status { get; set; }
        public List<PromotionDecision> Evaluations { get; set; }
        public int MotivationLetterId { get; set; }
        public Document MotivationLetter { get; set; }
        public int RankId { get; set; }
        public virtual Rank Rank { get; set; }
    }
}
