using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
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
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace OnlineMagazin.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<OnlineMagazinUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ForgotPasswordModel(UserManager<OnlineMagazinUser> userManager, IEmailService emailService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    ModelState.AddModelError(string.Empty, "Учетная запись с такой электронной почты не найден. Если у вас есть несколько почтовых ящиков, удостоверьтесь, что вы указали правильный адрес электронной почты");
                    return Page();
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                string appDomein = _configuration.GetSection("UserConfirmationData:AppDomein").Value;
                string confirmationLink = "Identity/Account/ResetPassword?code={0}";
                UserEmailOptions options = new UserEmailOptions
                {
                    ToEmails = new List<string>() { user.Email },
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("{{Email}}",user.Email),
                            new KeyValuePair<string, string>("{{Link}}",string.Format(appDomein+confirmationLink,code))
                        },
                };
                await _emailService.SendForgotPassword(options);
                //var callbackUrl = Url.Page(
                //    "/Account/ResetPassword",
                //    pageHandler: null, 
                //    values: new { area = "Identity", code },
                //    protocol: Request.Scheme);

                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Сброс пароля",
                //    $"Пожалуйста, сбросьте пароль через <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите сюда</a>.");

                return RedirectToAction("ForgetPasswordConfirmationSend");
            }

            return Page();
        }
    }
}
