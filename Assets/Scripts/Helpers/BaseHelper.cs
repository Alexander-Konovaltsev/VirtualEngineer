using TMPro;
using System;
using System.Linq;

namespace VirtualEngineer.Helpers
{
    public static class BaseHelper
    {
        public static void SetText(TMP_Text obj, string value)
        {
            obj.text = value;
            obj.gameObject.SetActive(true);
        }

        public static string ConvertSnakeCaseToPascalCase(string value)
        {
            return string.Concat(value.Split('_', StringSplitOptions.RemoveEmptyEntries).
                                 Select(word => char.ToUpper(word[0]) + word.Substring(1))
        );
        }
    }
}