using DAL;
using EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Students
{
    public class Functions
    {
        private ApplicationDbContext context;
        private Validator validator;

        public Functions()
        {
            context = new ApplicationDbContext();
            validator = new Validator(context);
        }

        public List<UniversityEvent> CheckPublishedEvents()
        {
            throw new NotImplementedException();
        }

        public byte[] CheckExamCopy(Exam exam)
        {
            throw new NotImplementedException();
        }

        public List<SignOnExam> CheckAllExamSignOns(Student student)
        {
            throw new NotImplementedException();
        }

        public void FinishSurvey(FinishedSurvey finishedSurvey, Student student)
        {
            throw new NotImplementedException();
        }

        public List<SignOnInformation> CheckSignOnInformation(Student student)
        {
            throw new NotImplementedException();
        }

        public List<ContactHours> CheckContactHours(Professor professor)
        {
            throw new NotImplementedException();
        }

        public void SubmitThesis(Thesis thesis, Student student)
        {
            throw new NotImplementedException();
        }

        public async Task<ValidationResult> SignOnExam(int examId, string userName)
        {
            string studentId = context.Users.Single(x => x.UserName == userName).Id;

            ValidationResult validation = validator.SignOnExam(examId, userName);
            if (validation.Succeded)
            {
                context.ExamSignOns.Add(new SignOnExam() 
                {
                    ExamId = examId,
                    StudentId = studentId
                });
                await context.SaveChangesAsync();
            }
            return validation;
        }

        public List<Subject> CheckFinishedSubjects(Student student)
        {
            throw new NotImplementedException();
        }

        public List<Subject> CheckActiveSubjects(Student student)
        {
            throw new NotImplementedException();
        }

        public List<SignOnExam> CheckOpenExamSignOns(Student student)
        {
            throw new NotImplementedException();
        }

        public async Task<ValidationResult> SignOffExam(int examId, string userName)
        {
            Queries query = new Queries();
            string studentId = query.GetUserIdByUsername(userName);
            ValidationResult validation = validator.SignOffExam(examId, studentId);

            if (validation.Succeded)
            {
                foreach (SignOnExam signOn in context.ExamSignOns.Where(x => x.StudentId == studentId).ToList())
                {
                    if (signOn.ExamId == examId)
                    {
                        signOn.IsSignedOff = true;
                        break;
                    }
                }
                await context.SaveChangesAsync();
            }

            return validation;
        }
    }
}
