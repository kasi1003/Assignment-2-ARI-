using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Models
{
    public class DocumentStoreVM
    {
        public DocumentStoreVM() { }
        public DocumentStoreVM(int id, List<DocumentVM> docs)
        {
            Id = id;
            Documents = docs;
        }
        public int Id { get; set; }
        public List<DocumentVM> Documents { get; set; }
    }
    public class DocumentVM
    {
        public DocumentVM(int id,string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
