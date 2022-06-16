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
    public class ChaptersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChaptersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Chapters
        public async Task<IActionResult> Index()
        {

            ViewData["AcademicClassId"] = new SelectList(_context.AcademicClasses, "Id", "ClassName");

            var applicationDbContext = _context.Chapters
                .Include(q => q.Questions)
                .Include(c => c.AcademicSubject)
                 .ThenInclude(ac => ac.AcademicClass)
                 .OrderBy(a => a.AcademicSubject.AcademicClass)
                 .ThenBy(p => p.AcademicSubject)
                 .ThenBy(c => c.ChapterNumber);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Chapters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Chapters == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters
                .Include(c => c.AcademicSubject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        // GET: Chapters/Create
        public IActionResult Create()
        {
            ViewData["AcademicSubjectId"] = new SelectList(_context.AcademicSubjects, "Id", "SubjectName");
            return View();
        }

        // POST: Chapters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChapterName,ChapterNumber,AcademicSubjectId,Id,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chapter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AcademicSubjectId"] = new SelectList(_context.AcademicSubjects, "Id", "SubjectName", chapter.AcademicSubjectId);
            return View(chapter);
        }

        // GET: Chapters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Chapters == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }
            ViewData["AcademicSubjectId"] = new SelectList(_context.AcademicSubjects, "Id", "SubjectName", chapter.AcademicSubjectId);
            return View(chapter);
        }

        // POST: Chapters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChapterName,ChapterNumber,AcademicSubjectId,Id,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy")] Chapter chapter)
        {
            if (id != chapter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chapter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChapterExists(chapter.Id))
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
            ViewData["AcademicSubjectId"] = new SelectList(_context.AcademicSubjects, "Id", "SubjectName", chapter.AcademicSubjectId);
            return View(chapter);
        }

        // GET: Chapters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Chapters == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters
                .Include(c => c.AcademicSubject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        // POST: Chapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chapters == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Chapters'  is null.");
            }
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter != null)
            {
                _context.Chapters.Remove(chapter);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChapterExists(int id)
        {
          return (_context.Chapters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
