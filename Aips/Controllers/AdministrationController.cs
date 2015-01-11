using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Web.Configuration;
using System.Web.Hosting;
using Logging;
using BL;
using EntityClasses;
using BL.Administrator;

namespace Aips.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        private Functions AdministratorFunctions;

        public AdministrationController()
        {
            AdministratorFunctions = new Functions();
        }

        public ActionResult Index()
        {
            return View("AdminPanel");
        }

        public ActionResult CreateUser()
        {
            try
            {
                Queries query = new Queries();

                ViewBag.Roles = new SelectList(query.GetAllRoles(), "Id", "Name");
                return View();
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IdentityResult result = await AdministratorFunctions.CreateUserAccount(new User()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.UserName,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.EMail
                    }, model.Password, model.RoleId);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    Helpers.ControllerExtensions.AddErrors(this, result);
                }
                catch (Exception e)
                {
                    return Helpers.ControllerExtensions.RedirectToError(this, e);
                }
            }
            ViewBag.Roles = Role.GetAll();
            return View(model);
        }

        public ActionResult Accounts()
        {
            return View(new LockAccountViewModel { UserCollection = new List<User>() });
        }

        public ActionResult SearchAccounts(string firstName, string lastName, string userName, string email)
        {
            Queries queries = new Queries();

            try
            {
                return PartialView("_AccountSearchResults", new LockAccountViewModel 
                { 
                    UserCollection = queries.SearchUsers(firstName: firstName, lastName: lastName, userName: userName, email: email) 
                });
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public ActionResult AccountDetails(string id)
        {
            Queries queries = new Queries();
            try
            {
                User user = queries.GetUserById(id);
                if (user == null)
                    throw new Exception("No user with the supplied ID was found");

                return View(user);
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        [HttpPost]
        public JsonResult LockAccount(string id)
        {
            try
            {
                AdministratorFunctions.LockAccount(id);
            }
            catch (Exception e)
            {
                Helpers.ControllerExtensions.RedirectToError(this, e);
                throw e;
            }

            return Json(new { });
        }

        [HttpPost]
        public JsonResult UnlockAccount(string id)
        {
            try
            {
                AdministratorFunctions.UnlockAccount(id);
            }
            catch (Exception e)
            {
                Helpers.ControllerExtensions.RedirectToError(this, e);
                throw e;
            }

            return Json(new { });
        }

        public ActionResult PublishInformation()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> PublishInformation(AdminInformationViewModel model)
        {
            if (ModelState.IsValid)
            {
                await AdministratorFunctions.PublishInformation(model.Title, model.Content, User.Identity.GetUserId());
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}