using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Entities
{
    public class Qualification:BaseEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Instition { get; set; }
        public int YearObtained { get; set; }
        public int CertificateDocumentId { get; set; }
        public int NQFLevel { get; set; }
        public virtual Document CertificateDocument { get; set; }
    }
}
