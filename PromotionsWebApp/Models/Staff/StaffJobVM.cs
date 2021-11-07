using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models
{
    public class StaffJobVM
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public virtual StaffVM Staff { get; set; }
        public string Name { get; set; }
        public bool IsCurrent { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
        public DateTime DateEmployed { get; set; }
        public DateTime? DateLeft { get; set; }
    }
}
