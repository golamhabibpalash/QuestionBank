using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using questionBank.Application.Data;
using questionBank.Application.Models;

namespace questionBank.Application.Controllers
{
    public class AcademicSubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AcademicSubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AcademicSubjects
        public async Task<IActionResult> Index(int? classId)
        {
            List<AcademicSubject> academicSubjects = null;

            if (classId == null)
            {
                academicSubjects = await _context.AcademicSubjects
                    .Include(c => c.Chapters)
                        .ThenInclude(d => d.Questions)
                            .Include(a => a.AcademicClass).ToListAsync();
            }
            else
            {
                academicSubjects = await _context.AcademicSubjects
                    .Where(c => c.AcademicClassId == classId)
                    .Include(c => c.Chapters)
                        .ThenInclude(d => d.Questions)
                            .Include(a => a.AcademicClass).ToListAsync();

                AcademicClass ac = await _context.AcademicClasses.FirstOrDefaultAsync(c => c.Id == classId);
                ViewBag.aClass = "";
                if (ac!=null)
                {
                    ViewBag.aClass = ac.ClassName;
                }
            }

            return View(academicSubjects);
        }

        // GET: AcademicSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AcademicSubjects == null)
            {
                return NotFound();
            }

            var academicSubject = await _context.AcademicSubjects
                .Include(a => a.AcademicClass)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academicSubject == null)
            {
                return NotFound();
            }

            return View(academicSubject);
        }

        // GET: AcademicSubjects/Create
        public IActionResult Create()
        {
            ViewData["AcademicClassId"] = new SelectList(_context.AcademicClasses, "Id", "ClassName");
            return View();
        }

        // POST: AcademicSubjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubjectName,AcademicClassId,Id,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy")] AcademicSubject academicSubject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academicSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AcademicClassId"] = new SelectList(_context.AcademicClasses, "Id", "ClassName", academicSubject.AcademicClassId);
            return View(academicSubject);
        }

        // GET: AcademicSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AcademicSubjects == null)
            {
                return NotFound();
            }

            var academicSubject = await _context.AcademicSubjects.FindAsync(id);
            if (academicSubject == null)
            {
                return NotFound();
            }
            ViewData["AcademicClassId"] = new SelectList(_context.AcademicClasses, "Id", "ClassName", academicSubject.AcademicClassId);
            return View(academicSubject);
        }

        // POST: AcademicSubjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubjectName,AcademicClassId,Id,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy")] AcademicSubject academicSubject)
        {
            if (id != academicSubject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicSubjectExists(academicSubject.Id))
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
            ViewData["AcademicClassId"] = new SelectList(_context.AcademicClasses, "Id", "ClassName", academicSubject.AcademicClassId);
            return View(academicSubject);
        }

        // GET: AcademicSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AcademicSubjects == null)
            {
                return NotFound();
            }

            var academicSubject = await _context.AcademicSubjects
                .Include(a => a.AcademicClass)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academicSubject == null)
            {
                return NotFound();
            }

            return View(academicSubject);
        }

        // POST: AcademicSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AcademicSubjects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AcademicSubjects'  is null.");
            }
            var academicSubject = await _context.AcademicSubjects.FindAsync(id);
            if (academicSubject != null)
            {
                _context.AcademicSubjects.Remove(academicSubject);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademicSubjectExists(int id)
        {
          return (_context.AcademicSubjects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
