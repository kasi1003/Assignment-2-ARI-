using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models
{
    public class StaffJobVM
    {
        public int StaffJobId { get; set; }
        public int StaffId { get; set; }
        public virtual StaffVM Staff { get; set; }
        public DepartmentEnum Department { get; set; }
        public int RankId { get; set; }
        public virtual RankVM Rank { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime DateEmployed { get; set; }
        public DateTime DateLeft { get; set; }
    }
}
