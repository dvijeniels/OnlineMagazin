using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineMagazin.Areas.Identity.Data;
using OnlineMagazin.Data;
using OnlineMagazin.Models;
using OnlineMagazin.Service;

namespace OnlineMagazin.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly OnlineMagazinContext _context;
        private readonly SignInManager<OnlineMagazinUser> _signInManager;
        private readonly UserManager<OnlineMagazinUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public RegisterModel(OnlineMagazinContext context,
            UserManager<OnlineMagazinUser> userManager,
            SignInManager<OnlineMagazinUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailService emailService, 
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailService = emailService;
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "Введите ФИО.")]
            [Display(Name = "ФИО")]
            public string FirstAndLastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Ваш пароль должен содержать не менее 6-ти символов (букв и цифр).", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтвердить пароль")]
            [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new OnlineMagazinUser { UserName = Input.Email, Email = Input.Email };
                user.FirstAndLastName = Input.FirstAndLastName;
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, OnlineMagazinRole.Roles.User.ToString());
                    _logger.LogInformation("Пользователь создал новую учетную запись с паролем.");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);
                    string appDomein = _configuration.GetSection("UserConfirmationData:AppDomein").Value;
                    string confirmationLink = _configuration.GetSection("UserConfirmationData:ConfirmationLink").Value;
                    UserEmailOptions options = new UserEmailOptions
                    {
                        ToEmails = new List<string>() { user.Email },
                        PlaceHolders=new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("{{Link}}",string.Format(appDomein+confirmationLink,user.Id,code))
                        },
                    };
                    await _emailService.SendEmailConfirmation(options);
                    Mails mails= new Mails();
                    mails.Mail = Input.Email;
                    mails.Status = true;
                    _context.Add(mails);
                    await _context.SaveChangesAsync();
                    //await _emailSender.SendEmailAsync(Input.Email, "Подтвердите ваш адрес электронной почты",
                    //    $"Пожалуйста, подтвердите свой аккаунт через <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите сюда</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        return RedirectToAction("RegisterConfirmationSend", "Home");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
