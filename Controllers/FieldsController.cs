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
    public class FieldsController : Controller
    {
        private readonly NC_21Context _context;

        public FieldsController(NC_21Context context)
        {
            _context = context;
        }

        // GET: Fields
        public async Task<IActionResult> Index()
        {
            var nC_21Context = _context.Field.Include(f => f.Institut).Include(f => f.Groups).ThenInclude(g => g.Students).Include(c=>c.Courses);
            return View(await nC_21Context.ToListAsync());
        }

        // GET: Fields/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @field = await _context.Field
                .Include(f => f.Institut)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@field == null)
            {
                return NotFound();
            }

            return View(@field);
        }

        // GET: Fields/Create
        public IActionResult Create()
        {
            ViewData["InstitutId"] = new SelectList(_context.Institut, "Id", "Nazwa");
            GetCourseList();
            return View();
        }

        // POST: Fields/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,InstitutId,Courses")] Field @field)
        {
            var kursy = HttpContext.Request.Form["selectedCourses"];
            var xid = @field.Id;
            @field.Courses = new List<Course>();
            foreach (var k in kursy)
            {
                var xwyb = _context.Course.Single(c => c.Id == int.Parse(k));
                @field.Courses.Add(xwyb);
            }

            if (ModelState.IsValid)
            {
                _context.Add(@field);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            
            ViewData["InstitutId"] = new SelectList(_context.Institut, "Id", "Nazwa", @field.InstitutId);

            return View(@field);
        }

        // GET: Fields/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @field = await _context.Field.FindAsync(id);
            if (@field == null)
            {
                return NotFound();
            }
            ViewData["InstitutId"] = new SelectList(_context.Institut, "Id", "Nazwa", @field.InstitutId);
            GetSelectedCourseList(id);
            return View(@field);
        }

        // POST: Fields/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,InstitutId,Courses")] Field @field)
        {
            if (id != @field.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@field);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FieldExists(@field.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var kursy = HttpContext.Request.Form["selectedCourses"];
                var xf = _context.Field.Include(f => f.Courses);
                var xf1 = xf.Single(x => x.Id == id);
                var xc1 = xf1.Courses;
                xc1.Clear();
                foreach (var k in kursy)
                {
                    var xwyb = _context.Course.Single(c => c.Id == int.Parse(k));
                    xc1.Add(xwyb);
                }
                _context.Update(xf1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstitutId"] = new SelectList(_context.Institut, "Id", "Nazwa", @field.InstitutId);
            ViewData["Courses"] = new SelectList(_context.Course, "Id", "Nazwa");
            return View(@field);
        }

        // GET: Fields/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @field = await _context.Field
                .Include(f => f.Institut)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@field == null)
            {
                return NotFound();
            }

            return View(@field);
        }

        // POST: Fields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @field = await _context.Field.FindAsync(id);
            _context.Field.Remove(@field);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FieldExists(int id)
        {
            return _context.Field.Any(e => e.Id == id);
        }

        private void GetCourseList()
        {
            var allcourses = _context.Course;
            var Kursy = new List<C>();
            foreach (var c in allcourses)
            {
                Kursy.Add(new C
                {
                    CourseId = c.Id,
                    Nazwa = c.Nazwa,
                    Checked = ""
                });
            }
            ViewData["Courses"] = Kursy;
        }

        private void GetSelectedCourseList(int? id)
        {
            var allcourses = _context.Course;
            var Kursy = new List<C>();
            foreach (var c in allcourses)
            {
                var xcourse = allcourses.Single(a=>a.Id==c.Id);
                var xch = "";
                var xfc = _context.Field.Include(f=>f.Courses).Single(f => f.Id == id);
                if (xfc.Courses.Contains(xcourse)) { xch = "checked"; } else { xch = ""; }
                Kursy.Add(new C
                {
                    CourseId = c.Id,
                    Nazwa = c.Nazwa,
                    Checked = xch
                    
                });
            }
            ViewData["Courses"] = Kursy;
        }


    }
}
