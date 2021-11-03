using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NC_21.Models;

namespace NC_21.Controllers
{
    public class InstitutsController : Controller
    {
        private readonly NC_21Context _context;

        public InstitutsController(NC_21Context context)
        {
            _context = context;
        }

        // GET: Instituts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Institut.ToListAsync());
        }

        // GET: Instituts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institut = await _context.Institut
                .FirstOrDefaultAsync(m => m.Id == id);
            if (institut == null)
            {
                return NotFound();
            }

            return View(institut);
        }

        // GET: Instituts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instituts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa")] Institut institut)
        {
            if (ModelState.IsValid)
            {
                _context.Add(institut);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(institut);
        }

        // GET: Instituts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institut = await _context.Institut.FindAsync(id);
            if (institut == null)
            {
                return NotFound();
            }
            return View(institut);
        }

        // POST: Instituts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa")] Institut institut)
        {
            if (id != institut.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(institut);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstitutExists(institut.Id))
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
            return View(institut);
        }

        // GET: Instituts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institut = await _context.Institut
                .FirstOrDefaultAsync(m => m.Id == id);
            if (institut == null)
            {
                return NotFound();
            }

            return View(institut);
        }

        // POST: Instituts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var institut = await _context.Institut.FindAsync(id);
            _context.Institut.Remove(institut);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstitutExists(int id)
        {
            return _context.Institut.Any(e => e.Id == id);
        }
    }
}
