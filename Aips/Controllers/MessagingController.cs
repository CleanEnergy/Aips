using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Mvc;
using BL.Messaging;
using System.Threading.Tasks;
using EntityClasses;

namespace Aips.Controllers
{
    [Authorize]
    public class MessagingController : Controller
    {
        private Functions MessagingFunctions;

        public MessagingController()
        {
            MessagingFunctions = new Functions();
        }

        public ActionResult Index()
        {
            ViewBag.Messages = MessagingFunctions.GetReceivedMessages(User.Identity.Name);
            return View("Inbox");
        }

        public ActionResult SentItems()
        {
            ViewBag.Messages = MessagingFunctions.GetSentMessages(User.Identity.Name);
            return View();
        }

        public ActionResult Trash()
        {
            ViewBag.Messages = MessagingFunctions.GetTrashMessages(User.Identity.Name);
            return View();
        }

        public ActionResult NewMessage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewMessage(MessageViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string[] recepientUsernames = model.RecepientUsernames.Split(';');
                    if (MessagingFunctions.ValidateRecepients(recepientUsernames))
                    {
                        await MessagingFunctions.SendMessage(model.Subject, model.Content, User.Identity.Name, recepientUsernames);
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

        public async Task<ActionResult> ReadMessage(int id)
        {
            try
            {
                UserMessage userMessage = await MessagingFunctions.GetMessage(id);

                return View(new MessageViewModel 
                {
                    Content = userMessage.Content,
                    RecepientUsername = userMessage.RecepientUsername,
                    SenderUsername = userMessage.SenderUsername,
                    SentOn = userMessage.SentOn,
                    Subject = userMessage.Subject
                });
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> TrashMessage(int id)
        {
            try
            {
                await MessagingFunctions.TrashMessage(id);
                return RedirectToAction("Trash");
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);
            }
        }

        public async Task<ActionResult> DeleteMessage(int id)
        {
            try
            {
                await MessagingFunctions.DeleteMessage(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return Helpers.ControllerExtensions.RedirectToError(this, e);   
            }
        }
    }
}