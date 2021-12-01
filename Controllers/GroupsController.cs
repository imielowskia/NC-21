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
    public class GroupsController : Controller
    {
        private readonly NC_21Context _context;

        public GroupsController(NC_21Context context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            var nC_21Context = _context.Group.Include(g => g.Students).Include(g => g.Field).ThenInclude(f => f.Institut);
            return View(await nC_21Context.ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id, int typ=0, int xcr = -1)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .Include(g => g.Field).ThenInclude(f=>f.Courses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewData["typ"] = typ;
            ViewData["cr"] = xcr;
            if (typ == 2)
            {
                int mid = 0;
                decimal xocena = 0;
                int xstudentid = 0;
                var oceny = HttpContext.Request.Form["Ocena"];
                var studenci = HttpContext.Request.Form["StudentId"];
                var xid = HttpContext.Request.Form["Id_cs"];
                for (int i=0; i < oceny.Count(); i++)
                {
                    xocena = decimal.Parse(oceny[i]);
                    if (xocena > 0)
                    {
                        mid = int.Parse(xid[i]);
                        xstudentid = int.Parse(studenci[i]);
                        var xcs = _context.CSGradeDetails.Where(cs => cs.Id == mid);
                        if (mid == -1)
                        {
                            var cs = new CSGradeDetail();
                            cs.CourseId = xcr;
                            cs.StudentId = xstudentid;
                            cs.Ocena = xocena;
                            cs.Data = DateTime.Today;
                            _context.Add(cs);
                        }
                        else
                        {
                            var xcs1 = _context.CSGradeDetails.Where(cs => cs.Id == mid).Single();
                            if (xcs1.Ocena != xocena)
                            {
                                xcs1.Ocena = xocena;
                                xcs1.Data = DateTime.Today;
                                _context.Update(xcs1);
                            }
                        }
                    }
                    
                }
                await _context.SaveChangesAsync();
                typ = 0;
                ViewData["typ"] = typ;
            }
            return View(@group);
        }

      

        // GET: Groups/Create
        public IActionResult Create()
        {
            ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Nazwa");
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,FieldId")] Group @group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Nazwa", @group.FieldId);
            
            return View(@group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Nazwa", @group.FieldId);
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,FieldId")] Group @group)
        {
            if (id != @group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.Id))
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
            ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Nazwa", @group.FieldId);
            return View(@group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .Include(g => g.Field)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @group = await _context.Group.FindAsync(id);
            _context.Group.Remove(@group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return _context.Group.Any(e => e.Id == id);
        }
    }
}
