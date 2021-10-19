using PromotionsWebApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Settings
{
    public class EmailMessage
    {
        public MailboxAddress From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Document Attachment { get; set; }
        public string UploadLink { get; set; }
        public EmailMessage(string to, string subject)
        {
            To = to;
            Subject = subject;
        }
        public EmailMessage(string to, string subject, string text)
        {
            To = to;
            Subject = subject;
            Content = text;
        }
        public EmailMessage(string to, string subject, string content, Document attachment, string uploadLink)
        {
            To = to;
            Subject = subject;
            Content = content;
            Attachment = attachment;
            UploadLink = uploadLink;
        }
        public EmailMessage(string to, string subject, string content, Document attachment)
        {
            To = to;
            Subject = subject;
            Content = content;
            Attachment = attachment;
            UploadLink = "";
        }

       
    }
}
