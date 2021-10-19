using PromotionsWebApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Settings
{
    public interface IEmailSender
    {
        Task SendEmailPasswordReset(string emailAddress, string link);
        Task SendLoginDetails(string emailAddress, string userName, string link);
        Task SendInboxNotification(string email,string link);
        Task SendNewUserDetails(string emailAddress, string userName, string password,string link);
 

    }
}
