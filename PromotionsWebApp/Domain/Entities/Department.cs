using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Entities
{
    public class Department:BaseEntity
    {
        public string Name { get; set; }
        public int FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }
    }
}
