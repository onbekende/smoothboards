using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectSmoothboard23.Models;

namespace ProjectSmoothboard23.Controllers
{
    public class HomeController : Controller
    {
        private readonly SmoothboardDBContext _context;

        public HomeController(SmoothboardDBContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }



        public IActionResult Contact()
        {
            return View();
        }


        public IActionResult Newsletter()
        {
            return View();
        }

        public async Task<IActionResult> Products()
        {
            return View(await _context.Product.ToListAsync());
        }

        public async Task<IActionResult> Product(int? id)
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

            return View("Productpage", product);
        }

        public async Task<IActionResult> FAQ()
        {
            return View(await _context.FAQ.ToListAsync());
        }
    }
}
