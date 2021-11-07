using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Entities
{
    public class Publication:BaseEntity
    {
        public string Authors { get; set; }
        public int YearObtained { get; set; }
        public PublicationType PublicationType { get; set; }
        public string Name { get; set; }
        public int StaffId { get; set; }
    }
}
