using BL;
using BL.Assistant;
using EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Aips.Controllers
{
    [Authorize(Roles="Assistant, Administrator")]
    public class AssistantController : Controller
    {
        private Validator AssistantValidator;
        private Functions AssistantFunctions;

        public AssistantController()
        {
            AssistantValidator = new Validator();
            AssistantFunctions = new Functions();
        }

        public ActionResult Index()
        {
            return View("Dashboard");
        }

        public async Task<ActionResult> AllLabs()
        {
            try
            {
                List<LabViewModel> model = new List<LabViewModel>();

                Queries query = new Queries();
                string assistantId = query.GetUserIdByUsername(User.Identity.Name);
                foreach (Lab lab in await query.GetAllLabsForAssistant(assistantId))
                {
                    model.Add(new LabViewModel()
                    {
                        LabId = lab.LabId,
                        StartTime = lab.ScheduledClassroom.ScheduleStart,
                        EndTime = lab.ScheduledClassroom.ScheduleEnd,
                        SubjectId = lab.SubjectId,
                        Subject = lab.Subject,
                        SchoolDay = lab.ScheduledClassroom.SchoolDay
                    });
                }


                return View(model);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> MarkLabPresence(int labId)
        {
            try
            {
                TempData.Remove("LabId");
                TempData.Add("LabId", labId);
                Queries query = new Queries();
                ViewBag.Lab = await query.GetLab(labId);
                List<MarkLabPresenceViewModel> model = await GetLabPresenceViewModel(labId);

                return View(model);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MarkLabPresence(string ids)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string[] array = (string[])serializer.Deserialize(ids, typeof(string[]));
                int labId = (int)TempData["LabId"];

                ValidationResult result = await AssistantFunctions.MarkLabPresence(array, labId);
                if (result)
                {
                    return RedirectToAction("Index");
                }

                Queries query = new Queries();
                ViewBag.Lab = await query.GetLab(labId);
                List<MarkLabPresenceViewModel> model = await GetLabPresenceViewModel(labId);

                return View(model);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }
        private async Task<List<MarkLabPresenceViewModel>> GetLabPresenceViewModel(int labId)
        {
            Queries query = new Queries();
            List<User> assignedStudents = await query.GetStudentsForLab(labId);

            List<MarkLabPresenceViewModel> model = new List<MarkLabPresenceViewModel>();

            foreach (User student in assignedStudents)
            {
                model.Add(new MarkLabPresenceViewModel()
                {
                    StudentId = student.Id,
                    StudentFirstName = student.FirstName,
                    StudentLastName = student.LastName
                });
            }
            return model;
        }
    }
}