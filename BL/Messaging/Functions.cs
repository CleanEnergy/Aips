using DAL;
using EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Messaging
{
    public class Functions
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public List<UserMessage> GetReceivedMessages(string userName)
        {
            return context.UserMessages.Where(x => x.RecepientUsername == userName && !x.IsTrash).ToList();
        }

        public List<UserMessage> GetSentMessages(string userName)
        {
            return context.UserMessages.Where(x => x.SenderUsername == userName && !x.IsTrash).ToList();
        }

        public List<UserMessage> GetTrashMessages(string userName)
        {
            return context.UserMessages.Where(x => (x.SenderUsername == userName || x.RecepientUsername == userName) && x.IsTrash == true).ToList();
        }

        public Task SendMessage(string subject, string content, string senderUsername, string[] recepientUsernames)
        {
            for (int i = 0; i < recepientUsernames.Length; i++)
            {
                context.UserMessages.Add(new UserMessage 
                {
                    Subject = subject,
                    Content = content,
                    SenderUsername = senderUsername,
                    RecepientUsername = recepientUsernames[i],
                    SentOn = DateTime.Now
                });
            }
            return context.SaveChangesAsync();
        }

        public Task SendMessage(string subject, string content, string senderUsername, string recepientUsername)
        {
            List<string> recepientInList = new List<string>();
            recepientInList.Add(recepientUsername);
            return SendMessage(subject, content, senderUsername, recepientInList.ToArray());
        }

        public bool ValidateRecepients(string[] recepientUsernames)
        {
            for (int i = 0; i < recepientUsernames.Length; i++)
            {
                // NOTE: indexers are not supported in LINQ to SQL
                string userName = recepientUsernames[i];
                if (!context.Users.Any(x => x.UserName == userName))
                {
                    return false;
                }
            }

            return true;
        }

        public Task<UserMessage> GetMessage(int id)
        {
            return Task.FromResult<UserMessage>(context.UserMessages.Where<UserMessage>(x => x.UserMessageId == id).SingleOrDefault());
        }

        public Task TrashMessage(int id)
        {
            context.UserMessages.Where<UserMessage>(x => x.UserMessageId == id).First().IsTrash = true;
            Task.WaitAll(context.SaveChangesAsync());
            return Task.FromResult<object>(null);
        }

        public Task DeleteMessage(int id)
        {
            context.Set<UserMessage>().Remove(context.UserMessages.Where<UserMessage>(x => x.UserMessageId == id).SingleOrDefault());
            Task.WaitAll(context.SaveChangesAsync());
            return Task.FromResult<object>(null);
        }
    }
}
