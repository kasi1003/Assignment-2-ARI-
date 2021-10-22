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
using PromotionsWebApp.Domain.Settings;

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
            List<DefaultUser> defaultUserList = SeedData.DefaultUserSeed();
            await CreateRoles(rm);
            foreach (DefaultUser defaultUser in defaultUserList) 
            {
                var user = await CreateDefaultUser(um, defaultUser);
                await SetPasswordForDefaultUser(um, defaultUser, user);
                await AddDefaultRoleToDefaultUser(um, user.Role.ToString(), user);

            }         
        }
        private static async Task CreateRoles(RoleManager<IdentityRole> rm)
        {
            foreach(string role in Enum.GetNames(typeof(UserRoleEnum)))
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
            var user = new User(defaultUser.Title, defaultUser.FirstName,defaultUser.LastName,
                                defaultUser.Role,defaultUser.Department, defaultUser.Email);
            user.PasswordReset = false;
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

            var createdUser = await um.FindByEmailAsync(user.Email);
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
                var exception = new ApplicationException($"Password for the user `{user.Email}` cannot be set");
                //logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                throw exception;
            }
        }
        private static async Task AddDefaultRoleToDefaultUser(UserManager<User> um, string role, User user)
        {
            //logger.LogInformation($"Add default user `{defaultUser.Email}` to role '{administratorRole}'");
            var ir = await um.AddToRoleAsync(user, role);
            if (ir.Succeeded)
            {
                //logger.LogDebug($"Added the role '{administratorRole}' to default user `{defaultUser.Email}` successfully");
            }
            else
            {
                var exception = new ApplicationException($"The role `{role}` cannot be set for the user `{user.Email}`");
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
                        if(!context.Rank.Any())
                        {
                            changed = true;
                            await context.Rank.AddRangeAsync(SeedData.RankSeed());
                        }
                        
                    }).Wait();
                    if (changed)
                        context.SaveChanges();
                }
               
            }
        }
    }
}
