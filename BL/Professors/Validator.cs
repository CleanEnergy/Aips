using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Professors
{
    public class Validator
    {
        private ApplicationDbContext context;

        public Validator()
        {
            context = new ApplicationDbContext();
        }
        public Validator(ApplicationDbContext _context)
        {
            context = _context;
        }


        internal ValidationResult EnterExamGrade(string studentId, int examId, int grade)
        {
            ValidationResult result = new ValidationResult();

            if (grade < 1 || grade > 10)
            {
                result.AddError("Grade", "The grade is invalid.");
            }

            return result;
        }


        public ValidationResult CanEnterExamGrade(int examId, string studentId)
        {
            ValidationResult result = new ValidationResult();

            if (!context.ExamCopies.Where(x => x.ExamId == examId).Any(x => x.StudentId == studentId))
            {
                result.AddError("", "Cannot grade this exam, because it's copy has not been uploaded yet.");
            }

            return result;
        }
    }
}
