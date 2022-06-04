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
    public class QuestionDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: QuestionDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.QuestionDetails.Include(q => q.Question).Include(q => q.QuestionType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: QuestionDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.QuestionDetails == null)
            {
                return NotFound();
            }

            var questionDetail = await _context.QuestionDetails
                .Include(q => q.Question)
                .Include(q => q.QuestionType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionDetail == null)
            {
                return NotFound();
            }

            return View(questionDetail);
        }

        // GET: QuestionDetails/Create
        public IActionResult Create()
        {
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id");
            ViewData["QuestionTypeId"] = new SelectList(_context.QuestionTypes, "Id", "Id");
            return View();
        }

        // POST: QuestionDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionText,QuestionId,QuestionTypeId,Id,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy")] QuestionDetail questionDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(questionDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", questionDetail.QuestionId);
            ViewData["QuestionTypeId"] = new SelectList(_context.QuestionTypes, "Id", "Id", questionDetail.QuestionTypeId);
            return View(questionDetail);
        }

        // GET: QuestionDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.QuestionDetails == null)
            {
                return NotFound();
            }

            var questionDetail = await _context.QuestionDetails.FindAsync(id);
            if (questionDetail == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", questionDetail.QuestionId);
            ViewData["QuestionTypeId"] = new SelectList(_context.QuestionTypes, "Id", "Id", questionDetail.QuestionTypeId);
            return View(questionDetail);
        }

        // POST: QuestionDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestionText,QuestionId,QuestionTypeId,Id,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy")] QuestionDetail questionDetail)
        {
            if (id != questionDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionDetailExists(questionDetail.Id))
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
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", questionDetail.QuestionId);
            ViewData["QuestionTypeId"] = new SelectList(_context.QuestionTypes, "Id", "Id", questionDetail.QuestionTypeId);
            return View(questionDetail);
        }

        // GET: QuestionDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.QuestionDetails == null)
            {
                return NotFound();
            }

            var questionDetail = await _context.QuestionDetails
                .Include(q => q.Question)
                .Include(q => q.QuestionType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionDetail == null)
            {
                return NotFound();
            }

            return View(questionDetail);
        }

        // POST: QuestionDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.QuestionDetails == null)
            {
                return Problem("Entity set 'ApplicationDbContext.QuestionDetails'  is null.");
            }
            var questionDetail = await _context.QuestionDetails.FindAsync(id);
            if (questionDetail != null)
            {
                _context.QuestionDetails.Remove(questionDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionDetailExists(int id)
        {
          return (_context.QuestionDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
