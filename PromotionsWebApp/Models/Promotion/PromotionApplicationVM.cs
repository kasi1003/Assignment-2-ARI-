using System;
using System.Collections.Generic;

namespace PromotionsWebApp.Models.Promotion
{
    public class PromotionApplicationVM
    {
        public PromotionApplicationVM() 
        {
            Publications = new List<PublicationVM>();
        }
        public string StaffName { get; set; }
        public DateTime DateEmployed { get; set; }
        public SupportingDocumentsVM SupportingDocuments { get; set; }
        public List<PublicationVM> Publications { get; set; }
        public virtual RankVM Rank { get; set; }
        public bool canApply { get; set; }

    }
}
