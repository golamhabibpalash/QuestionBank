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
    public class AcademicClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AcademicClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AcademicClasses
        public async Task<IActionResult> Index(int? classId)
        {
            List<AcademicClass> academicClasses = null;
            if (classId == null)
            {
                academicClasses = await _context.AcademicClasses
                    .Include(s => s.AcademicSubjects)
                        .ThenInclude(p => p.Chapters)
                            .ThenInclude(q => q.Questions).ToListAsync();
            }
            else
            {
                academicClasses = await _context.AcademicClasses
                    .Where(a => a.Id == classId)
                    .Include(s => s.AcademicSubjects)
                        .ThenInclude(p => p.Chapters)
                            .ThenInclude(q => q.Questions).ToListAsync();
            }
            return View(academicClasses);
            Problem("Entity set 'ApplicationDbContext.AcademicClasses'  is null.");
        }

        // GET: AcademicClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AcademicClasses == null)
            {
                return NotFound();
            }

            var academicClass = await _context.AcademicClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academicClass == null)
            {
                return NotFound();
            }

            return View(academicClass);
        }

        // GET: AcademicClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AcademicClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassName,Id,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy")] AcademicClass academicClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academicClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(academicClass);
        }

        // GET: AcademicClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AcademicClasses == null)
            {
                return NotFound();
            }

            var academicClass = await _context.AcademicClasses.FindAsync(id);
            if (academicClass == null)
            {
                return NotFound();
            }
            return View(academicClass);
        }

        // POST: AcademicClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClassName,Id,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy")] AcademicClass academicClass)
        {
            if (id != academicClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicClassExists(academicClass.Id))
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
            return View(academicClass);
        }

        // GET: AcademicClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AcademicClasses == null)
            {
                return NotFound();
            }

            var academicClass = await _context.AcademicClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academicClass == null)
            {
                return NotFound();
            }

            return View(academicClass);
        }

        // POST: AcademicClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AcademicClasses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AcademicClasses'  is null.");
            }
            var academicClass = await _context.AcademicClasses.FindAsync(id);
            if (academicClass != null)
            {
                _context.AcademicClasses.Remove(academicClass);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademicClassExists(int id)
        {
          return (_context.AcademicClasses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
