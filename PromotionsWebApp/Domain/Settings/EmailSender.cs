using PromotionsWebApp.Domain.Entities;
using PromotionsWebApp.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Settings
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailMetadata _emailConfig;

        public EmailSender(EmailMetadata emailConfig)
        {
            _emailConfig = emailConfig;

        }

        private async Task SendEmailAsync(EmailMessage model)
        {
            string sMessage;
            SmtpClient smtpClient = new SmtpClient();

            MailMessage message = new MailMessage();
            try
            {

                MailAddress fromAddress = new MailAddress(_emailConfig.Sender, "CENORED Procurement");
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Host = _emailConfig.SmtpServer;
                smtpClient.Port = _emailConfig.Port;
                message.From = fromAddress;

                MailAddress toAddress = new MailAddress(model.To);
                message.To.Add(toAddress);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new System.Net.NetworkCredential(_emailConfig.UserName, _emailConfig.Password);

                message.Subject = model.Subject;
                message.Body = model.Content;
                message.IsBodyHtml = true;

                if (model.Attachment != null)
                {
                    message.Attachments.Add(new Attachment(new MemoryStream(model.Attachment.Content), model.Attachment.FileName));
                }
                smtpClient.Send(message);
                sMessage = "Email sent.";

            }
            catch (Exception ex)
            {
                sMessage = "Coudn't send the message!\n " + ex.Message;
                throw;
            }

        }

        public async Task SendRFQRequestNotification(RFQRequest data, string procurementEmail, Document file = null)
        {
            EmailMessage message = new EmailMessage(procurementEmail,
                "CENORED RFQ Request Notification: " + data.Name,
                CreateRFQRequestNotificationText(data.Requestor.ToString(), data.Requestor.Department.ToString(), data.Name, data.Description, file, data.SelectedVendorString), file);
            await SendEmailAsync(message);
        }
        public async Task SendRFQToVendor(RFQ data, Document file = null)
        {
            foreach (RFQVendor ven in data.SubmittedQuotations)
            {
               
                EmailMessage message = new EmailMessage(ven.Vendor.EmailAddress,
                    "CENORED RFQ: " + data.RFQRequest.Name,
                    CreateRFQToVendorText(data.RFQRequest.Description, data.Deadline.Date.ToShortDateString(),
                    ven.UrlUploadLink, file, data.ReferenceNumber),
                    file,
                    ven.UrlUploadLink);

                await SendEmailAsync(message);
            }
        }
        public async Task SendRFQNotification(RFQ data, List<string> emails, string vendors, Document file = null)
        {
            foreach (string email in emails)
            {
                
                EmailMessage message = new EmailMessage(email,
                    "CENORED RFQ Notification: " + data.RFQRequest.Name,
                    CreateRFQNotificationText(data.RFQRequest.Description, data.Deadline.Date.ToShortDateString(), data.RFQRequest.Requestor.ToString(), file, data.ReferenceNumber, vendors),
                    file);
                await SendEmailAsync(message);
            }

        }
        public async Task SendBECNotification(RFQ data, string email, string vendors, Document file = null)
        {
            
            EmailMessage message = new EmailMessage(email, "CENORED RFQ Notification: " + data.RFQRequest.Name,
                CreateBECNotificationText(data.RFQRequest.Description, data.Deadline.Date.ToShortDateString(), data.RFQRequest.Requestor.ToString(),
                data.RFQRequest.Requestor.Department.ToString(), file, data.ReferenceNumber, vendors),
                file);

            await SendEmailAsync(message);


        }

        public async Task SendEmailPasswordReset(string emailAddress, string link)
        {
            EmailMessage message = new EmailMessage(emailAddress, "CENORED PROCUREMENT Reset Password", CreateResetPasswordText(link));
            await SendEmailAsync(message);
        }
        public async Task SendLoginDetails(string emailAddress, string userName, string password)
        {
            EmailMessage message = new EmailMessage(emailAddress, "CENORED PROCUREMENT Login Details", CreateLoginDetailsText(userName, password));
            await SendEmailAsync(message);
        }
        public async Task SendNewUserDetails(string emailAddress, string userName, string password, string link)
        {
            EmailMessage message = new EmailMessage(emailAddress, "CENORED PROCUREMENT New User", CreateNewUserText(userName, password, link));
            await SendEmailAsync(message);
        }
        public async Task SendInboxNotification(string email, string link)
        {
            EmailMessage message = new EmailMessage(email, "CENORED PROCUREMENT Inbox", CreateInboxNotificationText(link));
            await SendEmailAsync(message);
        }
        public async Task SendRFQReleasedNotification(string email, RFQ data, string link)
        {
            EmailMessage message = new EmailMessage(email, "CENORED PROCUREMENT RFQ Released", CreateRFQReleasedNotificationText(data, link));
            await SendEmailAsync(message);
        }

        public async Task SendTenderClosedNotification(string email,Tender tender)
        {
            EmailMessage message = new EmailMessage(email, "CENORED PROCUREMENT Tender Closed", CreateTenderClosedNotificationText(tender));
            await SendEmailAsync(message);
        }
        public async Task SendTenderCreatedNotification(string email, Tender tender)
        {
            EmailMessage message = new EmailMessage(email, "CENORED PROCUREMENT Tender Created", CreateTenderCreatedNotificationText(tender));
            await SendEmailAsync(message);
        }
        public async Task SendTenderInvitationNotification(List<string> emails, Tender tender)
        {
            foreach(string email in emails)
            {
                EmailMessage message = new EmailMessage(email, "CENORED PROCUREMENT Tender Invitation", CreateTenderInvitationNotificationText(tender));
                await SendEmailAsync(message);
            }
            
        }
        public async Task SendTenderAppliedNotification(TenderVendor data)
        {
            EmailMessage message = new EmailMessage(data.EmailAddress, "CENORED PROCUREMENT Tender Applied", CreateTenderAppliedNotificationText(data));
            await SendEmailAsync(message);
        }
        public async Task SendTenderAppliedApprovedNotification(TenderVendor data)
        {
            EmailMessage message = new EmailMessage(data.EmailAddress, "CENORED PROCUREMENT Tender Application Approved", CreateTenderApplicationApprovedNotificationText(data));
            await SendEmailAsync(message);
        }
        public async Task SendTenderAppliedDeniedNotification(TenderVendor data)
        {
            EmailMessage message = new EmailMessage(data.EmailAddress, "CENORED PROCUREMENT Tender Application Denied", CreateTenderApplicationDeniedNotificationText(data));
            await SendEmailAsync(message);
        }
        public async Task SendTenderSubmittedNotification(TenderVendor data)
        {
            EmailMessage message = new EmailMessage(data.EmailAddress, "CENORED PROCUREMENT Tender Submitted", CreateTenderSubmittedNotificationText(data));
            await SendEmailAsync(message);
        }
        public async Task SendTenderUpdatedNotification(List<TenderVendor> vendors,Tender tender)
        {
            foreach (TenderVendor vendor in vendors)
            {
                EmailMessage message = new EmailMessage(vendor.EmailAddress, "CENORED PROCUREMENT Tender Updated", CreateTenderUpdatedNotificationText(vendor,tender));
                await SendEmailAsync(message);
            }
        }
        private string CreateRFQRequestNotificationText(string requestor, string department, string name, string description, Document file, string vendors)
        {
            var output = "";
            if (file == null)
            {
                output = string.Format("<p>Good Day,</p>" + "<p>A RFQ Request has been created.</p>" +
                                           "<p>Please find attached below description for RFQ Request.</p>" +
                                            "<p>Requestor: {0} </p>" +
                                            "<p>Department: {1}</p>" +
                                            "<p>Name: {2}</p>" +
                                            "<p>Description: {3} </p>" +
                                            "<p>Possible Vendors: {4}" +
                                            "<p>Kind Regards,<br/>Cenored E-Procurement System<br/>CENORED</p>", requestor, department, name, description, vendors);
            }
            else
            {
                output = string.Format("<p>Good Day,</p>" + "<p>A RFQ Request has been created.</p>" +
                                            "<p>Please find attached below description and specifications document for RFQ Request .</p>" +
                                            "<p>Requestor: {0} </p>" +
                                            "<p>Department: {1}</p>" +
                                            "<p>Name: {2}</p>" +
                                            "<p>Description: {3} </p>" +
                                            "<p>Possible Vendors: {4}" +
                                            "<p>Kind Regards,<br/>Cenored E-Procurement System<br/>CENORED</p>", requestor, department, name, description, vendors);
            }
            return output;
        }
        private string CreateRFQNotificationText(string content, string deadline, string requestor, Document file, string refNr, string vendors)
        {
            var output = "";
            if (file == null)
            {
                output = string.Format("<p>Good Day,</p><p>A RFQ has been created</p>" +
                                            "<p>Please find attached below description for RFQ.</p>" +
                                            "<p>Requestor: {0} </p>" +
                                            "<p>Reference Number: {1}</p>" +
                                            "<p>Description: {2} </p>" +
                                            "<p>Deadline: {3}</p>" +
                                            "<p>Vendors: {4}" +
                                            "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", requestor, refNr, content, deadline, vendors);
            }
            else
            {
                output = string.Format("<p>Good Day,</p><p>A RFQ has been created</p>" +
                                            "<p>Please find attached below description and specifications document for RFQ.</p>" +
                                            "<p>Requestor: {0} </p>" +
                                            "<p>Reference Number: {1}</p>" +
                                            "<p>Description: {2} </p>" +
                                            "<p>Deadline: {3}</p>" +
                                            "<p>Vendors: {4}" +
                                            "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", requestor, refNr, content, deadline, vendors);
            }
            return output;
        }
        private string CreateBECNotificationText(string content, string deadline, string requestor, string department, Document file, string refNr, string vendors)
        {
            var output = "";
            if (file == null)
            {
                output = string.Format("<p>Good Day,</p><p>A possible high value RFQ has been requested</p>" +
                                            "<p>Please find below description for RFQ and attached Specifications Document.</p>" +
                                            "<p>Requestor: {0} </p>" +
                                            "<p>Department: {1} </p>" +
                                            "<p>Reference Number: {2}</p>" +
                                            "<p>Description: {3} </p>" +
                                            "<p>Deadline: {4}</p>" +
                                            "<p>Vendors: {5}" +
                                            "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", requestor, department, refNr, content, deadline, vendors);
            }
            else
            {
                output = string.Format("<p>Good Day,</p><p>A possible high value RFQ has been requested</p>" +
                                           "<p>Please find below description for RFQ.</p>" +
                                           "<p>Requestor: {0} </p>" +
                                           "<p>Department: {1} </p>" +
                                           "<p>Reference Number: {2}</p>" +
                                           "<p>Description: {3} </p>" +
                                           "<p>Deadline: {4}</p>" +
                                           "<p>Vendors: {5}" +
                                           "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", requestor, department, refNr, content, deadline, vendors);
            }
            return output;
        }
        private string CreateRFQToVendorText(string content, string deadline, string link, Document file, string refNr)
        {
            var output = "";
            if (file == null)
            {
                output = string.Format("<p>Good Day,</p><p>Please find attached below description for RFQ.</p>" +
                                            "<p>Description: {0} </p>" +
                                            "<p>Deadline: {1}</p>" +
                                            "<p>Please use the following link to upload your quotation</p>" +
                                            "<p>{2}</p>" +
                                            "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", content, deadline, link);
            }
            else
            {
                output = string.Format("<p>Good Day,</p><p>Please find attached below description and specifications document for RFQ.</p>" +
                                            "<p> Description: {0} </p> " +
                                            "<p>Deadline: {1}</p>" +
                                            "<p>Please use the following link to upload your quotation</p>" +
                                            "<p>{2}</p>" +
                                            "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", content, deadline, link);
            }
            return output;
        }
        private string CreateResetPasswordText(string Link)
        {
            var output = "";
            output = string.Format("<p>Good Day,</p><p>You have requested to change your password on the CENORED PROCUREMENT System</p>" +
                                    "<p>Please use the following link to change your password</p>" +
                                    "<p>{0}</p><br/>" +
                                    "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", Link);
            return output;
        }
        private string CreateLoginDetailsText(string userName, string Link)
        {
            var output = "";
            output = string.Format("<p>Good Day,</p><p>You have requested your Login Details on the CENORED PROCUREMENT System</p>" +
                                    "<p>Please find them below</p>" +
                                    "<p>Username: {0}</p>" +
                                    "<p>If you need to reset your password please follow the link below</p>" +
                                    "<p>{1}</p>" +
                                    "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", userName, Link);
            return output;
        }
        private string CreateNewUserText(string userName, string password, string link)
        {
            var output = "";
            output = string.Format("<p>Good Day,</p><p>An account has been created for you on the CENORED PROCUREMENT SYSTEM</p>" +
                                    "<p>Please find Login Details below:</p>" +
                                    "<p>Username: {0}</p>" +
                                    "<p>Password: {1}</p><br/>" +
                                     "<p>Please login to change your password</p>" +
                                    "<p>{2}</p>" +
                                     "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", userName, password, link);
            return output;
        }
        private string CreateInboxNotificationText(string link)
        {
            var output = "";
            output = string.Format("<p>Good Day,</p><p>There are some items in your inbox that require your attention/p>" +
                                    "<p>Please attend to them.</p>" +
                                    "<p>{0}</p>" +
                                     "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", link);
            return output;
        }
        private string CreateRFQReleasedNotificationText(RFQ data, string link)
        {
            var output = "";
            output = string.Format("<p>Good Day,</p><p>The following RFQ has been released and quotations is ready for download</p>" +
                                    "<p>Reference Number: {0}</p>" +
                                    "<p>Name: {1}</p>" +
                                    "<p>Description: {2}</p>" +
                                    "<p>Log in to Procurement System to Download Quotations</p>" +
                                    "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", data.ReferenceNumber, data.RFQRequest.Name, data.RFQRequest.Description, link);
            return output;
        }

        private string CreateTenderCreatedNotificationText(Tender tender)
        {
            var output = "";
            var tendId = "CenTen" + tender.Id;
            output = string.Format("<p>Good Day,</p><p>The following Tender has been created</p>" +
                                    "<p>Tender #: {0}</p>" +
                                    "<p>Name: {1}</p>" +
                                    "<p>Description: {2}</p>" +
                                    "<p>Deadline: {3}</p>" +
                                    "<p>Requestor: {4}</p>" +
                                    "<p>Log in to Procurement System to View Progress</p>" +
                                    "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", tendId,tender.Name,tender.Description,tender.Deadline.ToString("d/M/yyyy HH:mm"), tender.Requestor);
            return output;
        }
        private string CreateTenderInvitationNotificationText(Tender tender)
        {
            var output = "";
            var tendId = "CenTen" + tender.Id;
            output = string.Format("<p>Good Day,</p><p>You have been invited to apply for the following tender</p>" +
                                    "<p>Tender #: {0}</p>" +
                                    "<p>Name: {1}</p>" +
                                    "<p>Description: {2}</p>" +
                                    "<p>Deadline: {3}</p><br/>" +
                                    "<p>Please use the following link to apply for tender:</p>" +
                                    "<p>{4}</p>"+
                                    "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", 
                                    tendId, tender.Name, tender.Description, tender.Deadline.ToString("d/M/yyyy HH:mm"), 
                                    tender.TenderUrl);
            return output;
        }
        private string CreateTenderClosedNotificationText(Tender tender)
        {
            var output = "";
            var tendId = "CenTen" + tender.Id;
            output = string.Format("<p>Good Day,</p><p>The following Tender has been closed</p>" +
                                    "<p>Tender #: {0}</p>" +
                                    "<p>Name: {1}</p>" +
                                    "<p>Description: {2}</p>" +
                                    "<p>Deadline: {3}</p>" +
                                    "<p>Requestor: {4}</p>" +
                                    "<p>Log in to Procurement System to Submissions</p>" +
                                    "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>", tendId, tender.Name, tender.Description, tender.Deadline.ToString("d/M/yyyy HH:mm"), tender.Requestor);
            return output;
        }
        private string CreateTenderAppliedNotificationText(TenderVendor tend)
        {
            var output = "";
            var tendId = "CenTen" + tend.Tender.Id;
            if(tend.Tender.Payable)
            {
                output = string.Format("<p>Good Day,</p><p>You have applied for the following tender</p>" +
                                   "<p>Tender #: {0}</p>" +
                                   "<p>Name: {1}</p>" +
                                   "<p>Description: {2}</p>" +
                                   "<p>Deadline: {3}</p><br/>" +
                                   "<p>The CENORED Procurement Team will review your payment document and send an email once your application is succesful.</p>" +                                   
                                   "<br/><p>NB: All communications are encrypted and confidential." +
                                   "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>",
                                   tendId, tend.Tender.Name, tend.Tender.Description, tend.Tender.Deadline.ToString("d/M/yyyy HH:mm"),
                                   tend.Tender.TenderDocumentUrl, tend.UploadLink);
            }
            else
            {
                output = string.Format("<p>Good Day,</p><p>You have applied for the following tender</p>" +
                                   "<p>Tender #: {0}</p>" +
                                   "<p>Name: {1}</p>" +
                                   "<p>Description: {2}</p>" +
                                   "<p>Deadline: {3}</p>" +
                                   "<p>Please use the following link to download the Tender Specifications Document</p>" +
                                   "<p>{4}</p>" +
                                    "<p>Please use the following link to Submit your Documents</p>" +
                                   "<p>{5}</p>" +
                                   "<br/><p>NB: All communications are encrypted and confidential." +
                                   "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>",
                                   tendId, tend.Tender.Name, tend.Tender.Description, tend.Tender.Deadline.ToString("d/M/yyyy HH:mm"),
                                   tend.Tender.TenderDocumentUrl, tend.UploadLink);
            }
           
            return output;
        }
        private string CreateTenderApplicationApprovedNotificationText(TenderVendor tend)
        {
            var output = "";
            var tendId = "CenTen" + tend.Tender.Id;
            if (tend.Tender.Payable)
            {
                output = string.Format("<p>Good Day,</p><p>Your application for the following Tender as been approved</p>" +
                                  "<p>Tender #: {0}</p>" +
                                   "<p>Name: {1}</p>" +
                                   "<p>Description: {2}</p>" +
                                   "<p>Deadline: {3}</p>" +
                                   "<p>Please use the following link to download the Tender Specifications Document</p>" +
                                   "<p>{4}</p>" +
                                    "<p>Please use the following link to Submit your Documents</p>" +
                                   "<p>{5}</p>" +
                                   "<br/><p>NB: All communications are encrypted and confidential." +
                                   "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>",
                                   tendId, tend.Tender.Name, tend.Tender.Description, tend.Tender.Deadline.ToString("d/M/yyyy HH:mm"),
                                   tend.Tender.TenderDocumentUrl, tend.UploadLink);
            }
            return output;
        }
        private string CreateTenderApplicationDeniedNotificationText(TenderVendor tend)
        {
            var output = "";
            var tendId = "CenTen" + tend.Tender.Id;
            if (tend.Tender.Payable)
            {
                output = string.Format("<p>Good Day,</p><p>Your application for the following Tender has been denied.</p>" +
                                  "<p>Tender #: {0}</p>" +
                                   "<p>Name: {1}</p>" +
                                   "<p>Description: {2}</p>" +
                                   "<p>Please use the following link if you wish to reapply for this tender</p>" +
                                   "<p>{3}</p>" +
                                   "<br/><p>NB: All communications are encrypted and confidential." +
                                   "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>",
                                   tendId, tend.Tender.Name, tend.Tender.Description,tend.Tender.TenderUrl);
            }
            return output;
        }
        private string CreateTenderSubmittedNotificationText(TenderVendor tend)
        {
            var output = "";
            var tendId = "CenTen" + tend.Tender.Id;
            output = string.Format("<p>Good Day,</p><p>You have submitted documents for the following tender</p>" +
                                    "<p>Tender #: {0}</p>" +
                                    "<p>Name: {1}</p>" +
                                    "<p>Description: {2}</p>" +
                                    "<p>Deadline: {3}</p><br/>" +
                                    "<p>CENORED will contact you if your bid has been successful</p>" +
                                    "<br/><p>NB: All communications are encrypted and confidential." +
                                    "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>",
                                    tendId, tend.Tender.Name, tend.Tender.Description, tend.Tender.Deadline.ToString("d/M/yyyy HH:mm"));
            return output;
        }
        private string CreateTenderUpdatedNotificationText(TenderVendor vendor,Tender tender)
        {
            var output = "";
            var tendId = "CenTen" + tender.Id;
            output = string.Format("<p>Good Day,</p><p>The following Tender has been updated</p>" +
                                    "<p>Tender #: {0}</p>" +
                                    "<p>Name: {1}</p>" +
                                    "<p>Description: {2}</p>" +
                                    "<p>Deadline: {3}</p><br/>" +
                                    "<p>Please use the following link to review your submission:</p>" +
                                    "<p>{4}</p>" +
                                    "<p>Kind Regards,<br/>Procurement Team<br/>CENORED</p>",
                                    tendId, tender.Name, tender.Description, tender.Deadline.ToString("d/M/yyyy HH:mm"),
                                    vendor.UploadLink);
            return output;
        }
    }
}
