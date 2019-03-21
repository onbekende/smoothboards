using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectSmoothboard23.Models;
using System.Net.Mail;
using System.Net;

namespace ProjectSmoothboard23.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SmoothboardDBContext _context;

        public ProductsController(SmoothboardDBContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.productid == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("productid,productname,description,prijs")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Maildata()
        {
            int productid = Convert.ToInt32(Request.Form["productid"]);
            string naam =  Request.Form["name"];
            string telnr = Request.Form["telnr"];
            string productnaam = Request.Form["productname"];
            string email = Request.Form["email"];

            if (ModelState.IsValid)
            {
                //SMTP server en poort 587
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new NetworkCredential("smoothboardinfo@gmail.com", "Smoothboard012!");
                smtpClient.EnableSsl = true;
                //Gmail werkt met Securityprotocol TLS
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                //smtpClient.UseDefaultCredentials = false;

                MailMessage mail = new MailMessage();
                mail.Body = 
                    "<header style='height: 30px; background: #EAEAEA; width: 100%; text-align: center;'>Smoothboard Product informatie</header>" + 
                    "<h3>Beste Sean,<br/>" +
                    "<br/>" + naam +
                    " Heeft een vraag over <strong>" + productnaam + "</strong>.<br/>" +
                    "<img src='https://via.placeholder.com/150' width='65px'>" +
                    "Neem contact op via mail of per telefoon <br/>" +
                    telnr + "<br/><br/>" +
                    "Met vriendelijke groet,<br/" +
                    "<br/>" +
                    naam +
                    "<footer style='color: #DD6E42; width: 100%; text-align:center; background: #EAEAEA; height: 30px; '>Smoothboards</footer>";
                mail.Subject = "Inschrijving Nieuwsbrief";
                mail.IsBodyHtml = true;
                //Setting From , To and CC
                mail.From = new MailAddress("smoothboardinfo@gmail.com", "Smoothboard styler NO-REPLY");
                mail.To.Add(new MailAddress("smoothboardinfo@gmail.com"));
                mail.CC.Add(new MailAddress(email));

                smtpClient.Send(mail);
                
            }
            return RedirectToAction("Products", "Home");
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("productid,productname,description,prijs")] Product product)
        {
            if (id != product.productid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.productid))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.productid == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.productid == id);
        }
    }
}
