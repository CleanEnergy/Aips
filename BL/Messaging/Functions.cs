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
            return context.UserMessages.Where(x => x.RecepientUsername == userName && !x.IsTrash).OrderByDescending(x => x.SentOn).ToList();
        }

        public List<UserMessage> GetSentMessages(string userName)
        {
            return context.UserMessages.Where(x => x.SenderUsername == userName && !x.IsTrash).OrderByDescending(x => x.SentOn).ToList();
        }

        public List<UserMessage> GetTrashMessages(string userName)
        {
            return context.UserMessages.Where(x => (x.SenderUsername == userName || x.RecepientUsername == userName) && x.IsTrash == true).OrderByDescending(x => x.SentOn).ToList();
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

        public async Task<UserMessage> GetMessage(int id, bool setSeen = false)
        {
            UserMessage message = context.UserMessages.Single(x => x.UserMessageId == id);

            if (setSeen)
            {
                if (!message.Seen)
                {
                    context.UserMessages.Single(x => x.UserMessageId == id).Seen = true;
                    await context.SaveChangesAsync();
                }
            }
            return message;
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

        public Task<int> GetUnreadMessageCount(string userName)
        {
            return Task.FromResult<int>(
                context.UserMessages.Where(x => x.RecepientUsername == userName && !x.Seen).Count()    
            );
        }
    }
}
