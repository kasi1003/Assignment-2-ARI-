using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Entities
{
    public class Document:BaseEntity
    {
        public Document() : base() { }
        public Document(string fileName) : base()
        {
            FileName = fileName;
            isDeleted = false;
        }
        public Document(string fileName, byte[] content)
        {
            FileName = fileName;
            Content = content;
            isDeleted = false;
        }
        public int? SupportingDocumentsId { get; set; }
        [NotMapped]
        public virtual SupportingDocuments SupportingDocuments { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public DocumentFileTypeEnum FileType { get; set; }

    }
}
