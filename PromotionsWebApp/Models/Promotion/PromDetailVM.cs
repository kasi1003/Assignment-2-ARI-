using Microsoft.AspNetCore.Http;

namespace PromotionsWebApp.Models.Promotion
{
    public class PromDetailVM
    {
        public PromotionsWebApp.Domain.Entities.Promotion  Promotion { get; set; }
        public PromDecide AcceptDec { get; set; }
        public PromDecide RejectDec { get; set; }
    }
}
