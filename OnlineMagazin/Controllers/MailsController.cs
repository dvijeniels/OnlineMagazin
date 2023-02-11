using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;
using OnlineMagazin.Service;

namespace OnlineMagazin.Controllers
{
    public class MailsController : Controller
    {
        private readonly OnlineMagazinContext _context;
        private readonly IEmailService _emailService;

        public MailsController(OnlineMagazinContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public IActionResult Subcribe()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Subcribe(string Subject, string EmailTemplate)
        {
            var mailsFrom =_context.Mails.Where(a=>a.Status==true).Select(a=>a.Mail).ToList();
            UserEmailOptions options = new UserEmailOptions
            {
                Body = EmailTemplate,
                Subject = Subject,
                ToEmails = mailsFrom,
            };
            await _emailService.SendTestEmail(options);
            return RedirectToAction(nameof(Index));
        }
        // GET: Mails
        public async Task<IActionResult> Index()
        {
            //UserEmailOptions options = new UserEmailOptions
            //{
            //    ToEmails = new List<string>() { "djadi708@gmail.com" }
            //};
            //await _emailService.SendTestEmail(options);
            return View(await _context.Mails.ToListAsync());
        }

        // GET: Mails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Mails == null)
            {
                return NotFound();
            }

            var mails = await _context.Mails
                .FirstOrDefaultAsync(m => m.MailId == id);
            if (mails == null)
            {
                return NotFound();
            }

            return View(mails);
        }

        // GET: Mails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MailId,Mail,Status")] Mails mails)
        {
            if (ModelState.IsValid)
            {
                var result=_context.Mails.Where(mail => mail.Mail == mails.Mail).Count();
                if (result == 0)
                {
                    _context.Add(mails);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(mails);
        }

        // GET: Mails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Mails == null)
            {
                return NotFound();
            }

            var mails = await _context.Mails.FindAsync(id);
            if (mails == null)
            {
                return NotFound();
            }
            return View(mails);
        }

        // POST: Mails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MailId,Mail,Status")] Mails mails)
        {
            if (id != mails.MailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MailsExists(mails.MailId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mails);
        }

        // GET: Mails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mails == null)
            {
                return NotFound();
            }

            var mails = await _context.Mails
                .FirstOrDefaultAsync(m => m.MailId == id);
            if (mails == null)
            {
                return NotFound();
            }

            return View(mails);
        }

        // POST: Mails/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (_context.Mails == null)
            {
                return Problem("Entity set 'OnlineMagazinContext.Mails'  is null.");
            }
            var mails = _context.Mails.Find(id);
            if (mails != null)
            {
                _context.Mails.Remove(mails);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool MailsExists(int id)
        {
          return _context.Mails.Any(e => e.MailId == id);
        }
    }
}
