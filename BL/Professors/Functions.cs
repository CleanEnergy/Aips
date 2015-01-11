using DAL;
using EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Professors
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

        public static List<Subject> GetSubjects(Professor professor)
        {
            throw new NotImplementedException();
        }

        public static void GradeExam(Exam exam, Student student, int grade)
        {
            throw new NotImplementedException();
        }

        public static void ChangeExamGrade(Exam exam, Student student, int grade)
        {
            throw new NotImplementedException();
        }

        public static void GradeThesis(Thesis thesis, int grade)
        {
            throw new NotImplementedException();
        }

        public static void SendGradedExamMessage(Student student, Exam exam, string message)
        {
            throw new NotImplementedException();
        }

        public static void SendExamGradeChangedMessage(Student student, Exam exam, string message)
        {
            throw new NotImplementedException();
        }

        public static void CalculateAverageGradeForSubject(Subject subject)
        {
            throw new NotImplementedException();
        }

        public static List<Student> GetStudentsSignedOnSubject(Subject subject)
        {
            throw new NotImplementedException();
        }

        public static void SetMinimalLabPresence(Lab lab, int presencePercent)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetUsersSignedOnExam(List<SignOnExam> informations)
        {
            Queries query = new Queries();
            List<User> result = new List<User>();
            foreach (SignOnExam information in informations)
            {
                result.Add(query.GetUserById(information.StudentId));
            }
            return Task.FromResult<List<User>>(result);
        }


        public async Task<ValidationResult> EnterExamGrade(string studentId, int examId, int grade)
        {
            ValidationResult validation = validator.EnterExamGrade(studentId, examId, grade);

            if (validation.Succeded)
            {
                ExamStudent entity = context.ExamStudent.FirstOrDefault(x => x.StudentId == studentId && x.ExamId == examId);

                if (entity == null)
                {
                    context.ExamStudent.Add(new ExamStudent()
                    {
                        StudentId = studentId,
                        ExamId = examId,
                        Grade = grade
                    });
                }
                else
                {
                    entity.Grade = grade;
                }
                await context.SaveChangesAsync();
            }

            return validation;
        }
    }
}
