using BL;
using BL.Professors;
using EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Aips.Controllers
{
    public class ProfessorController : Controller
    {
        private Functions ProfessorFunctions;
        private Validator ProfessorValidator;

        public ProfessorController()
        {
            ProfessorFunctions = new Functions();
            ProfessorValidator = new Validator();
        }

        public ActionResult Index()
        {
            return View("Dashboard");
        }

        public async Task<ActionResult> AllExams()
        {
            try
            {
                Queries query = new Queries();
                List<Exam> exams = await query.GetAllExams();

                return View(exams);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> GradeExam(int examId)
        {
            try
            {
                GradeExamViewModel model = new GradeExamViewModel();
                model.ExamId = examId;

                Queries query = new Queries();
                List<SignOnExam> informations = await query.GetStudentsSignedOnExam(examId);
                model.SignedOnStudents = informations;

                List<ExamStudent> alreadyGraded = await query.GetAlreadyGradedExams(examId);

                ViewBag.AlreadyGraded = alreadyGraded;

                model.Exam = await query.GetExam(examId);

                return View(model);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> EnterExamGrade(int examId, string studentId)
        {
            try
            {
                ValidationResult validation = ProfessorValidator.CanEnterExamGrade(examId, studentId);
                if (!validation.Succeded)
                {
                    return Helpers.ControllerExtensions.RedirectToInformation(this, validation);
                }

                Queries query = new Queries();
                EnterExamGradeViewModel model = new EnterExamGradeViewModel() 
                {
                    Exam = await query.GetExam(examId),
                    Student = query.GetUserById(studentId),
                    ExamId = examId,
                    StudentId = studentId
                };

                return View(model);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> ViewExamCopy(int examId, string studentId)
        {
            try
            {
                Queries query = new Queries();
                ExamCopy copy = await query.GetExamCopy(examId, studentId);
                return File(copy.ExamImage, copy.MimeType);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);   
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnterExamGrade(EnterExamGradeViewModel model)
        {
            try
            {
                Queries query = new Queries();
                if (ModelState.IsValid)
                {
                    ValidationResult result = await ProfessorFunctions.EnterExamGrade(model.StudentId, model.ExamId, model.Grade);

                    if (result.Succeded)
                    {
                        if (model.InformStudent)
                        {
                            Exam exam = await query.GetExam(model.ExamId);

                            StringBuilder sb = new StringBuilder();
                            sb.Append("The exam for " + exam.Subject.Title + " that took place on " + exam.DateAndTime.ToShortDateString()
                                + " at " + exam.DateAndTime.ToShortTimeString());
                            sb.Append(" has been graded. You can view it <a href=");
                            sb.Append('"');
                            sb.Append("/Student/ViewExamDetails?examId=" + model.ExamId);
                            sb.Append('"');
                            sb.Append(">here</a>");

                            BL.Messaging.Functions messagingFunctions = new BL.Messaging.Functions();
                            await messagingFunctions.SendMessage("An exam has been graded", sb.ToString(), "StudentsOffice", query.GetUsernameById(model.StudentId));
                            
                        }

                        return RedirectToAction("GradeExam", new { examId = model.ExamId });
                    }

                    Helpers.ControllerExtensions.AddValidationErrors(result, this);
                }

                model.Student = query.GetUserById(model.StudentId);
                model.Exam = await query.GetExam(model.ExamId);

                return View(model);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

    }
}