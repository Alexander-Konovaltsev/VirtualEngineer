using TMPro;
using System.Collections.Generic;

namespace VirtualEngineer.Validation
{
    public class InputValidator
    {
        private TMP_InputField inputField;
        private TMP_Text errorText;

        private List<IValidator> rules = new List<IValidator>();

        public InputValidator(TMP_InputField inputField, TMP_Text errorText)
        {
            this.inputField = inputField;
            this.errorText = errorText;
        }

        public void AddRule(IValidator rule)
        {
            rules.Add(rule);
        }

        public bool Validate()
        {
            string value = inputField.text.Trim();

            foreach (var rule in rules)
            {
                if (!rule.Validate(value, out string error))
                {
                    ShowError(error);
                    return false;
                }
            }

            HideError();
            return true;
        }

        private void ShowError(string message)
        {
            errorText.text = message;
            errorText.gameObject.SetActive(true);
        }

        private void HideError()
        {
            errorText.gameObject.SetActive(false);
        }
    }
}