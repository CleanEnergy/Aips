using BL.Scheduling;
using DAL;
using EntityClasses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.StudentsOffice
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

        public Task PublishUniversityEvent(UniversityEvent universityEvent)
        {
            context.UniversityEvents.Add(universityEvent);
            Task.WaitAll(context.SaveChangesAsync());
            return Task.FromResult<object>(null);
        }

        public async Task InformAllStudentsOfUniversityEvent(UniversityEvent universityEvent, string subject, string message, string senderUsername)
        {
            Messaging.Functions messagingFunctions = new Messaging.Functions();
            ApplicationUserManager UserManager = new ApplicationUserManager(new UserStore<User>(context));

            List<User> users = UserManager.Users.ToList();
            List<string> recepientUsernames = new List<string>();

            foreach (User user in users)
            {
                if ((await UserManager.GetRolesAsync(user.Id)).Contains("Student"))
                {
                    recepientUsernames.Add(user.UserName);
                }
            }
            await messagingFunctions.SendMessage(subject, message + universityEvent.ToString(), senderUsername, recepientUsernames.ToArray());
        }

        public async Task<ValidationResult> CreateSubject(Subject subject, int courseId, string professorId)
        {
            ValidationResult result = validator.CreateSubject(subject);

            if (result.Succeded)
            {
                subject.Course = context.Courses.Single(x => x.CourseId == courseId);
                context.Subjects.Add(subject);

                context.ProfessorsSubjects.Add(new ProfessorsSubjects() 
                {
                    SubjectId = subject.SubjectId,
                    ProfessorId = professorId
                });

                await context.SaveChangesAsync();
            }

            return result;
        }

        public async Task<ValidationResult> ScheduleExam(int subjectId, DateTime dateAndTime, int classroomId)
        {
            ValidationResult validation = validator.ScheduleExam(dateAndTime, subjectId);

            if (validation.Succeded)
            {
                Subject subject = context.Subjects.Single(x => x.SubjectId == subjectId);
                context.Exams.Add(new Exam { Subject = subject, DateAndTime = dateAndTime, ClassroomId = classroomId });
                await context.SaveChangesAsync();
            }

            return validation;
        }

        public Task CancelExam(int examId)
        {
            context.Exams.Remove(context.Exams.Single(x => x.ExamId == examId));
            return context.SaveChangesAsync();
        }

        public Task ChangeExamDate(Exam exam, DateTime dateAndTime)
        {
            foreach (Exam storedExam in context.Exams)
            {
                if (storedExam.ExamId == exam.ExamId)
                {
                    storedExam.DateAndTime = dateAndTime;
                    storedExam.Subject = exam.Subject;
                }
            }
            
            return context.SaveChangesAsync();
        }

        public void PublishSurvey(Survey survey)
        {
            throw new NotImplementedException();
        } 

        public async Task<ValidationResult> AssignSubjectToProfessor(int subjectId, string professorId)
        {
            ValidationResult validation = validator.AssignSubjectToProfessor(subjectId, professorId);

            if (validation.Succeded)
            {
                context.ProfessorsSubjects.Add(new ProfessorsSubjects
                {
                    ProfessorId = professorId,
                    SubjectId = subjectId
                });
                await context.SaveChangesAsync();
            }

            return validation;
        }

        public async Task<ValidationResult> AssignSubjectToAssistant(int subjectId, string assistantId)
        {
            ValidationResult validation = validator.AssignSubjectToAssistant(subjectId, assistantId);

            if (validation.Succeded)
            {
                context.AssistantsSubjects.Add(new AssistantsSubjects
                {
                    AssistantId = assistantId,
                    SubjectId = subjectId
                });
                await context.SaveChangesAsync();
            }

            return validation;
        }

        public async Task<ValidationResult> AddStudent(User student, string password, int courseId, int schoolYearId)
        {
            ValidationResult validationResult = validator.AddStudent(student, courseId, schoolYearId);

            if (!validationResult.Succeded)
            {
                return validationResult;
            }

            IdentityResult result = null;
            using (DbContextTransaction t = context.Database.BeginTransaction())
            {
                // Create new login
                ApplicationUserManager UserManager = new ApplicationUserManager(new UserStore<User>(context));
                result = await UserManager.CreateAsync(student, password);
                
                if (result.Succeeded)
                {
                    User savedStudent = context.Users.Single(x => x.UserName == student.UserName);

                    // Add to student role
                    UserManager.AddToRole(savedStudent.Id, "Student");

                    // Get the selected school year
                    SchoolYear schoolYear = context.SchoolYears.Single(x => x.SchoolYearId == schoolYearId);

                    // Create sign on information
                    context.SignOnInformation.Add(new SignOnInformation
                    {
                        Student = savedStudent,
                        Course = context.Courses.Single(x => x.CourseId == courseId),
                        SchoolYear = schoolYear,
                        Year = 1,
                        Degree = 1
                    });

                    // Add the enrolled student to the school year
                    SchoolYear storedSchoolYear = context.SchoolYears.SingleOrDefault(x => x.SchoolYearId == schoolYear.SchoolYearId);
                    if (storedSchoolYear.EnrolledStudents == null)
                    {
                        storedSchoolYear.EnrolledStudents = new List<User>();
                    }
                    storedSchoolYear.EnrolledStudents.Add(savedStudent);

                    await context.SaveChangesAsync();
                    t.Commit();
                }
            }
            return validationResult;
        }

        public async Task InformStudentsOfScheduledExam(int subjectId, DateTime examDate, string senderUsername)
        {
            Messaging.Functions messagingFunctions = new Messaging.Functions();
            Subject subject = context.Subjects.Single(x => x.SubjectId == subjectId);
            ApplicationUserManager UserManager = new ApplicationUserManager(new UserStore<User>(context));

            List<User> users = UserManager.Users.ToList();
            List<string> recepientUsernames = new List<string>();

            foreach (User user in users)
            {
                if ((await UserManager.GetRolesAsync(user.Id)).Contains("Student"))
                {
                    recepientUsernames.Add(user.UserName);
                }
            }

            string messageSubject = "An exam has been scheduled for " + subject.Title;
            string content = "The exam for " + subject.Title + " will take place on " + examDate.ToShortDateString() + " at " + examDate.ToShortTimeString();

            await messagingFunctions.SendMessage(messageSubject, content, senderUsername, recepientUsernames.ToArray());
        }

        public async Task InformStudentsOfRescheduledExam(int subjectId, DateTime examDate, string senderUsername)
        {
            Messaging.Functions messagingFunctions = new Messaging.Functions();
            Subject subject = context.Subjects.Single(x => x.SubjectId == subjectId);
            ApplicationUserManager UserManager = new ApplicationUserManager(new UserStore<User>(context));

            List<User> users = UserManager.Users.ToList();
            List<string> recepientUsernames = new List<string>();

            foreach (User user in users)
            {
                if ((await UserManager.GetRolesAsync(user.Id)).Contains("Student"))
                {
                    recepientUsernames.Add(user.UserName);
                }
            }

            string messageSubject = "An exam has been RESCHEDULED for " + subject.Title;
            string content = "The exam for " + subject.Title + " will now take place on " + examDate.ToShortDateString() + " at " + examDate.ToShortTimeString();

            await messagingFunctions.SendMessage(messageSubject, content, senderUsername, recepientUsernames.ToArray());
        }

        public async Task InformStudentsOfCancelledExam(int subjectId, DateTime examDate, string senderUsername)
        {
            Messaging.Functions messagingFunctions = new Messaging.Functions();
            Subject subject = context.Subjects.Single(x => x.SubjectId == subjectId);
            ApplicationUserManager UserManager = new ApplicationUserManager(new UserStore<User>(context));

            List<User> users = UserManager.Users.ToList();
            List<string> recepientUsernames = new List<string>();

            foreach (User user in users)
            {
                if ((await UserManager.GetRolesAsync(user.Id)).Contains("Student"))
                {
                    recepientUsernames.Add(user.UserName);
                }
            }

            string messageSubject = "An exam has been CANCELED for " + subject.Title;
            string content = "The exam for " + subject.Title + " scheduled to take place on " + examDate.ToShortDateString() + " at " + examDate.ToShortTimeString() + " has been canceled.";

            await messagingFunctions.SendMessage(messageSubject, content, senderUsername, recepientUsernames.ToArray());
        }

        public Task StoreExamPDF(string authorId, int examId, Stream pdfStream, string mimeType)
        {
            byte[] fileBuffer;

            using (BinaryReader br = new BinaryReader(pdfStream))
            {
                fileBuffer = br.ReadBytes((int)pdfStream.Length);
            }

            context.ExamCopies.Add(new ExamCopy 
            {
                StudentId = authorId,
                ExamId = examId,
                ExamImage = fileBuffer,
                MimeType = mimeType
            });

            return context.SaveChangesAsync();
        }

        public Task<ValidationResult> AddFaculty(string name)
        {
            ValidationResult validation = validator.AddFaculty(name);

            if (validation.Succeded)
            {
                context.Faculties.Add(new Faculty { Name = name });
                context.SaveChanges();
            }

            return Task.FromResult<ValidationResult>(validation);
        }

        public User GetStudent(string username)
        {
            return context.Users.Single(x => x.UserName == username);
        }

        public async Task<ValidationResult> AddSchoolYear(DateTime yearStart, DateTime yearEnd)
        {
            ValidationResult result = validator.AddSchoolYear(yearStart, yearEnd);

            if (result.Succeded)
            {
                context.SchoolYears.Add(new SchoolYear
                {
                    YearStart = yearStart,
                    YearEnd = yearEnd
                });
                await context.SaveChangesAsync();
            }

            return result;
        }

        public IEnumerable<SchoolYear> GetAllSchoolYears()
        {
            return context.SchoolYears.ToList();
        }

        public async Task<ValidationResult> AddCourse(Course course, int facultyId)
        {
            ValidationResult validation = validator.AddCourse(course, facultyId);

            if (validation.Succeded)
            {
                course.Faculty = context.Faculties.Single(x => x.FacultyId == facultyId);
                context.Courses.Add(course);
                await context.SaveChangesAsync();
            }
            return validation;
        }

        public void AddSurvey(Survey survey)
        {
            context.Surveys.Add(survey);
            context.SaveChanges();
        }

        public void AddSurveyQuestion(SurveyQuestion question)
        {
            context.SurveyQuestions.Add(question);
            context.SaveChanges();
        }
        
        public void AddSurveyAnswers(List<SurveyAnswer> answers)
        {
            context.SurveyAnswers.AddRange(answers);
            context.SaveChanges();
        }

        public async Task<ValidationResult> ScheduleLectures(DateTime scheduleStart, DateTime scheduleEnd, int classroomId, int subjectId, int schoolDayId)
        {
            throw new NotImplementedException();
            /*ValidationResult validation = validator.ScheduleLecture(
                scheduleStart,
                scheduleEnd,
                classroomId,
                subjectId
            );

            if (validation.Succeded)
            {
                using (DbContextTransaction t = context.Database.BeginTransaction())
                {
                    context.ScheduledClassrooms.Add(new ScheduledClassroom 
                    {
                        ClassroomId = classroomId,
                        ScheduleStart = scheduleStart,
                        ScheduleEnd = scheduleEnd,
                        SchoolDayId = schoolDayId,
                        SubjectId = subjectId
                    });
                    await context.SaveChangesAsync();

                    context.Lectures.Add(new Lecture
                    {
                        SubjectId = subjectId,
                        ScheduledClassroomId = context.ScheduledClassrooms.Single(
                            x => x.SchoolDayId == schoolDayId && 
                                 x.ScheduleStart == scheduleStart && 
                                 x.ScheduleEnd == scheduleEnd
                            ).ScheduledClassroomId
                    });
                    await context.SaveChangesAsync();

                    t.Commit();
                }
            }

            return validation;*/
        }

        public async Task<ValidationResult> RescheduleLecture(int lectureId, DateTime scheduleStart, DateTime scheduleEnd, int classroomId, int subjectId, int schoolDayId)
        {
            throw new NotImplementedException();
            /*ValidationResult result = validator.RescheduleLecture(
                scheduleStart,
                scheduleEnd,
                classroomId,
                subjectId
            );

            if (result.Succeded)
            {
                foreach (ScheduledClassroom scheduledClassroom in context.ScheduledClassrooms.ToList())
                {
                    if (scheduledClassroom.SubjectId == subjectId)
                    {
                        scheduledClassroom.ClassroomId = classroomId;
                        scheduledClassroom.ScheduleStart = scheduleStart;
                        scheduledClassroom.ScheduleEnd = scheduleEnd;
                        scheduledClassroom.SchoolDayId = schoolDayId;
                        scheduledClassroom.SubjectId = subjectId;
                    }
                }
                await context.SaveChangesAsync();
            }

            return result;*/
        }

        /*public async Task<ValidationResult> ScheduleLabs(Lab lab)
        {
            ValidationResult validation = validator.ScheduleLabs(lab);

            if (validation.Succeded)
            {
                context.Labs.Add(lab);
                await context.SaveChangesAsync();
            }

            return validation;
        }*/

        private List<ScheduleInterval> GetExistingProfessorSchedules(int hours, string professorId)
        {
            Queries query = new Queries();
            List<Lecture> lectures = query.GetLecturesForProfessor(professorId).ToList();
            return GetFreeLectureIntervals(lectures, hours);
        }

        private List<ScheduleInterval> GetExistingCourseSchedules(int hours, int subjectId)
        {
            int courseId = context.Subjects.Single(x => x.SubjectId == subjectId).Course.CourseId;
            List<Lecture> lectures = context.Lectures.Where(x => x.Subject.Course.CourseId == courseId).ToList();
            return GetFreeLectureIntervals(lectures, hours);
        }

        private List<ScheduleInterval> GetExistingClassroomSchedules(int hours, int classroomTypeId)
        {
            List<ScheduledClassroom> scheduledClassrooms = context.ScheduledClassrooms.Where(x => x.Classroom.ClassroomTypeId == classroomTypeId).ToList();
            return GetFreeClassroomIntervals(scheduledClassrooms, hours);
        }

        private List<ScheduleInterval> GetFreeLectureIntervals(List<Lecture> lectures, int hours)
        {
            List<ScheduleInterval> existingIntervals = new List<ScheduleInterval>();

            foreach (Lecture lecture in lectures)
            {
                existingIntervals.Add(new ScheduleInterval(lecture.ScheduledClassroom.ScheduleStart, lecture.ScheduledClassroom.ScheduleEnd, lecture.ScheduledClassroom.SchoolDay));
            }

            return ScheduleInterval.ComputeFreeIntervals(hours, existingIntervals);
        }
        
        private List<ScheduleInterval> GetFreeClassroomIntervals(List<ScheduledClassroom> scheduledClassrooms, int hours)
        {
            List<ScheduleInterval> existingIntervals = new List<ScheduleInterval>();

            foreach (ScheduledClassroom scheduledClassroom in scheduledClassrooms)
            {
                existingIntervals.Add(new ScheduleInterval(scheduledClassroom.ScheduleStart, scheduledClassroom.ScheduleEnd, scheduledClassroom.SchoolDay));
            }

            return ScheduleInterval.ComputeFreeIntervals(hours, existingIntervals);
        }
        private List<ScheduleInterval> MergeLists(List<ScheduleInterval> left, List<ScheduleInterval> right)
        {
            List<ScheduleInterval> result = new List<ScheduleInterval>();

            if (left.Count > right.Count)
            {
                for (int i = 0; i < left.Count; i++)
                {
                    ScheduleInterval current = left[i];
                    if (right.Any(x => x.IntervalStart == current.IntervalStart && x.IntervalEnd == current.IntervalEnd && x.SchoolDay.SchoolDayId == current.SchoolDay.SchoolDayId))
                    {
                        result.Add(current);
                    }
                }
            }
            else
            {
                for (int i = 0; i < right.Count; i++)
                {
                    ScheduleInterval current = right[i];
                    if (left.Any(x => x.IntervalStart == current.IntervalStart && x.IntervalEnd == current.IntervalEnd && x.SchoolDay.SchoolDayId == current.SchoolDay.SchoolDayId))
                    {
                        result.Add(current);
                    }
                }
            }

            return result;
        }

        public List<ScheduleInterval> GetFreeProfessorCourseAndClassroomTime(string professorId, int subjectId, int hours, int classroomTypeId)
        {
            List<ScheduleInterval> courseIntervals = GetExistingCourseSchedules(hours, subjectId);
            List<ScheduleInterval> professorIntervals = GetExistingProfessorSchedules(hours, professorId);

            List<ScheduleInterval> classroomIntervals = GetExistingClassroomSchedules(hours, classroomTypeId);

            var res = MergeLists(courseIntervals, professorIntervals);
            res = MergeLists(res, classroomIntervals);
            return res;
        }

        public List<ClassroomScheduleInterval> GetFreeClassroomsInSchedules(List<ScheduleInterval> intervals, int classroomTypeId)
        {
            List<ClassroomScheduleInterval> classroomsInSchedule = new List<ClassroomScheduleInterval>();

            List<Classroom> allClassrooms = context.Classrooms.Where(x => x.ClassroomTypeId == classroomTypeId).ToList();
            List<ScheduledClassroom> allScheduledClassrooms = context.ScheduledClassrooms.ToList();

            List<ScheduledClassroom> freeScheduledClassrooms = new List<ScheduledClassroom>();
            foreach (ScheduleInterval interval in intervals)
            {
                freeScheduledClassrooms.Clear();

                freeScheduledClassrooms = context.ScheduledClassrooms
                    .Where(x => x.ScheduleStart != interval.IntervalStart && 
                           x.ScheduleEnd != interval.IntervalEnd && 
                           x.SchoolDayId != interval.SchoolDay.SchoolDayId &&
                           x.Classroom.ClassroomType.ClassroomTypeId == classroomTypeId
                        )
                    .ToList();

                List<Classroom> freeClassrooms = new List<Classroom>();
                foreach (ScheduledClassroom scheduledClassroom in freeScheduledClassrooms)
                {
                    freeClassrooms.Add(context.Classrooms.Single(x => x.ClassroomId == scheduledClassroom.ClassroomId));
                }

                classroomsInSchedule.Add(new ClassroomScheduleInterval { Interval = interval, Classrooms = freeClassrooms });
            }

            foreach (Classroom classroom in allClassrooms)
            {
                if (classroomsInSchedule.Any(x => !x.Classrooms.Contains(classroom)))
                {
                    classroomsInSchedule.ForEach((y) => {
                        y.Classrooms.Add(classroom);
                    });
                }
            }

            return classroomsInSchedule;
        }

        public List<Classroom> GetFreeClassroomsInSchedule(ScheduleInterval interval, int classroomTypeId)
        {
            List<Classroom> freeClassrooms = new List<Classroom>();

            List<ScheduledClassroom> classroomsScheduledOnDay = context.ScheduledClassrooms.Where(x => x.SchoolDay.SchoolDayId == interval.SchoolDay.SchoolDayId).ToList();
            List<Classroom> allClassrooms = context.Classrooms.Where(x => x.ClassroomTypeId == classroomTypeId).ToList();

            foreach (ScheduledClassroom scheduledClassroom in classroomsScheduledOnDay)
            {
                if (scheduledClassroom.ScheduleStart != interval.IntervalStart && scheduledClassroom.ScheduleEnd != interval.IntervalEnd)
                {
                    freeClassrooms.Add(scheduledClassroom.Classroom);
                }
            }
            foreach (Classroom classroom in allClassrooms)
            {
                if (!classroomsScheduledOnDay.Any(x => x.ClassroomId == classroom.ClassroomId))
                {
                    freeClassrooms.Add(classroom);
                }
            }

            return freeClassrooms;
        }

        public List<ScheduleInterval> GetExistingCourseIntervals(int courseId, int year, int degree)
        {
            List<ScheduleInterval> intervals = new List<ScheduleInterval>();

            List<Lecture> lectures = context.Lectures.Where(x => x.Subject.Course.CourseId == courseId).ToList();
            List<Lab> labs = context.Labs.Where(x => x.Subject.Course.CourseId == courseId).ToList();

            List<ScheduledClassroom> scheduledClassrooms = context.ScheduledClassrooms.ToList();

            foreach (Lecture lecture in lectures)
            {
                intervals.Add(new ScheduleInterval(
                    lecture.ScheduledClassroom.ScheduleStart, 
                    lecture.ScheduledClassroom.ScheduleEnd, 
                    lecture.ScheduledClassroom.SchoolDay
                ));
            }
            foreach (Lab lab in labs)
            {
                intervals.Add(new ScheduleInterval(
                    lab.ScheduledClassroom.ScheduleStart,
                    lab.ScheduledClassroom.ScheduleEnd,
                    lab.ScheduledClassroom.SchoolDay
                ));
            }

            return intervals;
        }
        public List<ScheduleInterval> GetExistingProfessorIntervals(int subjectId)
        {
            string professorId = context.ProfessorsSubjects.First(y => y.SubjectId == subjectId).ProfessorId;
            List<Lecture> lectures = context.Lectures.Where(x => x.ProfessorId == professorId).ToList();

            List<ScheduleInterval> intervals = new List<ScheduleInterval>();
            foreach (Lecture lecture in lectures)
            {
                intervals.Add(new ScheduleInterval(
                    lecture.ScheduledClassroom.ScheduleStart,
                    lecture.ScheduledClassroom.ScheduleEnd,
                    lecture.ScheduledClassroom.SchoolDay
                ));             
            }
            return intervals;
        }
        public List<ScheduleInterval> GetExistingAssistantIntervals(int subjectId)
        {
            string assistantId = context.AssistantsSubjects.First(x => x.SubjectId == subjectId).AssistantId;
            List<Lab> labs = context.Labs.Where(x => x.AssistantId == assistantId).ToList();

            List<ScheduleInterval> intervals = new List<ScheduleInterval>();
            foreach (Lab lab in labs)
            {
                intervals.Add(new ScheduleInterval(
                    lab.ScheduledClassroom.ScheduleStart,
                    lab.ScheduledClassroom.ScheduleEnd,
                    lab.ScheduledClassroom.SchoolDay
                ));
            }
            return intervals;
        }

        public async Task<ValidationResult> NewSchedule(ScheduleViewModel model, List<ScheduleIntervalViewModel> intervals, bool isLab)
        {
            model.Intervals = intervals;
            ValidationResult validation = validator.NewSchedule(model.Intervals.Single(x => x.Id == model.SelectedIntervalId), model.SelectedClassroomId);

            if (validation.Succeded)
            {
                using (DbContextTransaction t = context.Database.BeginTransaction())
                {
                    try
                    {
                        ScheduleIntervalViewModel interval = model.Intervals.Single(x => x.Id == model.SelectedIntervalId);
                        context.ScheduledClassrooms.Add(new ScheduledClassroom()
                        {
                            ClassroomId = model.SelectedClassroomId,
                            ScheduleStart = interval.IntervalStart,
                            ScheduleEnd = interval.IntervalEnd,
                            SchoolDayId = interval.SchoolDay.SchoolDayId
                        });
                        await context.SaveChangesAsync();


                        if (isLab)
                        {
                            int scheduledClassroomId = context.ScheduledClassrooms
                                .Single(x => x.SchoolDayId == interval.SchoolDay.SchoolDayId &&
                                        x.ScheduleStart == interval.IntervalStart &&
                                        x.ScheduleEnd == interval.IntervalEnd
                                    ).ScheduledClassroomId;

                            string assistantId = context.AssistantsSubjects.Single(x => x.SubjectId == model.SubjectId).AssistantId;

                            context.Labs.Add(new Lab()
                            {
                                SubjectId = model.SubjectId,
                                AssistantId = assistantId,
                                ScheduledClassroomId = scheduledClassroomId
                            });

                            await context.SaveChangesAsync();    
                        }
                        

                        t.Commit();
                    }
                    catch (Exception e)
                    {
                        t.Rollback();
                        throw e;
                    }
                }

                
            }

            return validation;
        }

        public async Task<ValidationResult> AssignLab(string assistantId, int subjectId)
        {
            ValidationResult validation = await validator.AssignLab(assistantId, subjectId);
            if (validation)
            {
                context.AssistantsSubjects.Add(new AssistantsSubjects() 
                {
                    AssistantId = assistantId,
                    SubjectId = subjectId
                });
            }
            return validation;
        }
    }
}
