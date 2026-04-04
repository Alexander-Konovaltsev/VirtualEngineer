using System.Collections.Generic;
using TMPro;

namespace VirtualEngineer.UI
{
    public class Dropdown
    {
        private TMP_Dropdown dropdown;
        private List<string> options;

        public Dropdown(TMP_Dropdown dropdown)
        {
            this.dropdown = dropdown;
        }

        public void SetOptions(List<string> options)
        {
            this.options = options;

            dropdown.ClearOptions();
            dropdown.AddOptions(options);
            dropdown.value = 0;
            dropdown.RefreshShownValue();
        }

        public string GetSelectOption()
        {
            return options[dropdown.value];
        }
    }
}