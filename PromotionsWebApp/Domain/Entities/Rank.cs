using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Entities
{
    public class Rank:BaseEntity
    {
        public Rank() { }
        public Rank(string name,string desc,int level)
        {
            Name = name;
            Description = desc;
            NQFLevel = level;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NQFLevel { get; set; }
    }
}
