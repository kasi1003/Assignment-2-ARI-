using Microsoft.AspNetCore.Http;

namespace PromotionsWebApp.Models.Promotion
{
    public class PromotionSubmissionVM
    {
        public int StaffId { get; set; }
        public IFormFile MotivationLetter { get; set; }
    }
}
