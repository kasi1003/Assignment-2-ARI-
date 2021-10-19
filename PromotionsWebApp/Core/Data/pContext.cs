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
        public DbSet<Department> Department { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<DocumentStore> DocumentStore { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<Rank> Rank { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<StaffJob> StaffJob { get; set; }
        public DbSet<StaffRating> StaffRating { get; set; }
    }
}
