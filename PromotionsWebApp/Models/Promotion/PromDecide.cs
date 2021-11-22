using Microsoft.AspNetCore.Http;

namespace PromotionsWebApp.Models.Promotion
{
    public class PromDecide
    {
        public int PromotionId { get; set; }
        public string Remarks { get; set; }
        public IFormFile Submission { get; set; }
    }
}
