using DAL;
using EntityClasses;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.StudentsOffice
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

        /*public bool IsScheduleFree(DateTime startTime, DateTime endTime, int day, int subjectId)
        {
            Queries query = new Queries();
            User professor = query.GetProfessorForSubject(subjectId);

            return !query.GetLecturesForProfessor(professor.Id).Any((x) =>
            {
                TimeSpan lectureDuration = x.EndTime - x.StartTime;

                return (startTime + lectureDuration > endTime) && x.Day == day;

            });
        }*/

        public Task<bool> CanCreateSubject()
        {
            return Task.FromResult<bool>(context.Courses.ToList().Count != 0);
        }

        public bool CanAddCourse()
        {
            return context.Faculties.ToList().Count != 0;
        }

        internal ValidationResult AddFaculty(string facultyName)
        {
            ValidationResult result = new ValidationResult();
            if (context.Faculties.Any(x => x.Name == facultyName))
            {
                result.AddError("Name", "A faculty with this name already exists!");
            }
            return result;
        }

        internal ValidationResult AddCourse(Course course, int facultyId)
        {
            ValidationResult result = new ValidationResult();

            if (context.Courses.Any(x => x.Name == course.Name))
            {
                result.AddError("Name", "A course with this name already exists!");
            }

            return result;
        }

        internal ValidationResult CreateSubject(Subject subject)
        {
            ValidationResult result = new ValidationResult();

            if (context.Subjects.Any(x => x.Title == subject.Title))
            {
                result.AddError("Title", "A subject with this title already exists!");
            }
            if (subject.Year < 1 || subject.Year > 4)
            {
                result.AddError("Year", "An invalid year was entered!");
            }
            if (subject.Degree < 1 || subject.Degree > 2)
            {
                result.AddError("Degree", "An invalid degree was entered!");
            }

            return result;
        }

        internal ValidationResult AddSchoolYear(DateTime yearStart, DateTime yearEnd)
        {
            ValidationResult result = new ValidationResult();

            if (context.SchoolYears.Any(x => x.YearStart == yearStart && x.YearEnd== yearEnd))
            {
                result.AddError("", "This school year already exists!");
            }

            return result;
        }

        public bool CanAddStudent()
        {
            if (context.SchoolYears.ToList().Count == 0 || context.Courses.ToList().Count == 0)
            {
                return false;
            }
            return true;
        }

        internal ValidationResult AddStudent(User student, int courseId, int schoolYearId)
        {
            ValidationResult result = new ValidationResult();

            if (context.Users.Any(x => x.UserName == student.UserName))
            {
                result.AddError("UserName", "A user with this username already exists.");
            }
            if (context.Users.Any(x => x.Email == student.Email))
            {
                result.AddError("Email", "A user with this Email already exists.");
            }
            if (!context.Courses.Any(x => x.CourseId == courseId))
            {
                result.AddError("CourseId", "This course does not exist.");
            }
            if (!context.SchoolYears.Any(x => x.SchoolYearId == schoolYearId))
            {
                result.AddError("SchoolYearId", "This school year does not exist.");
            }

            return result;
        }

        internal ValidationResult AssignSubjectToProfessor(int subjectId, string professorId)
        {
            ValidationResult result = new ValidationResult();

            if (context.Subjects.SingleOrDefault(x => x.SubjectId == subjectId) == null)
            {
                result.AddError("SubjectId", "This subject does not exist.");
            }
            if (context.Users.SingleOrDefault(x => x.Id == professorId) == null)
            {
                result.AddError("ProfessorId", "This professor does not exist.");
            }

            if (result.Succeded)
            {
                if (context.ProfessorsSubjects.SingleOrDefault(x => x.ProfessorId == professorId && x.SubjectId == subjectId) != null)
                {
                    result.AddError("", "This subject is already assigned to this professor.");
                }
                else if (context.ProfessorsSubjects.Any(x => x.SubjectId == subjectId))
                {
                    result.AddError("", "This subject is already assigned");
                }
            }

            return result;
        }

        public ValidationResult CanScheduleExam()
        {
            ValidationResult result = new ValidationResult();
            if (context.Subjects.ToList().Count == 0)
            {
                result.AddError("", "Cannot schedule an exam, because no subjects exist.");
            }
            if (context.Classrooms.ToList().Count == 0)
            {
                result.AddError("", "Cannot schedule an exam, because no classrooms exist.");                
            }
            return result;
        }

        internal ValidationResult ScheduleExam(DateTime dateAndTime, int subjectId)
        {
            ValidationResult result = new ValidationResult();

            if (dateAndTime < DateTime.Now)
            {
                result.AddError("", "The date must be set in the future");
                return result;
            }

            List<Exam> exams = context.Exams.Where(x => x.Subject.SubjectId == subjectId).ToList();

            if (exams.Count == 0)
            {
                return result;
            }

            if ((exams.Where(x => x.SubjectId == subjectId).Max(x => x.DateAndTime).AddDays(14) > dateAndTime))
            {
                result.AddError("", "Exams must be at least two weeks apart.");
            }

            return result;
        }

        public ValidationResult CanCreateNewSchedules()
        {
            ValidationResult result = new ValidationResult();

            if (context.Subjects.ToList().Count == 0)
            {
                result.AddError("", "No schedules can be created because no subjects exits.");
            }
            else if (context.ProfessorsSubjects.ToList().Count == 0)
            {
                result.AddError("", "No schedules can be created because no subjects are assigned.");
            }

            return result;
        }

        internal ValidationResult AssignSubjectToAssistant(int subjectId, string assistantId)
        {
            ValidationResult result = new ValidationResult();

            if (context.Subjects.SingleOrDefault(x => x.SubjectId == subjectId) == null)
            {
                result.AddError("SubjectId", "This subject does not exist.");
            }
            if (context.Users.SingleOrDefault(x => x.Id == assistantId) == null)
            {
                result.AddError("AssistantId", "This assistant does not exist.");
            }

            if (result.Succeded)
            {
                if (context.AssistantsSubjects.SingleOrDefault(x => x.AssistantId == assistantId && x.SubjectId == subjectId) != null)
                {
                    result.AddError("", "This subject is already assigned to this assistant.");
                }
                else if (context.AssistantsSubjects.Any(x => x.SubjectId == subjectId))
                {
                    result.AddError("", "This subject is already assigned");
                }
            }

            return result;
        }

        internal ValidationResult NewSchedule(ScheduleIntervalViewModel scheduleIntervalViewModel, int classroomId)
        {
            ValidationResult result = new ValidationResult();
            if (!scheduleIntervalViewModel.Classrooms.Any(x => x.ClassroomId == classroomId))
            {
                result.AddError("ClassroomId", "This classroom is not free.");
            }
            return result;
        }

        public ValidationResult CanAssignSubject()
        {
            ValidationResult result = new ValidationResult();

            if (context.Subjects.ToList().Count == 0)
            {
                result.AddError("", "Cannot assign subject, because no subjects exist.");
            }

            if (context.Subjects.ToList().Count == context.ProfessorsSubjects.ToList().Count)
            {
                result.AddError("", "Cannot assign subject, because all subjects are already assigned.");
            }

            return result;
        }

    }
}
