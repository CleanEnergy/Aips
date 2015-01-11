using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Students
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

        internal ValidationResult SignOnExam(int examId, string userName)
        {
            ValidationResult result = new ValidationResult();

            if (!context.Exams.Any(x => x.ExamId == examId))
            {
                result.AddError("", "This exam does not exist.");
            }

            return result;
        }

        public ValidationResult CanSignOffExam(int examId, string studentId)
        {
            ValidationResult result = new ValidationResult();

            if ((context.Exams.Single(x => x.ExamId == examId).DateAndTime - DateTime.Now).Days < 3)
            {
                result.AddError("", "Cannot sign off from this exam anymore.");
            }
            if (!context.ExamSignOns.Any(x => x.ExamId == examId && x.StudentId == studentId))
            {
                result.AddError("", "Cannot sign off an exam you are not signed on to.");
            }

            return result;
        }

        internal ValidationResult SignOffExam(int examId, string studentId)
        {
            ValidationResult result = new ValidationResult();

            return result;
        }
    }
}
