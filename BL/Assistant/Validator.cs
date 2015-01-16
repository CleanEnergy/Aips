using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Assistant
{
    public class Validator
    {
        private ApplicationDbContext context;

        public Validator()
        {
            context = new ApplicationDbContext();
        }

        public Validator(ApplicationDbContext _context)
        {
            context = _context;
        }

        internal ValidationResult MarkLabPresence(string[] idsArray, int labId)
        {
            ValidationResult result = new ValidationResult();


            return result;
        }
    }
}
