using BL;
using BL.Students;
using EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Aips.Controllers
{
    [Authorize(Roles = "Administrator, Student")]
    public class StudentController : Controller
    {
        private Functions StudentFunctions;
        private Validator StudentValidator;

        public StudentController()
        {
            StudentFunctions = new Functions();
            StudentValidator = new Validator();
        }

        public ActionResult Index()
        {
            return View("Dashboard");
        }

        #region SignOnInformation

        public async Task<ActionResult> ViewAllSignOnInformation()
        {
            try
            {
                Queries query = new Queries();
                string studentId = query.GetUserIdByUsername(User.Identity.Name);
                List<SignOnInformation> allInformation = await query.GetAllSignOnInformationsForStudent(studentId);

                foreach (SignOnInformation info in allInformation)
                {
                    info.Course = query.GetCourse(info.CourseId);
                }

                return View(allInformation);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);   
            }
        }

        #endregion

        #region Subjects

        public async Task<ActionResult> ViewAllSubjects()
        {
            try
            {
                Queries query = new Queries();
                string studentId = query.GetUserIdByUsername(User.Identity.Name);
                List<Subject> subjects = await query.GetAllSubjectsForStudent(studentId);

                List<SubjectViewModel> model = new List<SubjectViewModel>();
                foreach (Subject subject in subjects)
                {
                    float? grade = query.GetGradeForStudent(subject.SubjectId, studentId);

                    model.Add(new SubjectViewModel() 
                    {
                        Title = subject.Title,
                        Year = subject.Year,
                        Degree = subject.Degree,
                        IsActive = subject.IsActive,
                        Grade = grade != null ? grade.ToString() : "/"
                    });
                }

                return View(model);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);   
            }
        }

        #endregion

        #region Exams

        public async Task<ActionResult> SignOnExam()
        {
            Queries query = new Queries();
            ViewBag.Exams = await query.GetExamsAvailableForStudent(User.Identity.Name);

            return View();
        }

        public async Task<ActionResult> PreExamSignOn(int examId)
        {
            try
            {
                Queries query = new Queries();
                Exam exam = await query.GetExam(examId);

                ExamViewModel model = new ExamViewModel()
                {
                    ExamId = exam.ExamId,
                    ExamDateAndTime = exam.DateAndTime,
                    SubjectName = exam.Subject.Title,
                    ClassroomName = exam.Classroom.Name
                };

                return View(model);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PreExamSignOn(ExamViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new HttpRequestValidationException();
                }

                ValidationResult result = await StudentFunctions.SignOnExam(model.ExamId, User.Identity.Name);
                if (result.Succeded)
                {
                    return RedirectToAction("Index");
                }
                return Helpers.ControllerExtensions.RedirectToInformation(this, result);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> CheckOpenExamSignOns()
        {
            try
            {
                Queries query = new Queries();
                ViewBag.SignOns = await query.GetOpenExamSignOns(User.Identity.Name);
                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> SignOffExam(int examId)
        {
            try
            {
                Queries query = new Queries();
                Exam exam = await query.GetExam(examId);

                ValidationResult result = StudentValidator.CanSignOffExam(examId, query.GetUserIdByUsername(User.Identity.Name));

                if (result.Succeded)
                {
                    ExamViewModel model = new ExamViewModel()
                    {
                        ExamId = exam.ExamId,
                        ExamDateAndTime = exam.DateAndTime,
                        SubjectName = exam.Subject.Title,
                        ClassroomName = exam.Classroom.Name
                    };

                    return View(model);
                }
                return Helpers.ControllerExtensions.RedirectToInformation(this, result);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignOffExam(ExamViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new HttpRequestValidationException();
                }
                ValidationResult result = await StudentFunctions.SignOffExam(model.ExamId, User.Identity.Name);
                if (result.Succeded)
                {
                    return RedirectToAction("Index");
                }
                return Helpers.ControllerExtensions.RedirectToInformation(this, result);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> CheckAllExamSignOns()
        {
            try
            {
                Queries query = new Queries();
                string studentId = query.GetUserIdByUsername(User.Identity.Name);
                ViewBag.SignOns = await query.GetAllSignOnsForStudent(studentId);

                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> CheckExamCopies()
        {
            try
            {
                Queries query = new Queries();
                string studentId = query.GetUserIdByUsername(User.Identity.Name);
                ViewBag.Copies = await query.GetAllExamCopiesForStudent(studentId);

                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        public async Task<ActionResult> OpenExamCopy(int examId)
        {
            try
            {
                Queries query = new Queries();
                string studentId = query.GetUserIdByUsername(User.Identity.Name);
                ExamCopy file = await query.GetExamCopy(examId, studentId);
                return File(file.ExamImage, file.MimeType);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> ViewExamDetails(int examId)
        {
            try
            {
                Queries query = new Queries();
                string studentId = query.GetUserIdByUsername(User.Identity.Name);
                ExamStudent examStudent = await query.GetExamForStudent(examId, studentId);
                ExamViewModel model = new ExamViewModel() 
                {
                    ExamDateAndTime = examStudent.Exam.DateAndTime,
                    SubjectName = examStudent.Exam.Subject.Title,
                    Grade = examStudent.Grade,
                    SubjectId = examStudent.Exam.SubjectId,
                    ExamId = examId
                };
                return View(model);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);   
            }
        }

        public async Task<ActionResult> ViewAllExamGrades()
        {
            try
            {
                Queries query = new Queries();
                string studentId = query.GetUserIdByUsername(User.Identity.Name);

                ViewBag.Exams = await query.GetAllStudentsExams(studentId);

                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        #endregion

        #region Labs

        public async Task<ActionResult> ViewLabPresence()
        {
            try
            {
                List<LabPresenceForSubjectViewModel> model = new List<LabPresenceForSubjectViewModel>();

                Queries query = new Queries();
                string studentId = query.GetUserIdByUsername(User.Identity.Name);

                List<ScheduledLab> scheduledLabs = await query.GetScheduledLabsForStudentsSubjects(studentId);

                List<StudentsLabs> presentList = await query.GetLabPresenceForStudent(studentId);

                int presenceForSubjectSum = 0;
                int scheduledLabsTotal = 0;

                IEnumerable<IGrouping<int, ScheduledLab>> scheduledLabsBySubject = scheduledLabs.GroupBy(x => x.Lab.SubjectId);

                foreach (var labGroupBySubject in scheduledLabsBySubject)
                {
                    int subjectId = labGroupBySubject.Key;
                    string subjectName = query.GetSubject(subjectId).Title;

                    foreach (ScheduledLab scheduledLab in labGroupBySubject)
                    {
                        if (presentList.Any(x => x.ScheduledLabId == scheduledLab.ScheduledLabId && x.WasPresent))
	                    {
		                    presenceForSubjectSum++;
	                    }
                        scheduledLabsTotal++;
                    }

                    model.Add(new LabPresenceForSubjectViewModel() 
                    {
                        SubjectName = subjectName,
                        PresencePercent = ((float)(presenceForSubjectSum) / (float)(scheduledLabsTotal)) * 100
                    });
                }

                return View(model);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        #endregion
    }
    
}