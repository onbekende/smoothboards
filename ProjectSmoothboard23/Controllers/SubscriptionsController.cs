using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectSmoothboard23.Models;

namespace ProjectSmoothboard23.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly SmoothboardDBContext _context;

        public SubscriptionsController(SmoothboardDBContext context)
        {
            _context = context;
        }

        // GET: Subscriptions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Subscription.ToListAsync());
        }

        // GET: Subscriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscription
                .FirstOrDefaultAsync(m => m.id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // GET: Subscriptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subscriptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,lastname,gender,country,email")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subscription);
                await _context.SaveChangesAsync();
                //SMTP server en poort 587
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential("smoothboardinfo@gmail.com", "Smoothboard012!");
                smtpClient.EnableSsl = true;
                //Gmail werkt met Securityprotocol TLS
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                //smtpClient.UseDefaultCredentials = false;

                MailMessage mail = new MailMessage();
                mail.Body = "Beste " + subscription.name+ " " + subscription.lastname + ", <br/>" +
                     "<br/>" +
                     " Dank je voor het inschrijven bij de SmoothBoards.<br/>" +
                     "Uw krijgt iedere week op vrijdag een vernieuwde nieuwsbrief voor leuke updates.<br/>" +
                     "<br/>" +
                     "Wilt u geen nieuwsbrieven klik dan <a href='https://localhost:44380/Subscriptions/Delete/" + subscription.id+ "'>hier</a>!<br/>" +
                     "<br/>" +
                     "Met vriendelijke groet,<br/" +
                     "<br/>" +
                     "Het SmoothBoards team" +
                     "";
                mail.Subject = "Inschrijving Nieuwsbrief";
                mail.IsBodyHtml = true;
                //Setting From , To and CC
                mail.From = new MailAddress("smoothboardinfo@gmail.com", "Smoothboard styler NO-REPLY");
                mail.To.Add(new MailAddress(subscription.email));

                smtpClient.Send(mail);
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }

       


        // GET: Subscriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscription.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            return View(subscription);
        }

        // POST: Subscriptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,lastname,gender,country,email")] Subscription subscription)
        {
            if (id != subscription.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionExists(subscription.id))
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
            return View(subscription);
        }

        // GET: Subscriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscription
                .FirstOrDefaultAsync(m => m.id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // POST: Subscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subscription = await _context.Subscription.FindAsync(id);
            _context.Subscription.Remove(subscription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionExists(int id)
        {
            return _context.Subscription.Any(e => e.id == id);
        }
    }
}
