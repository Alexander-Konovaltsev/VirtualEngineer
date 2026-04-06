using System.Text.RegularExpressions;

namespace VirtualEngineer.Validation.Rules
{
    public class EmailValidator : IValidator
    {
        private readonly Regex emailRegex = new Regex(@"^[^@\s]+@[a-zA-Z]+\.[a-zA-Z]+$");

        public bool Validate(string value, out string error)
        {
            if (!emailRegex.IsMatch(value))
            {
                error = "Некорректный email";
                return false;
            }

            error = null;
            return true;
        }
    }

}