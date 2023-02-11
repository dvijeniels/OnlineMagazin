﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using OnlineMagazin.Areas.Identity.Data;
using OnlineMagazin.Models;
using OnlineMagazin.Service;

namespace OnlineMagazin.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<OnlineMagazinUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IEmailService _emailService;

        public ResendEmailConfirmationModel(UserManager<OnlineMagazinUser> userManager, IEmailSender emailSender, IEmailService emailService)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _emailService = emailService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Учетная запись с такой электронной почты не найден. Если у вас есть несколько почтовых ящиков, удостоверьтесь, что вы указали правильный адрес электронной почты");
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            string appDomein = "https://pskanker.ru/";
            string confirmationLink = "confirm-email?userId={0}&code={1}";
            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("{{Link}}",string.Format(appDomein+confirmationLink,userId,code))
                        },
            };
            await _emailService.SendEmailConfirmation(options);
            //var callbackUrl = Url.Page(
            //    "/Account/ConfirmEmail",
            //    pageHandler: null,
            //    values: new { userId = userId, code = code },
            //    protocol: Request.Scheme);
            //await _emailSender.SendEmailAsync(
            //    Input.Email,
            //    "Confirm your email",
            //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return RedirectToAction("RegisterConfirmationSend", "Home");
        }
    }
}