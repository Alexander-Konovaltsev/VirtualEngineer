using TMPro;

namespace VirtualEngineer.Helpers
{
    public static class BaseHelper
    {
        public static void SetText(TMP_Text obj, string value)
        {
            obj.text = value;
            obj.gameObject.SetActive(true);
        }
    }
}