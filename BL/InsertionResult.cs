using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ValidationResult
    {
        public List<KeyValuePair<string, string>> ValidationMessages { get; private set; }

        public bool Succeded { get { return ValidationMessages.Count == 0; } }

        public ValidationResult()
        {
            ValidationMessages = new List<KeyValuePair<string, string>>();
        }

        public void AddError(string property, string message)
        {
            ValidationMessages.Add(new KeyValuePair<string,string>(property, message));
        }
    }
}
