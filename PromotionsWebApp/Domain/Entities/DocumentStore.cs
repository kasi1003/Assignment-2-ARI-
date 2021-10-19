
using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Entities
{
    public class DocumentStore : BaseEntity
    {
        public DocumentStore() : base() 
        {
            Documents = new List<Document>();
        }

        public virtual List<Document> Documents { get; set; }
    }
}
