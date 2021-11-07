using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Entities
{
    public class Faculty:BaseEntity
    {
        public string Name { get; set; }
        public virtual IEnumerable<Department> Departments { get; set; }
    }
}
