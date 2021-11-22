using PromotionsWebApp.Domain.Abstract;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace PromotionsWebApp.Models.Promotion
{
    public class PromotionVM
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public PromotionStatusEnum Status { get; set; }
        public List<PromotionDecisionVM> Evaluations { get; set; }
        public int MotivationLetterId { get; set; }
    }
}
