using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using questionBank.Application.Models;

namespace questionBank.Application.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionDetail> QuestionDetails { get; set; }
        public DbSet<AcademicClass> AcademicClasses { get; set; }
        public DbSet<AcademicSubject> AcademicSubjects { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
    }
}