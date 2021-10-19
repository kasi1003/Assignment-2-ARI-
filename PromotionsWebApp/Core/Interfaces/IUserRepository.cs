using Microsoft.AspNetCore.Identity;
using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Core.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> Get();
        User GetByEmail(string email);
        Task<IdentityResult> Create(User user, string password, UserRoleEnum role);
        Task<IdentityResult> Delete(User user);
        Task<IdentityResult> Update(User user);
        UserManager<User> GetUserManager();
    }
}
