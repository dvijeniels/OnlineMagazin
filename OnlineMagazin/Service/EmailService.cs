using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using NuGet.Protocol.Plugins;
using OnlineMagazin.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMagazin.Service
{
    public class EmailService : IEmailService
    {
        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly SMTPConfigModel _smtpConfig;

        public async Task SendTestEmail(UserEmailOptions userEmailOptions)
        {
            //userEmailOptions.Body = UpdatePlaceHolders(userEmailOptions.Body, userEmailOptions.PlaceHolders);
            await SendEmailEveryone(userEmailOptions);
        }
        public async Task SendEmailConfirmation(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = "Для завершения регистрации подтвердите свой email";
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirm"), userEmailOptions.PlaceHolders);
            await SendEmail(userEmailOptions);
        }
        public async Task SendForgotPassword(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = "Сброс пароля";
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword"), userEmailOptions.PlaceHolders);
            await SendEmail(userEmailOptions);
        }
        public EmailService(IOptions<SMTPConfigModel> _smtpConfig)
        {
            this._smtpConfig = _smtpConfig.Value;
        }
        private async Task SendEmailEveryone(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML
            };
            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
                mail.Body = "";
                NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);
                mail.Body += userEmailOptions.Body+ "<br><br><a href=https://pskanker.ru/Home/UnSubcribe?mail=" + toEmail + ">Отписаться от рассылок</a>";
                SmtpClient smtpClient = new SmtpClient
                {
                    Host = _smtpConfig.Host,
                    Port = _smtpConfig.Port,
                    EnableSsl = _smtpConfig.EnableSSL,
                    UseDefaultCredentials = _smtpConfig.UserDefaultCredentials,
                    Credentials = networkCredential,
                };

                mail.BodyEncoding = Encoding.Default;

                await smtpClient.SendMailAsync(mail);
            }
        }
        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {

            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML
            };
            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }
            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password /*"vectormarketstroy@gmail.com", "bietemqtvzhxxjvk"*/);
            
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UserDefaultCredentials,
                Credentials = networkCredential,
                //DeliveryMethod = SmtpDeliveryMethod.Network
            };

            mail.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mail);
            
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }
        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeHolder in keyValuePairs)
                {
                    if (text.Contains(placeHolder.Key))
                    {
                        text = text.Replace(placeHolder.Key, placeHolder.Value);
                    }

                }
            }
            return text;
        }
    }
}
