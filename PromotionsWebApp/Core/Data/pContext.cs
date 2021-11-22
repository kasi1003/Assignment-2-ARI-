using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionsWebApp.Core.Data
{
    public class pContext : IdentityDbContext<User>
    {
        public pContext(DbContextOptions<pContext> options)
            : base(options)
        {
        }
        public DbSet<Document> Document { get; set; }
        public DbSet<SupportingDocuments> SupportingDocuments { get; set; }
        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<Rank> Rank { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<StaffJob> StaffJob { get; set; }
        public DbSet<Qualification> Qualification { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Publication> Publication { get; set; }
        public DbSet<Promotion> Promotion { get; set; }
        public DbSet<PromotionDecision> PromotionDecision { get; set; }
    }
}
