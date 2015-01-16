using DAL;
using EntityClasses;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.IO;

namespace BL
{
    public class Queries
    {
        private ApplicationDbContext context;

        public Queries()
        {
            context = new ApplicationDbContext();
        }

        /*~Queries()
        {
            if (context.Database.Connection.State == System.Data.ConnectionState.Open)
            {
                context.Database.Connection.Close();                
            }
        }*/

        public List<User> SearchUsers(string firstName = "", string lastName = "", string userName = "", string email = "")
        {
            if (firstName == "" && lastName == "" && userName == "" && email == "")
                return context.Users.ToList();

            return context.Users.Where(x =>  x.FirstName == firstName || x.LastName == lastName || x.UserName == userName || x.Email == email ).ToList();
        }

        public User GetUserById(string id)
        {
            return context.Users.Where(x => x.Id == id).SingleOrDefault();
        }

        public User GetUserByUsername(string username)
        {
            return context.Users.Where(x => x.UserName == username).SingleOrDefault();
        }

        public string GetUserIdByUsername(string username)
        {
            return context.Users.Single(x => x.UserName == username).Id;
        }

        public List<AdminMessage> GetAdminMessages()
        {
            return context.AdminMessages.ToList();
        }

        public List<UniversityEvent> GetUniversityEvents()
        {
            return context.UniversityEvents.ToList();
        }

        public Task<List<Subject>> GetAllSubjects()
        {
            return Task.FromResult<List<Subject>>(context.Subjects.ToList());
        }

        public Task<List<Exam>> GetAllScheduledExams()
        {
            return Task.FromResult<List<Exam>>(context.Exams.ToList());
        }

        public Task<Exam> GetExam(int examId)
        {
            return Task.FromResult<Exam>(context.Exams.Single(x => x.ExamId == examId));
        }

        public async Task<List<User>> GetAllStudents()
        {
            List<User> students = new List<User>();

            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<User>(context));

            foreach (User user in manager.Users.ToList())
            {
                if (await manager.IsInRoleAsync(user.Id, "Student"))
                {
                    students.Add(user);
                }
            }

            return students;
        }

        public Task<List<Exam>> GetAllExams()
        {
            return Task.FromResult<List<Exam>>(context.Exams.ToList());
        }

        public Task<List<ExamCopy>> GetUploadedExamCopies()
        {
            return Task.FromResult<List<ExamCopy>>(context.ExamCopies.ToList());
        }

        public Task<List<ExamCopy>> GetUploadedExamsPerExam(int examId)
        {
            List<ExamCopy> list = context.ExamCopies.Where(x => x.ExamId == examId).ToList();
            return Task.FromResult<List<ExamCopy>>(list);
        }

        public Task<ExamCopy> GetExamCopy(int uploadId)
        {
            return Task.FromResult<ExamCopy>(context.ExamCopies.Where(x => x.ExamCopyId == uploadId).Single());
        }

        public Task<List<Faculty>> GetAllFaculties()
        {
            return Task.FromResult<List<Faculty>>(context.Faculties.ToList());
        }

        public Faculty GetFaculty(int facultyId)
        {
            return context.Faculties.First(x => x.FacultyId == facultyId);
        }

        public List<SchoolYear> GetAvailableSchoolYears()
        {
            return context.SchoolYears.Where(x => x.YearStart.Year > DateTime.Now.Year - 1).ToList();
        }

        public List<Course> GetAllCourses()
        {
            return context.Courses.ToList();
        }


        public List<Course> GetCoursesForFaculty(int facultyId)
        {
            return context.Courses.Where(x => x.Faculty.FacultyId == facultyId).ToList();
        }

        public List<Subject> GetSubjectsForCourse(int courseId, int year, int degree)
        {
            return context.Subjects
                .Where(x => x.Course.CourseId == courseId &&
                            x.Year == year &&
                            x.Degree == degree)
                        .ToList();
        }

        public int GetSurveyId(string surveyTitle)
        {
            return context.Surveys.Single(x => x.Title == surveyTitle).SurveyId;
        }

        public List<SurveyQuestionType> GetAllSurveyQuestionTypes()
        {
            return context.SurveyQuestionTypes.ToList();
        }

        public int GetSurveyQuestionId(int surveyId, string questionText)
        {
            return context.SurveyQuestions.Single(x => x.SurveyId == surveyId && questionText.ToLower() == x.Question.ToLower()).SurveyQuestionId;
        }

        public Survey GetSurvey(int id)
        {
            return context.Surveys.Single(x => x.SurveyId == id);
        }

        public List<SurveyQuestion> GetSurveyQuestions(int id)
        {
            return context.SurveyQuestions.Where(x => x.SurveyId == id).ToList();
        }

        public List<SurveyAnswer> GetSurveyAnswers(int surveyId, List<SurveyQuestion> questions)
        {
            List<SurveyAnswer> answers = new List<SurveyAnswer>();

            foreach (SurveyQuestion question in questions)
            {
                answers.AddRange(context.SurveyAnswers.Where(x => x.SurveyQuestionId == question.SurveyQuestionId));
            }

            return answers;
        }

        public List<Survey> GetAllSurveys()
        {
            return context.Surveys.ToList();
        }

        public async Task<List<User>> GetAllProfessors()
        {
            List<User> result = new List<User>();
            ApplicationUserManager man = new ApplicationUserManager(new UserStore<User>(context));

            foreach (User user in context.Users.ToList())
            {
                if ((await man.GetRolesAsync(user.Id)).Contains("Professor"))
                {
                    result.Add(user);
                }
            }

            return result;
        }

        public List<Lecture> GetLecturesForSubject(int subjectId)
        {
            return context.Lectures.Where(x => x.SubjectId == subjectId).ToList();
        }

        public Lecture GetLecture(int lectureId)
        {
            return context.Lectures.Single(x => x.LectureId == lectureId);
        }

        public Subject GetSubject(int subjectId)
        {
            return context.Subjects.Single(x => x.SubjectId == subjectId);
        }


        public User GetProfessorForSubject(int subjectId)
        {
            return context.Users.Single(x => 
                x.Id == context.ProfessorsSubjects.FirstOrDefault(
                    y => y.SubjectId == subjectId).ProfessorId
                );
        }

        public List<Lecture> GetLecturesForProfessor(string professorId)
        {
            List<int> professorsSubjectsIds = context.ProfessorsSubjects.Where(x => x.ProfessorId == professorId).Select(x => x.SubjectId).ToList();

            return context.Lectures.AsEnumerable().Where((x) => { return professorsSubjectsIds.Any(y => y == x.SubjectId); }).ToList();
        }

        public async Task<List<Subject>> GetUnscheduledSubjects()
        {
            List<Subject> allSubjects = await GetAllSubjects();
            List<Lecture> lectures = context.Lectures.ToList();

            List<Subject> unscheduledSubjects = new List<Subject>();

            for (int i = 0; i < allSubjects.Count; i++)
            {
                if (lectures.SingleOrDefault(x => x.SubjectId == allSubjects[i].SubjectId) == null)
                {
                    unscheduledSubjects.Add(allSubjects[i]);
                }
            }

            return unscheduledSubjects;
        }

        public Task<List<ProfessorsSubjects>> GetAssignedSubjects()
        {
            return Task.FromResult<List<ProfessorsSubjects>>(context.ProfessorsSubjects.ToList());
        }

        public Task<List<Subject>> GetAssignedSubjects(List<Subject> subjects)
        {
            List<Subject> assignedSubjects = new List<Subject>();

            foreach (Subject subject in subjects)
            {
                if (context.ProfessorsSubjects.SingleOrDefault(x => x.SubjectId == subject.SubjectId) != null)
                {
                    assignedSubjects.Add(subject);
                }
            }

            return Task.FromResult<List<Subject>>(assignedSubjects);
        }

        public async Task<List<Subject>> GetUnassignedSubjects()
        {
            List<Subject> allSubjects = await GetAllSubjects();
            var assignedSubjectIds = context.ProfessorsSubjects.Select(x => new { Id = x.SubjectId }).ToList();

            List<Subject> unassignedSubjects = new List<Subject>();
            for (int i = 0; i < allSubjects.Count; i++)
            {
                if (!assignedSubjectIds.Contains(new { Id = allSubjects[i].SubjectId }))
                {
                    unassignedSubjects.Add(allSubjects[i]);
                }
            }

            return unassignedSubjects;
        }

        public List<SchoolDay> GetSchoolDays()
        {
            return context.SchoolDays.ToList();
        }

        internal List<Lab> GetLabsForCourseByYearAndDegree(int courseId, int year, int degree)
        {
            return context.Labs.Where(x => x.Subject.Course.CourseId == courseId && x.Subject.Year == year && x.Subject.Degree == degree).ToList();
        }

        internal List<Classroom> GetAllClassrooms()
        {
            return context.Classrooms.ToList();
        }

        internal List<ScheduledClassroom> GetAllScheduledClassrooms()
        {
            return context.ScheduledClassrooms.ToList();
        }

        public Task<IEnumerable<ClassroomType>> GetClassroomTypes()
        {
            return Task.FromResult<IEnumerable<ClassroomType>>(context.ClassroomTypes.ToList());
        }

        public IEnumerable<Lecture> GetLecturesForCourseByYearAndDegree(int courseId, int year, int degree)
        {
            List<Subject> subjects = context.Subjects.Where(x => x.Course.CourseId == courseId && x.Year == year && x.Degree == degree).ToList();

            List<Lecture> lectures = new List<Lecture>();

            subjects.ForEach((subject) => 
            {
                Lecture lecture = context.Lectures.SingleOrDefault(x => x.SubjectId == subject.SubjectId);
                if (lecture != null)
                {
                    lectures.Add(lecture);
                }
            });

            return lectures;
        }

        public Task<IEnumerable<Subject>> GetSubjectsWithoutLabs()
        {
            throw new NotImplementedException();

            /*List<Subject> allSubjects = context.Subjects.ToList();
            List<ProfessorsSubjects> assignedSubjects = context.ProfessorsSubjects.ToList();

            List<Subject> mergedSubjects = new List<Subject>();

            foreach (ProfessorsSubjects assignedSubject in assignedSubjects)
            {
                mergedSubjects.Add(allSubjects.Single(x => x.SubjectId == assignedSubject.SubjectId));
            }

            List<ScheduledClassroom> scheduledLabs = context.ScheduledClassrooms.Where(x => x.Classroom.ClassroomType.Name == "Lab").ToList();

            List<Subject> result = new List<Subject>();
            foreach (Subject subject in mergedSubjects)
            {
                if (!scheduledLabs.Any(x => x.SubjectId == subject.SubjectId))
                {
                    result.Add(subject);
                }
            }
            return Task.FromResult<IEnumerable<Subject>>(result);*/
        }

        public async Task<List<User>> GetAllTeachers()
        {
            List<User> result = new List<User>();
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<User>(context));

            foreach (User user in context.Users.ToList())
            {
                var roles = await manager.GetRolesAsync(user.Id);
                if (roles.Contains("Professor") || roles.Contains("Assistant")) 
                {
                    result.Add(user);
                }
            }

            return result;
        }

        public async Task<List<User>> GetAllAssistants()
        {
            List<User> result = new List<User>();
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<User>(context));

            foreach (User user in context.Users.ToList())
            {
                var roles = await manager.GetRolesAsync(user.Id);
                if (roles.Contains("Assistant"))
                {
                    result.Add(user);
                }
            }

            return result;
        }

        public List<IdentityRole> GetAllRoles()
        {
            return context.Roles.ToList();
        }

        public async Task<List<Subject>> GetUnassignedSubjectsForAssistants()
        {
            List<Subject> allSubjects = await GetAllSubjects();
            var assignedSubjectIds = context.AssistantsSubjects.Select(x => new { Id = x.SubjectId }).ToList();

            List<Subject> unassignedSubjects = new List<Subject>();
            for (int i = 0; i < allSubjects.Count; i++)
            {
                if (!assignedSubjectIds.Contains(new { Id = allSubjects[i].SubjectId }))
                {
                    unassignedSubjects.Add(allSubjects[i]);
                }
            }

            return unassignedSubjects;
        }

        public Task<List<Classroom>> GetAllClassroomsAsync()
        {
            return Task.FromResult<List<Classroom>>(context.Classrooms.ToList());
        }

        public Task<List<Exam>> GetExamsAvailableForStudent(string userName)
        {
            string studentId = GetUserIdByUsername(userName);
            List<SignOnInformation> studentsSignOnInformations = context.SignOnInformation.Where(x => x.Student.Id == studentId).ToList();
            List<SignOnExam> studentsExistingExamSignOns = context.ExamSignOns.Where(x => x.StudentId == studentId).ToList(); 
            SignOnInformation latestSignOn = studentsSignOnInformations.Max();

            List<Exam> examsInYearAndDegree = context.Exams.Where(x => x.Subject.Year == latestSignOn.Year && x.Subject.Degree == latestSignOn.Degree).ToList();
            List<Exam> examsForCourse = examsInYearAndDegree.Where(x => x.Subject.Course.CourseId == latestSignOn.Course.CourseId).ToList();
            
            for (int i = 0; i < examsForCourse.Count; i++)
            {
                if (studentsExistingExamSignOns.Any(x => x.ExamId == examsForCourse[i].ExamId))
                {
                    examsForCourse.RemoveAt(i);
                }
            }

            return Task.FromResult<List<Exam>>(examsForCourse);
        }

        public Task<List<SignOnExam>> GetOpenExamSignOns(string userName)
        {
            DateTime examSignOffLimit = DateTime.Now.Subtract(TimeSpan.FromDays(3));
            string userId = GetUserIdByUsername(userName);
            return Task.FromResult<List<SignOnExam>>(
                context.ExamSignOns
                    .Where(x => x.StudentId == userId && !x.IsSignedOff && x.Exam.DateAndTime > examSignOffLimit)
                    .ToList()
                    .OrderBy(x => x.Exam.DateAndTime)
                    .ToList()
                );
        }

        public Task<List<SignOnExam>> GetAllSignOnsForStudent(string studentId)
        {
            return Task.FromResult<List<SignOnExam>>(context.ExamSignOns
                .Where(x => x.StudentId == studentId)
                .ToList()
            );
        }

        public Task<List<ExamCopy>> GetAllExamCopiesForStudent(string studentId)
        {
            return Task.FromResult<List<ExamCopy>>(context.ExamCopies
                .Where(x => x.StudentId == studentId)
                .ToList()
            );
        }

        public Task<ExamCopy> GetExamCopy(int examId, string studentId)
        {
            return Task.FromResult<ExamCopy>(
                context.ExamCopies.Single(x => x.ExamId == examId && x.StudentId == studentId)
            );
        }

        public Task<List<SignOnExam>> GetStudentsSignedOnExam(int examId)
        {
            List<SignOnExam> signOns = context.ExamSignOns
                    .Where(x => x.ExamId == examId)
                    .ToList();

            return Task.FromResult<List<SignOnExam>>(signOns);
        }

        public Task<List<ExamStudent>> GetAlreadyGradedExams(int examId)
        {
            return Task.FromResult<List<ExamStudent>>(
                context.ExamStudent
                .Where(x => x.ExamId == examId)
                .ToList()
            );
        }

        public string GetUsernameById(string userId)
        {
            return context.Users.Single(x => x.Id == userId).UserName;
        }

        public Task<ExamStudent> GetExamForStudent(int examId, string studentId)
        {
            return Task.FromResult<ExamStudent>(
                context.ExamStudent.Single(x => x.StudentId == studentId && x.ExamId == examId)
            );
        }

        public Task<List<ExamStudent>> GetAllStudentsExams(string studentId)
        {
            return Task.FromResult<List<ExamStudent>>(context.ExamStudent.Where(x => x.StudentId == studentId)
                .ToList()
            );
        }

        public Task<List<SignOnInformation>> GetAllSignOnInformationsForStudent(string studentId)
        {
            return Task.FromResult<List<SignOnInformation>>(
                context.SignOnInformation.Where(x => x.Student.Id == studentId).ToList().OrderBy(x => x.SchoolYear.YearStart).ToList()    
            );
        }

        public async Task<List<Subject>> GetAssignedSubjectsForCourse(int courseId, int year, int degree)
        {
            List<ProfessorsSubjects> assignedSubjects = context.ProfessorsSubjects.ToList();
            List<Subject> intersection = new List<Subject>();

            foreach (Subject subject in await GetAllSubjects())
            {
                if (assignedSubjects.FirstOrDefault(x => x.SubjectId == subject.SubjectId) != null)
                {
                    intersection.Add(subject);
                }    
            }

            return intersection;
        }

        public List<Exam> GetExamsForSubject(int subjectId)
        {
            return context.Exams.Where(x => x.SubjectId == subjectId).ToList();
        }

        public Task<List<ExamCopy>> GetExamCopies(int examId)
        {
            return Task.FromResult<List<ExamCopy>>(context.ExamCopies.Where(x => x.ExamId == examId).ToList());
        }

        public Course GetCourse(int courseId)
        {
            return context.Courses.Single(x => x.CourseId == courseId);
        }

        /*public Task<List<Subject>> GetAllSubjectsForStudent(string studentId)
        {
            List<SignOnInformation> signOnInfo = context.SignOnInformation.Where(x => x.StudentId == studentId).ToList();

            List<Subject> subjects = new List<Subject>();

            foreach (SignOnInformation info in signOnInfo)
            {
                subjects.AddRange(GetSubjectsForCourse(info.CourseId, info.Year, info.Degree));
            }

            return Task.FromResult<List<Subject>>(subjects);
        }*/


        public float? GetGradeForStudent(int subjectId, string studentId)
        {
            List<ExamStudent> allExams = context.ExamStudent.Where(x => x.Exam.SubjectId == subjectId && x.StudentId == studentId).ToList();

            int gradeSum = allExams.Sum(x => x.Grade);

            if (gradeSum == 0)
            {
                return null;
            }

            float average = gradeSum / allExams.Count;
            return average;
        }

        public Task<List<SignOnExam>> GetStudentsSignedOffExam(int examId)
        {
            List<SignOnExam> signedOff = context.ExamSignOns.Where(x => x.ExamId == examId && x.IsSignedOff).ToList();

            return Task.FromResult<List<SignOnExam>>(signedOff);
        }

        public Task<List<Lab>> GetAllLabs()
        {
            return Task.FromResult<List<Lab>>(context.Labs.ToList());
        }

        public Task<List<Lab>> GetAllLabsForAssistant(string assistantId)
        {
            return Task.FromResult<List<Lab>>(context.Labs.Where(x => x.AssistantId == assistantId).ToList());
        }

        public async Task<List<User>> GetStudentsForLab(int labId)
        {
            Lab lab = await GetLab(labId);

            List<SignOnInformation> signOnInfos = context.SignOnInformation
                .Where(x => x.CourseId == lab.Subject.Course.CourseId && 
                            x.Degree == lab.Subject.Degree && 
                            x.Year == lab.Subject.Year &&
                            x.SchoolYear.YearStart.Year == DateTime.Now.Year
                        )
                .ToList();

            List<User> students = new List<User>();
            foreach (SignOnInformation information in signOnInfos)
            {
                students.Add(information.Student);
            }

            return students;
        }

        public Task<Lab> GetLab(int labId)
        {
            return Task.FromResult<Lab>(context.Labs.Single(x => x.LabId == labId));
        }

        public Task<List<StudentsLabs>> GetLabPresenceForStudent(string studentId)
        {
            return Task.FromResult<List<StudentsLabs>>(
                context.StudentsLabs
                    .Where(x => x.StudentId == studentId)
                    .ToList()
            );
        }

        public async Task<List<ScheduledLab>> GetScheduledLabsForStudentsSubjects(string studentId)
        {
            List<ScheduledLab> scheduledLabs = new List<ScheduledLab>();

            List<Subject> subjects = await GetAllSubjectsForStudent(studentId);

            foreach (Subject subject in subjects)
            {
                scheduledLabs.AddRange(context.ScheduledLabs.Where(x => x.Lab.SubjectId == subject.SubjectId).ToList());
            }

            return scheduledLabs;
        }

        public Task<List<Subject>> GetAllSubjectsForStudent(string studentId)
        {
            List<SignOnInformation> signOnInformation = context.SignOnInformation.Where(x => x.StudentId == studentId).ToList();
            DateTime latestYear = signOnInformation.Max(x => x.SchoolYear.YearStart);

            SignOnInformation activeSignOnInfo = signOnInformation.Single(x => x.SchoolYear.YearStart == latestYear);

            return Task.FromResult<List<Subject>>(
                context.Subjects
                        .Where(x => x.Course.CourseId == activeSignOnInfo.CourseId &&
                                    x.Degree == activeSignOnInfo.Degree &&
                                    x.Year == activeSignOnInfo.Year)
                        .ToList());
        }
    }

}
