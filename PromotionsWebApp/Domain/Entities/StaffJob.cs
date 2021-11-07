using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Entities
{
    public class StaffJob:BaseEntity
    {
        public int StaffId { get; set; }
        public virtual Staff Staff { get; set; }
        public int RankId { get; set; }
        public virtual Rank Rank { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime DateEmployed { get; set; }
        public DateTime? DateLeft { get; set; }
    }
}
