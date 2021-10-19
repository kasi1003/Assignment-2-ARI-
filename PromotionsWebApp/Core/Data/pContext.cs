using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionsWebApp.Core.Data
{
    public class pContext : IdentityDbContext
    {
        public pContext(DbContextOptions<pContext> options)
            : base(options)
        {
        }
    }
}
