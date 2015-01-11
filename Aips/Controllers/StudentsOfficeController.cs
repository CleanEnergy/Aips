using EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL.StudentsOffice;
using System.Threading.Tasks;
using BL;
using System.IO;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Web.Script.Serialization;
using BL.Scheduling;

namespace Aips.Controllers
{
    [Authorize(Roles = "Administrator, StudentsOffice")]
    public class StudentsOfficeController : Controller
    {
        private Functions StudentsOfficeFunctions;
        private Validator StudentsOfficeValidator;

        public StudentsOfficeController()
        {
            StudentsOfficeFunctions = new Functions();
            StudentsOfficeValidator = new Validator();
        }

        public ActionResult Index()
        {
            return View("Dashboard");
        }

        #region Events
        public ActionResult PublishEvent()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PublishEvent(UniversityEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!Helpers.ControllerExtensions.CheckValidDate(model.Day, model.Month, model.Year))
                {
                    ModelState.AddModelError("", "The date is not valid.");
                }
                else
                {
                    try
                    {
                        UniversityEvent universityEvent = new UniversityEvent
                        {
                            Title = model.Title,
                            Description = model.Description,
                            DateAndTime = new DateTime(model.Year, model.Month, model.Day)
                        };

                        await StudentsOfficeFunctions.PublishUniversityEvent(universityEvent);

                        if (model.InformAllStudents)
                        {
                            await StudentsOfficeFunctions.InformAllStudentsOfUniversityEvent(universityEvent, "Invitation to " + universityEvent.Title, "You have been invited to the following event: ", "Students Office");
                        }

                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        return Helpers.ControllerExtensions.RedirectToError(this, e);
                    }
                }
            }
            return View(model);
        }
        #endregion
        #region Subjects
        public async Task<ActionResult> CreateSubject()
        {
            try
            {
                if (await StudentsOfficeValidator.CanCreateSubject())
                {
                    Queries query = new Queries();
                    ViewBag.Faculties = new SelectList(await query.GetAllFaculties(), "FacultyId", "Name");

                    return View();
                }
                else
                {
                    throw new Exception("No courses exist to create the subject in.");
                }
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSubject(SubjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ValidationResult result = await StudentsOfficeFunctions.CreateSubject(new Subject
                    {
                        Title = model.Title,
                        IsActive = model.IsActive,
                        Year = model.Year,
                        Degree = model.Degree,
                    }, model.CourseId, model.ProfessorId);

                    if (result.Succeded)
                    {
                        return RedirectToAction("Index");
                    }

                    Helpers.ControllerExtensions.AddValidationErrors(result, this);
                }
                catch (Exception e)
                {
                    Helpers.ControllerExtensions.RedirectToError(this, e);
                }
            }

            Queries query = new Queries();
            ViewBag.Faculties = new SelectList(await query.GetAllFaculties(), "FacultyId", "Name");

            return View(model);
        }
        public async Task<ActionResult> GetCoursesWithProfessors(int facultyId)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    throw new WebException();
                }

                Queries query = new Queries();
                ViewBag.Courses = query.GetCoursesForFaculty(facultyId);
                ViewBag.Professors = await query.GetAllProfessors();

                return PartialView("_CoursesWithProfessors");
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        public async Task<ActionResult> CheckExistingSubjects()
        {
            Queries queries = new Queries();
            List<SubjectViewModel> model = new List<SubjectViewModel>();

            foreach (Subject subject in await queries.GetAllSubjects())
            {
                model.Add(new SubjectViewModel { IsActive = subject.IsActive, Title = subject.Title, Year = subject.Year, Degree = subject.Degree });
            }

            return View(model);
        }
        #endregion
        #region Exams
        public async Task<ActionResult> ScheduleExam()
        {
            try
            {
                ValidationResult result = StudentsOfficeValidator.CanScheduleExam();
                if (!result.Succeded)
                {
                    return Helpers.ControllerExtensions.RedirectToInformation(this, result);
                }

                Queries query = new Queries();
                ViewBag.Subjects = new SelectList(await query.GetAllSubjects(), "SubjectId", "Title");
                ViewBag.Classrooms = new SelectList((await query.GetAllClassroomsAsync()).OrderBy((classroom) => { return classroom.Name; }), "ClassroomId", "Name");

                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ScheduleExam(ExamViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!Helpers.ControllerExtensions.CheckValidDate(model.Day, model.Month, model.Year))
                {
                    ModelState.AddModelError("", "The date is not valid.");
                }
                else
                {
                    try
                    {
                        DateTime examDate = new DateTime(model.Year, model.Month, model.Day, model.Hour, model.Minute, 0);
                        ValidationResult result = await StudentsOfficeFunctions.ScheduleExam(model.SubjectId, examDate, model.ClassroomId);

                        if (result.Succeded)
                        {
                            if (model.InformStudents)
                            {
                                await StudentsOfficeFunctions.InformStudentsOfScheduledExam(model.SubjectId, examDate, "StudentsOffice");
                            }

                            return RedirectToAction("Index");
                        }

                        Helpers.ControllerExtensions.AddValidationErrors(result, this);
                    }
                    catch (Exception e)
                    {
                        Helpers.ControllerExtensions.RedirectToError(this, e);
                    }
                }

            }
            Queries query = new Queries();
            ViewBag.Subjects = new SelectList(await query.GetAllSubjects(), "SubjectId", "Title");
            ViewBag.Classrooms = new SelectList((await query.GetAllClassroomsAsync()).OrderBy((classroom) => { return classroom.Name; }), "ClassroomId", "Name");

            return View(model);
        }
        public async Task<ActionResult> ChangeExamDate(int examId)
        {
            Queries queries = new Queries();
            try
            {
                Exam exam = await queries.GetExam(examId);

                return View(new ExamViewModel
                {
                    ExamId = exam.ExamId,
                    SubjectName = exam.Subject.Title,
                    Day = exam.DateAndTime.Day,
                    Month = exam.DateAndTime.Month,
                    Year = exam.DateAndTime.Year,
                    Minute = exam.DateAndTime.Minute,
                    Hour = exam.DateAndTime.Hour,
                    SubjectId = exam.Subject.SubjectId
                });
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeExamDate(ExamViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Queries queries = new Queries();
                    if (!Helpers.ControllerExtensions.CheckValidDate(model.Day, model.Month, model.Year))
                    {
                        ModelState.AddModelError("", "The date is not valid.");
                    }
                    else
                    {
                        DateTime examDate = new DateTime(model.Year, model.Month, model.Day, model.Hour, model.Minute, 0);
                        await StudentsOfficeFunctions.ChangeExamDate(await queries.GetExam(model.ExamId), examDate);

                        if (model.InformStudents)
                        {
                            await StudentsOfficeFunctions.InformStudentsOfRescheduledExam(model.SubjectId, examDate, "StudentsOffice");
                        }

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {
                    return Helpers.ControllerExtensions.RedirectToError(this, e);
                }
            }

            return View(model);
        }

        public async Task<ActionResult> CancelExam(int examId)
        {
            Queries queries = new Queries();
            try
            {
                Exam exam = await queries.GetExam(examId);
                return View(new ExamViewModel
                {
                    ExamId = exam.ExamId,
                    SubjectName = exam.Subject.Title,
                    ExamDateAndTime = exam.DateAndTime,
                    SubjectId = exam.Subject.SubjectId
                });
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelExam(ExamViewModel model)
        {
            try
            {
                await StudentsOfficeFunctions.CancelExam(model.ExamId);

                if (model.InformStudents)
                {
                    await StudentsOfficeFunctions.InformStudentsOfCancelledExam(model.SubjectId, model.ExamDateAndTime, "StudentsOffice");
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> StoreExamPDF()
        {
            Queries query = new Queries();
            try
            {
                ViewBag.Faculties = new SelectList(await query.GetAllFaculties(), "FacultyId", "Name");
                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> StoreExamPDFPreUpload(int examId)
        {
            try
            {
                Queries query = new Queries();
                ViewBag.SignedOnStudents = await query.GetStudentsSignedOnExam(examId);
                ViewBag.Copies = await query.GetExamCopies(examId);
                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);   
            }
        }

        public ActionResult StoreExamPDFUpload(string studentId, int examId)
        {
            try
            {
                TempData.Clear();
                TempData.Add("StudentId", studentId);
                TempData.Add("ExamId", examId);

                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);   
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StoreExamPDFUpload(HttpPostedFileBase examCopy)
        {
            if (examCopy != null)
            {
                try
                {
                    await StudentsOfficeFunctions.StoreExamPDF((string)TempData["StudentId"], (int)TempData["ExamId"], examCopy.InputStream, examCopy.ContentType);
                    return RedirectToAction("StoreExamPDFPreUpload", new { examId = (int)TempData["ExamId"] });
                }
                catch (Exception e)
                {
                    return Helpers.ControllerExtensions.RedirectToError(this, e);
                }
            }

            ModelState.AddModelError("", "Please select a file.");
            return View();
        }

        public ActionResult GetFacultyCoursesCopy(int facultyId)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    throw new WebException();
                }

                Queries query = new Queries();
                ViewBag.Courses = new SelectList(query.GetCoursesForFaculty(facultyId), "CourseId", "Name");

                return PartialView("_CoursesForFaculty");
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        public ActionResult GetExamsForSubject(int subjectId)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    throw new WebException();
                }

                Queries query = new Queries();
                ViewBag.Exams = query.GetExamsForSubject(subjectId);

                return PartialView("_ExamsForSubject");
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> CheckScheduledExams()
        {
            Queries queries = new Queries();
            try
            {
                List<ExamViewModel> model = new List<ExamViewModel>();

                foreach (Exam exam in await queries.GetAllScheduledExams())
                {
                    model.Add(new ExamViewModel
                    {
                        SubjectId = exam.Subject.SubjectId,
                        SubjectName = exam.Subject.Title,
                        ExamDateAndTime = exam.DateAndTime,
                        ExamId = exam.ExamId
                    });
                }

                return View(model);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        public async Task<ActionResult> CheckUploadedExams()
        {
            try
            {
                Queries query = new Queries();
                ViewBag.Exams = await query.GetAllExams();

                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        public async Task<ActionResult> CheckUploadedExamsPerExam(int examId)
        {
            try
            {
                Queries query = new Queries();
                List<ExamCopy> copies = await query.GetUploadedExamsPerExam(examId);

                ViewBag.Copies = copies;

                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        public async Task<ActionResult> CheckUploadedExam(int uploadId)
        {
            try
            {
                Queries query = new Queries();
                ExamCopy copy = await query.GetExamCopy(uploadId);
                return File(copy.ExamImage, copy.MimeType);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        #endregion


        #region Faculties
        public ActionResult AddFaculty()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddFaculty(FacultyViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ValidationResult result = await StudentsOfficeFunctions.AddFaculty(model.Name);
                    if (result.Succeded)
                    {
                        return RedirectToAction("Index");
                    }
                    Helpers.ControllerExtensions.AddValidationErrors(result, this);
                }
                catch (Exception e)
                {
                    return Helpers.ControllerExtensions.RedirectToError(this, e);
                }
            }

            return View(model);
        }
        public async Task<ActionResult> CheckExistingFaculties()
        {
            List<FacultyViewModel> model = new List<FacultyViewModel>();

            Queries query = new Queries();

            foreach (Faculty faculty in await query.GetAllFaculties())
            {
                model.Add(new FacultyViewModel
                {
                    Name = faculty.Name
                });
            }

            return View(model);
        }
        #endregion
        #region SchoolYears
        public ActionResult AddSchoolYear()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSchoolYear(SchoolYearViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Helpers.ControllerExtensions.CheckValidDate(
                        model.YearStartDay, model.YearStartMonth, model.YearStartYear) &&
                        Helpers.ControllerExtensions.CheckValidDate(model.YearEndDay, model.YearEndMonth, model.YearEndYear)
                    )
                    {
                        ValidationResult result = await StudentsOfficeFunctions.AddSchoolYear(
                            new DateTime(model.YearStartYear, model.YearStartMonth, model.YearStartDay),
                            new DateTime(model.YearEndYear, model.YearEndMonth, model.YearEndDay)
                        );

                        if (result.Succeded)
                        {
                            return RedirectToAction("Index");
                        }

                        Helpers.ControllerExtensions.AddValidationErrors(result, this);
                    }
                    else
                    {
                        ModelState.AddModelError("", "The provided date(s) are invalid");
                    }
                }
                catch (Exception e)
                {
                    return Helpers.ControllerExtensions.RedirectToError(this, e);
                }
            }

            return View(model);
        }
        public ActionResult ViewAllSchoolYears()
        {
            try
            {
                List<SchoolYearViewModel> model = new List<SchoolYearViewModel>();

                foreach (SchoolYear year in StudentsOfficeFunctions.GetAllSchoolYears())
                {
                    model.Add(new SchoolYearViewModel
                    {
                        YearStartDay = year.YearStart.Day,
                        YearStartMonth = year.YearStart.Month,
                        YearStartYear = year.YearStart.Year,
                        YearEndDay = year.YearEnd.Day,
                        YearEndMonth = year.YearEnd.Month,
                        YearEndYear = year.YearEnd.Year
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
        #region Courses
        public async Task<ActionResult> AddCourse()
        {
            try
            {
                if (!StudentsOfficeValidator.CanAddCourse())
                {
                    throw new Exception("No faculties exist to add the courses to!");
                }
                Queries query = new Queries();
                ViewBag.Faculties = new SelectList(await query.GetAllFaculties(), "FacultyId", "Name");
                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCourse(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ValidationResult result = await StudentsOfficeFunctions.AddCourse(new Course
                    {
                        Name = model.Name,
                    }, model.FacultyId);

                    if (result.Succeded)
                    {
                        return RedirectToAction("Index");
                    }
                    Helpers.ControllerExtensions.AddValidationErrors(result, this);
                }
                catch (Exception e)
                {
                    return Helpers.ControllerExtensions.RedirectToError(this, e);
                }
            }
            Queries query = new Queries();
            ViewBag.Faculties = new SelectList(await query.GetAllFaculties(), "FacultyId", "Name", model.FacultyId);
            return View(model);
        }
        public ActionResult ViewAllCourses()
        {
            try
            {
                List<CourseViewModel> model = new List<CourseViewModel>();
                Queries query = new Queries();
                List<Course> courses = query.GetAllCourses();
                foreach (Course course in courses)
                {
                    model.Add(new CourseViewModel
                    {
                        Faculty = course.Faculty,
                        Name = course.Name,
                        CourseId = course.CourseId
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
        #region Students
        public async Task<ActionResult> AddStudent()
        {
            try
            {
                if (!StudentsOfficeValidator.CanAddStudent())
                {
                    throw new Exception("Cannot add a student: no school years exist or no courses exist.");
                }

                Queries query = new Queries();

                List<SchoolYear> schoolYears = query.GetAvailableSchoolYears();
                List<Faculty> faculties = await query.GetAllFaculties();
                var selectList = schoolYears.Select(x => new { Display = x.YearStart.Year.ToString() + "/" + x.YearEnd.Year.ToString(), Value = x.SchoolYearId });

                ViewBag.SchoolYears = new SelectList(selectList, "Value", "Display");
                ViewBag.Faculties = new SelectList(faculties, "FacultyId", "Name");
                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddStudent(CreateStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ValidationResult result = await StudentsOfficeFunctions.AddStudent(new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.EMail,
                        PhoneNumber = model.PhoneNumber,
                        UserName = model.UserName,
                    }, model.Password, model.CourseId, model.SchoolYearId);

                    if (result.Succeded)
                    {
                        return RedirectToAction("Index");
                    }

                    Helpers.ControllerExtensions.AddValidationErrors(result, this);
                }
                catch (Exception e)
                {
                    return Helpers.ControllerExtensions.RedirectToError(this, e);
                }
            }

            Queries query = new Queries();

            List<SchoolYear> schoolYears = query.GetAvailableSchoolYears();
            List<Faculty> faculties = await query.GetAllFaculties();
            var selectList = schoolYears.Select(x => new { Display = x.YearStart.Year.ToString() + "/" + x.YearEnd.Year.ToString(), Value = x.SchoolYearId });

            ViewBag.SchoolYears = new SelectList(selectList, "Value", "Display");
            ViewBag.Faculties = new SelectList(faculties, "FacultyId", "Name");

            return View(model);
        }
        public JsonResult GetFacultyCourses(int facultyId)
        {
            Queries query = new Queries();
            List<Course> courses = query.GetCoursesForFaculty(facultyId);

            List<dynamic> selectList = new List<dynamic>();
            selectList.Add(new { Id = -1, Name = "-- Select a course --" });
            foreach (Course course in courses)
            {
                selectList.Add(new { Id = course.CourseId, Name = course.Name });
            }

            return Json(new { List = selectList }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public JsonResult GetFaculties()
        {
            Queries query = new Queries();
            return Json(new { List = query.GetAllFaculties() }, JsonRequestBehavior.AllowGet);
        }
        
        #region Surveys
        public ActionResult CreateSurvey()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateSurvey(SurveyViewModel model)
        {
            try
            {
                Survey survey = new Survey { Title = model.Title, Visible = model.Visible };
                StudentsOfficeFunctions.AddSurvey(survey);

                Queries query = new Queries();
                int surveyId = query.GetSurveyId(survey.Title);
                List<SurveyQuestionType> questionTypes = query.GetAllSurveyQuestionTypes();

                List<SurveyQuestion> questions = new List<SurveyQuestion>();

                object surveyData = new JavaScriptSerializer().DeserializeObject(model.SurveyData);
                object[] surveyDataArray = (object[])surveyData;

                // Loop through questions - a data piece is a question
                for (int i = 0; i < surveyDataArray.Count(); i++)
                {
                    Dictionary<string, object> dataPiece = (Dictionary<string, object>)surveyDataArray.ElementAt(i);

                    SurveyQuestion question = new SurveyQuestion();
                    question.SurveyId = surveyId;
                    question.Question = (string)dataPiece["questionText"];

                    string questionType = (string)dataPiece["type"];
                    question.SurveyQuestionTypeId = questionTypes.Single(x => x.Name.ToLower() == questionType.ToLower()).TypeId;

                    StudentsOfficeFunctions.AddSurveyQuestion(question);
                    int questionId = query.GetSurveyQuestionId(surveyId, question.Question);

                    List<SurveyAnswer> answers = new List<SurveyAnswer>();
                    object[] answersArray = (object[])dataPiece["answers"];

                    if (questionType == "multiple" || questionType == "single")
                    {
                        for (int j = 0; j < answersArray.Count(); j++)
                        {
                            answers.Add(new SurveyAnswer
                            {
                                SurveyQuestionId = questionId,
                                Answer = answersArray.ElementAt(j).ToString()
                            });
                        }
                    }
                    else if (questionType == "text")
                    {
                        answers.Add(new SurveyAnswer
                        {
                            SurveyQuestionId = questionId,
                            Answer = answersArray.ElementAt(0).ToString()
                        });
                    }

                    StudentsOfficeFunctions.AddSurveyAnswers(answers);
                }
                return Json(new { id = surveyId /* once the survey is created, return it's id so it can be tested*/ });
            }
            catch (Exception e)
            {
                Helpers.ControllerExtensions.RedirectToError(this, e);
                throw e;
            }

        }
        public ActionResult TestSurvey(int id)
        {
            try
            {
                Queries query = new Queries();
                TestSurveyViewModel model = new TestSurveyViewModel();
                model.Survey = query.GetSurvey(id);
                model.SurveyQuestions = query.GetSurveyQuestions(id);
                model.SurveyAnswers = query.GetSurveyAnswers(id, model.SurveyQuestions);
                return View(model);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        public ActionResult CheckSurveys()
        {
            List<SurveyViewModel> model = new List<SurveyViewModel>();

            Queries query = new Queries();
            List<Survey> surveys = query.GetAllSurveys();

            foreach (Survey survey in surveys)
            {
                model.Add(new SurveyViewModel
                {
                    SurveyId = survey.SurveyId,
                    Title = survey.Title,
                    Visible = survey.Visible
                });
            }

            return View(model);
        }
        #endregion
        #region Subjects
        public async Task<ActionResult> AssignSubjectToProfessor()
        {
            try
            {
                ValidationResult validation = StudentsOfficeValidator.CanAssignSubject();

                if (!validation.Succeded)
                {
                    return Helpers.ControllerExtensions.RedirectToInformation(this, validation);
                }

                Queries query = new Queries();

                List<Subject> subjects = await query.GetUnassignedSubjects();
                ViewBag.Subjects = new SelectList(subjects.OrderBy(x => x.Title), "SubjectId", "Title");

                List<User> professors = await query.GetAllProfessors();
                professors = professors.OrderBy(x => x.LastName).ToList();
                var selectList = professors.Select(x => new { Display = x.LastName + " " + x.FirstName, Value = x.Id }).ToList();
                ViewBag.Professors = new SelectList(selectList, "Value", "Display");

                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignSubjectToProfessor(AssignSubjectToProfessorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ValidationResult result = await StudentsOfficeFunctions.AssignSubjectToProfessor(model.SubjectId, model.ProfessorId);

                    if (result.Succeded)
                    {
                        return RedirectToAction("Index");
                    }

                    Helpers.ControllerExtensions.AddValidationErrors(result, this);
                }
                catch (Exception e)
                {
                    return Helpers.ControllerExtensions.RedirectToError(this, e);
                }
            }

            return View(model);
        }

        public async Task<ActionResult> AssignSubjectToAssistant()
        {
            try
            {
                Queries query = new Queries();

                List<Subject> subjects = await query.GetUnassignedSubjectsForAssistants();
                ViewBag.Subjects = new SelectList(subjects.OrderBy(x => x.Title), "SubjectId", "Title");

                List<User> assistants = await query.GetAllAssistants();
                assistants = assistants.OrderBy(x => x.LastName).ToList();
                var selectList = assistants.Select(x => new { Display = x.LastName + " " + x.FirstName, Value = x.Id }).ToList();
                ViewBag.Assistants = new SelectList(selectList, "Value", "Display");

                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignSubjectToAssistant(AssignSubjectToAssistantViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ValidationResult result = await StudentsOfficeFunctions.AssignSubjectToAssistant(model.SubjectId, model.AssistantId);

                    if (result.Succeded)
                    {
                        return RedirectToAction("Index");
                    }

                    Helpers.ControllerExtensions.AddValidationErrors(result, this);
                }
                catch (Exception e)
                {
                    return Helpers.ControllerExtensions.RedirectToError(this, e);
                }
            }

            return View(model);
        }

        public async Task<ActionResult> CheckAssignedSubjects()
        {
            List<AssignedSubjectViewModel> model = new List<AssignedSubjectViewModel>();

            Queries query = new Queries();
            List<ProfessorsSubjects> assignedSubjects = await query.GetAssignedSubjects();

            foreach (ProfessorsSubjects assignedSubject in assignedSubjects)
            {
                model.Add(new AssignedSubjectViewModel
                {
                    ProfessorFirstName = assignedSubject.Professor.FirstName,
                    ProfessorLastName = assignedSubject.Professor.LastName,
                    ProfessorId = assignedSubject.ProfessorId,
                    SubjectTitle = assignedSubject.Subject.Title,
                    SubjectId = assignedSubject.SubjectId
                });
            }

            return View(model);
        }
        #endregion
        #region Schedule

        public async Task<ActionResult> NewSchedule()
        {
            try
            {
                ValidationResult result = StudentsOfficeValidator.CanCreateNewSchedules();

                if (result.Succeded)
                {
                    Queries query = new Queries();
                    ViewBag.Faculties = new SelectList(await query.GetAllFaculties(), "FacultyId", "Name");

                    return View();
                }
                return Helpers.ControllerExtensions.RedirectToInformation(this, result);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);   
            }
        }
        public ActionResult GetCoursesForFaculty(int facultyId)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    throw new WebException();
                }

                Queries query = new Queries();
                ViewBag.Courses = new SelectList(query.GetCoursesForFaculty(facultyId), "CourseId", "Name");

                return PartialView("_CoursesForFaculty");
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }

        }
        public async Task<ActionResult> GetCourseSubjects(int courseId, int year, int degree)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    throw new WebException();
                }

                Queries query = new Queries();
                ViewBag.Subjects = await query.GetAssignedSubjectsForCourse(courseId, year, degree);
                return PartialView("_CourseSubjects");
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        public async Task<ActionResult> GetTimetable(int hours, int courseId, int year, int degree, int subjectId, bool isLab)
        {
            try
            {
                TempData.Remove("Intervals");

                ScheduleViewModel model = new ScheduleViewModel();

                List<ScheduleInterval> existingIntervals = StudentsOfficeFunctions.GetExistingCourseIntervals(courseId, year, degree);

                if (isLab)
                {
                    existingIntervals.AddRange(StudentsOfficeFunctions.GetExistingAssistantIntervals(subjectId));
                }
                else
                {
                    existingIntervals.AddRange(StudentsOfficeFunctions.GetExistingProfessorIntervals(subjectId));
                }

                List<ScheduleInterval> intervals = ScheduleInterval.ComputeFreeIntervals(hours, existingIntervals);

                Queries query = new Queries();
                string classroomName = isLab ? "Lab" : "LectureHall";
                int classroomTypeId = (await query.GetClassroomTypes()).Single(x => x.Name == classroomName).ClassroomTypeId;
                List<ClassroomScheduleInterval> classrooms = StudentsOfficeFunctions.GetFreeClassroomsInSchedules(intervals, classroomTypeId);

                foreach (ScheduleInterval interval in intervals)
                {
                    model.Intervals.Add(new ScheduleIntervalViewModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        IntervalStart = interval.IntervalStart,
                        IntervalEnd = interval.IntervalEnd,
                        SchoolDay = interval.SchoolDay,
                        Duration = interval.Duration,
                        Classrooms = StudentsOfficeFunctions.GetFreeClassroomsInSchedule(interval, classroomTypeId)
                    });
                }

                TempData.Add("Intervals", model.Intervals);

                return PartialView("_Timetable", model);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewSchedule(ScheduleViewModel model, bool isLab)
        {
            if (ModelState.IsValid)
            {
                ValidationResult result = await StudentsOfficeFunctions.NewSchedule(model, (List<ScheduleIntervalViewModel>)TempData["Intervals"], isLab);

                if (result.Succeded)
                {
                    return RedirectToAction("Index");
                }
                Helpers.ControllerExtensions.AddValidationErrors(result, this);
            }

            ModelState.AddModelError("", "Could not create the schedule.");

            Queries query = new Queries();
            ViewBag.Faculties = new SelectList(await query.GetAllFaculties(), "FacultyId", "Name");
            return View(model);
        }

        #endregion
    }
}