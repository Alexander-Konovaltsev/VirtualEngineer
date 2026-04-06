namespace VirtualEngineer.Validation.Rules
{
    public class RequiredValidator : IValidator
    {
        private string fieldName;

        public RequiredValidator(string fieldName)
        {
            this.fieldName = fieldName;
        }

        public bool Validate(string value, out string error)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                error = $"Необходимо заполнить «{fieldName}»";
                return false;
            }

            error = null;
            return true;
        }
    }

}