using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NC_21.Models;

namespace NC_21.ViewComponents
{
    public class GradeDetailsViewComponent:ViewComponent
    {
        private readonly NC_21Context _context;

        public GradeDetailsViewComponent(NC_21Context context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int courseid, int groupid, int typ=0)
        {
            string xview = "Default";
            var lista =  _context.CSGradeDetails.Where(cs=>cs.CourseId==courseid);
            var listgr = await _context.Student.Where(s => s.GroupId == groupid).ToListAsync();
            
            var CSList = new List<CS>();
            foreach (var s in listgr)
            {
                if (lista.Where(l => l.StudentId == s.Id).Any())
                {
                    foreach (var cs in lista.Where(l => l.StudentId == s.Id))
                    {
                        CSList.Add(new CS
                        {
                            Id = cs.Id,
                            CourseId = cs.CourseId,
                            StudentId = cs.StudentId,
                            GroupId = cs.Student.GroupId,
                            I_N = cs.Student.I_N,
                            Ocena = cs.Ocena,
                            Data = cs.Data
                        });
                    }
                }
                
                
                    CSList.Add(new CS
                    {

                        Id = -1,
                        CourseId = courseid,
                        StudentId = s.Id,
                        GroupId = s.GroupId,
                        I_N = s.I_N,
                        Ocena = 0,
                        Data = DateTime.Today
                    });
                
            }
            if (typ == 0)
            {
                xview = "Default";

            }
            if (typ == 1)
            {
                typ = 0;                
                xview = "Edit";
            }
            ViewData["typ"] = typ;
            ViewData["lista"] = CSList;
            ViewData["courseid"] = courseid;
            ViewData["groupid"] = groupid;
            return View(xview, lista);
        }
    }
}

