using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using questionBank.Application.Data;
using questionBank.Application.Models;
using questionBank.Application.ViewModel;
using Microsoft.EntityFrameworkCore.Query;


namespace questionBank.Application.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index(int? chapterId, int? subjectId, int? classId)
        {
            var applicationDbContext = _context.Questions
                .Include(q => q.QuestionDetails)
                .Include(q => q.Chapter)
                    .ThenInclude(q => q.AcademicSubject)
                        .ThenInclude(p => p.AcademicClass);

            if (chapterId != null)
            {
                applicationDbContext = (IIncludableQueryable<Question, AcademicClass?>)(from q in applicationDbContext
                                       where q.ChapterId == chapterId
                                       select q);
            }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Chapter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            ViewData["AcademicClassId"] = new SelectList(_context.AcademicClasses, "Id", "ClassName");
            ViewData["AcademicSubjectId"] = new SelectList(_context.AcademicSubjects, "Id", "SubjectName");
            ViewData["ChapterId"] = new SelectList(_context.Chapters, "Id", "ChapterName");

            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Uddipok,Image,ImagePosition,ChapterId,AcademicClassId,AcademicSubjectId,QuestionDetails,Id,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy")] QuestionVM questionVM)
        {
            

            Question question = new Question();
            if (ModelState.IsValid)
            {
                question.ChapterId = questionVM.ChapterId;
                question.Uddipok = questionVM.Uddipok;
                int i = 1;
                foreach (var qDetail in questionVM.QuestionDetails)
                {
                    
                    QuestionDetail questionDetail = new QuestionDetail();

                    questionDetail.QuestionText = qDetail.QuestionText;
                    questionDetail.Question = question;
                    questionDetail.QuestionId = qDetail.QuestionId;
                    questionDetail.QuestionTypeId = i;
                    questionDetail.CreatedAt = DateTime.Now;
                    questionDetail.CreatedBy = "User";

                    question.QuestionDetails.Add(questionDetail);
                    i++;
                }                

                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChapterId"] = new SelectList(_context.Chapters, "Id", "Id", question.ChapterId);
            return View(questionVM);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["ChapterId"] = new SelectList(_context.Chapters, "Id", "Id", question.ChapterId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Uddipok,Image,ImagePosition,ChapterId,Id,CreatedAt,UpdatedAt,CreatedBy,UpdatedBy")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
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
            ViewData["ChapterId"] = new SelectList(_context.Chapters, "Id", "Id", question.ChapterId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Chapter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Questions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Questions'  is null.");
            }
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
          return (_context.Questions?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<JsonResult> SelectSubjectByClassId(int classId)
        {
            var subjects = await _context.AcademicSubjects.Where(m => m.AcademicClassId == classId).ToListAsync();
            return Json(subjects);
        }
        public async Task<JsonResult> SelectChapterBySubjectId(int subjectId)
        {
            var chapters = await _context.Chapters.Where(m => m.AcademicSubjectId == subjectId).ToListAsync();
            return Json(chapters);
        }

        [HttpGet]
        public IActionResult MakeQuestion()
        {

            ViewData["AcademicClassId"] = new SelectList(_context.AcademicClasses, "Id", "ClassName");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> MakeQuestion(MakeQuestionVM model)
        {
            if (ModelState.IsValid)
            {
                List<Question> questionList = new List<Question>();
                if (model.ChapterId > 0)
                {
                    questionList = await _context
                        .Questions
                        .Where(c => c.ChapterId == model.ChapterId)
                        .Include(s => s.Chapter.AcademicSubject)
                            .ThenInclude(c => c.AcademicClass).ToListAsync();
                }
                else
                {
                    questionList = await _context
                        .Questions
                        .Where(c => c.Chapter.AcademicSubject.AcademicClassId == model.AcademicClassId && c.Chapter.AcademicSubjectId == model.AcademicSubjectId)
                        .Include(m => m.QuestionDetails)
                        .ToListAsync();
                }
                int totalQuestion = 0;
                switch (model.Marks)
                {
                    case 10:
                        totalQuestion = 1;
                        break;
                    case 20:
                        totalQuestion = 2;
                        break;
                    case 30:
                        totalQuestion = 3;
                        break;
                    case 40:
                        totalQuestion = 4;
                        break;
                    case 50:
                        totalQuestion = 5;
                        break;
                    case 60:
                        totalQuestion = 6;
                        break;
                    case 70:
                        totalQuestion = 7;
                        break;
                    case 80:
                        totalQuestion = 8;
                        break;
                    case 90:
                        totalQuestion = 9;
                        break;
                    case 100:
                        totalQuestion = 10;
                        break;

                    default:
                        break;
                }
                questionList.Take(totalQuestion);
                return View("MadeQuestion");
            }

            ViewData["AcademicClassId"] = new SelectList(_context.AcademicClasses, "Id", "ClassName",model.AcademicClassId);

            return View(model);
        }

        public IActionResult MadeQuestion(MadeQuestionVM model)
        {
            return View();
        }
    }
}
