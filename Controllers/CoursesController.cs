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
    public class CoursesController : Controller
    {
        private readonly NC_21Context _context;

        public CoursesController(NC_21Context context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var NC_21_Context = _context.Course.Include(c => c.Fields).ThenInclude(f => f.Groups).ThenInclude(g => g.Students);
            
            var lista = new List<Grade>();
            foreach (var c in NC_21_Context)
            {
                foreach(var f in c.Fields)
                {
                    foreach(var g in f.Groups)
                    {
                        foreach(var s in g.Students)
                        {
                            var xd = DateTime.Today;
                            decimal xo = 0;
                            var xcs = _context.CourseStudents.Where(x=>x.CourseId==c.Id & x.StudentId==s.Id) ;
                            if (xcs.Count()>0)
                            {
                                xd = xcs.First().Data;
                                xo = xcs.First().Ocena;
                            }
                            
                            lista.Add(new Grade
                            {
                                StudentId = s.Id,
                                CourseId = c.Id,
                                GroupId = g.Id,
                                I_N = s.I_N,
                                Data = xd,
                                Ocena = xo
                            });
                        }
                    }
                }
            }
            ViewData["listaOcen"] = lista;
            return View(await NC_21_Context.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }

        //metoda do wyświetlania oceniania
        public async Task<IActionResult> TakeGrades(int courseid, int groupid)
        {
            var course = await _context.Course.FindAsync(courseid);
            var NC_21_Context = _context.Course.Where(c => c.Id == courseid).Include(c => c.Fields).ThenInclude(f => f.Groups).ThenInclude(g => g.Students);        
            var lista = new List<Grade>();
            var xkierunek = "";
            var xgr = "";
            var xid = courseid;
            foreach (var c in NC_21_Context)
            {
                foreach (var f in c.Fields)
                {
                    foreach (var g in f.Groups)
                    {
    
                        foreach (var s in g.Students.Where(s=>s.GroupId==groupid) )
                        {
                            xkierunek = f.Nazwa;
                            xgr = g.Nazwa;
                            var xd = DateTime.Today;
                            decimal xo = 0;
                            var xcs = _context.CourseStudents.Where(x => x.CourseId == c.Id & x.StudentId == s.Id);
                            if (xcs.Count() > 0)
                            {
                                xd = xcs.First().Data;
                                xo = xcs.First().Ocena;
                            }

                            lista.Add(new Grade
                            {
                                StudentId = s.Id,
                                CourseId = c.Id,
                                GroupId = g.Id,
                                I_N = s.I_N,
                                Data = xd,
                                Ocena = xo
                            });
                        }
                    }
                }
            }
            ViewData["course"] = xid;
            ViewData["field"] = xkierunek;
            ViewData["group"] = xgr;
            ViewData["listaOcen"] = lista;
            return View(course);
        }

        //metoda do zapisywania oceniania
        public async Task<IActionResult> SaveGrades(int id)
        {
            decimal xocena = 0;
            int xid = 0;
            var xdata = DateTime.Today;
            var course = await _context.Course.FindAsync(id);
            var oceny = HttpContext.Request.Form["listaOcen"];
            var studenci = HttpContext.Request.Form["listaStudentow"];
            var CS = new List<CourseStudent>();
            int ile = studenci.Count();
            for (int i=0;i<ile;i++)
            {
                xid = int.Parse(studenci[i]);
                xocena = decimal.Parse(oceny[i]);
                var xcs =_context.CourseStudents.Where(c => c.StudentId == xid & c.CourseId == id);
                if (xcs.Any())
                {
                    var xgrade = _context.CourseStudents.Where(c => c.StudentId == xid & c.CourseId == id).Single();
                    xgrade.Ocena = xocena;
                    xgrade.Data = DateTime.Today;
                    _context.Update(xgrade);
                }
                else
                {
                    var cs = new CourseStudent();
                    cs.StudentId = xid;
                    cs.CourseId = id;
                    cs.Ocena = xocena;
                    cs.Data = DateTime.Today;
                    _context.Add(cs);
                };
                    
                
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
