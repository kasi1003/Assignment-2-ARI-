using Microsoft.AspNetCore.Identity;
using PromotionsWebApp.Core.Interfaces;
using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PromotionsWebApp.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IQueryable<User> Get() => _userManager.Users;

        public User GetByEmail(string email) => _userManager.Users.First(u => u.Email == email);

        public Task<IdentityResult> Create(User user, string password, UserRoleEnum role)
        {
            //add claims & roles
            var ir = _userManager.CreateAsync(user, password);
            ir = _userManager.AddToRoleAsync(user, role.ToString());
            var newClaim = new Claim(role.ToString(), role.ToString(), ClaimValueTypes.String);
            ir = _userManager.AddClaimAsync(user, newClaim);
            return ir;
        }

        public async Task<IdentityResult> Delete(User user)
        {
            //remove old claims & roles
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveClaimsAsync(user, userClaims);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            user.isDeleted = true;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> Update(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public UserManager<User> GetUserManager()
        {
            return _userManager;
        }
        public SignInManager<User> GetSignInManager()
        {
            return _signInManager;
        }
    }
}
