using PromotionsWebApp.Core.Data;
using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PromotionsWebApp.Core.Configurations
{
    public static class DbContextExtension
    {
        //check if migration is done
        public static bool AllMigrationsApplied(this DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            bool isMigrationNeeded = !total.Except(applied).Any();
            return isMigrationNeeded;
        }

        //Identity seed methods
        private static async Task CreateDefaultUserAndRoleForApplication(UserManager<User> um, RoleManager<IdentityRole> rm)
        {

            const string administratorRole = "Administrator";
            const string managerRole = "Manager";
            const string masterRole = "Master";
            const string becRole = "BEC";
            List<DefaultUser> defaultUserList = SeedData.DefaultUserSeed();

            await CreateRoles(rm);
            foreach (DefaultUser defaultUser in defaultUserList) 
            {
                var user = await CreateDefaultUser(um, defaultUser);
                await SetPasswordForDefaultUser(um, defaultUser, user);
                switch(defaultUser.Role)
                {
                    case UserRoleEnum.Administrator:
                        await AddDefaultRoleToDefaultUser(um, defaultUser, administratorRole, user);
                        break;
                    case UserRoleEnum.Manager:
                        await AddDefaultRoleToDefaultUser(um, defaultUser, managerRole, user);
                        break;
                    case UserRoleEnum.Master:
                        await AddDefaultRoleToDefaultUser(um, defaultUser, masterRole, user);
                        break;
                    case UserRoleEnum.BEC:
                        await AddDefaultRoleToDefaultUser(um, defaultUser, becRole, user);
                        break;
                }
                
            }
            
        }
        private static async Task CreateRoles(RoleManager<IdentityRole> rm)
        {
            string[] roles = new[] {"Administrator", "Manager","Master","BEC" };
            foreach (var role in roles)
            {
                if (!await rm.RoleExistsAsync(role))
                {
                    var newRole = new IdentityRole(role);
                    await rm.CreateAsync(newRole);
                    await rm.AddClaimAsync(newRole, new Claim(role, role, ClaimValueTypes.String));
                }
            }
        }
        private static async Task<User> CreateDefaultUser(UserManager<User> um, DefaultUser defaultUser)
        {
            //logger.LogInformation($"Create default user with email `{defaultUser.Email}` for application");
            var user = new User
            {
                Email = defaultUser.Email,
                FirstName = defaultUser.FirstName,
                Surname = defaultUser.Surname,
                UserName = defaultUser.FirstName,
                Role = defaultUser.Role,
                Department = defaultUser.Department,
                EmailConfirmed = true
            };

            var ir = await um.CreateAsync(user);
            if (ir.Succeeded)
            {
                //logger.LogDebug($"Created default user `{defaultUser.Email}` successfully");
            }
            else
            {
                var exception = new ApplicationException($"Default user `{user.UserName}` cannot be created");
                //logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                throw exception;
            }

            var createdUser = await um.FindByEmailAsync(defaultUser.Email);
            return createdUser;
        }
        private static async Task SetPasswordForDefaultUser(UserManager<User> um, DefaultUser defaultUser, User user)
        {
            //logger.LogInformation($"Set password for default user `{defaultUser.Email}`");
            var ir = await um.AddPasswordAsync(user, defaultUser.Password);
            if (ir.Succeeded)
            {
                //logger.LogTrace($"Set password `{defaultUser.Password}` for default user `{defaultUser.Email}` successfully");
            }
            else
            {
                var exception = new ApplicationException($"Password for the user `{defaultUser.Email}` cannot be set");
                //logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                throw exception;
            }
        }
        private static async Task AddDefaultRoleToDefaultUser(UserManager<User> um, DefaultUser defaultUser, string role, User user)
        {
            //logger.LogInformation($"Add default user `{defaultUser.Email}` to role '{administratorRole}'");
            var ir = await um.AddToRoleAsync(user, role);
            if (ir.Succeeded)
            {
                //logger.LogDebug($"Added the role '{administratorRole}' to default user `{defaultUser.Email}` successfully");
            }
            else
            {
                var exception = new ApplicationException($"The role `{role}` cannot be set for the user `{defaultUser.Email}`");
                //logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                throw exception;
            }
        }


        //Seed Application Database
        public static void EnsureSeeded(this IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                if (serviceScope.ServiceProvider.GetService<pContext>().AllMigrationsApplied())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<pContext>();
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    //User Seed
                    context.Database.EnsureCreated();
                    bool changed = false;
                    Task.Run(async () =>
                    {
                        if (!context.Users.Any())
                        {
                            await CreateDefaultUserAndRoleForApplication(userManager, roleManager);
                        }
                        //if(!context.Vendor.Any())
                        //{
                        //    changed = true;
                        //    await context.Vendor.AddRangeAsync(SeedData.VendorSeed());

                        //}
                    }).Wait();
                   


                    if (changed)
                        context.SaveChanges();
                }
               
            }
        }
    }
}
