using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Entities
{
    public class Job : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public Rank RankId { get; set; }
        public virtual Rank Rank {get;set;}
    }
}
