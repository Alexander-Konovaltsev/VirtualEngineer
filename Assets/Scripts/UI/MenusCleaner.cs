using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtualEngineer.Enums;
using UnityEngine.EventSystems;

namespace VirtualEngineer.UI
{
    public static class MenusCleaner
    {
        private static string commonContainerInputPrefix = "MainContainer/ContainerInput/";

        public static void ClearMenu(Menu menu, GameObject menuGO)
        {
            switch (menu)
            {
                case Menu.AboutMenu:
                    ClearAboutMenu(menuGO);
                    break;
                case Menu.AuthorizationMenu:
                    ClearAuthorizationMenu(menuGO);
                    break;
                case Menu.RegistrationMenu:
                    ClearRegistrationMenu(menuGO);
                    break;
            }
        }

        private static void ClearAboutMenu(GameObject aboutMenu)
        {
            Transform scrollTransform = aboutMenu.transform.Find("MainContainer/TextBg/Scroll View");
            if (scrollTransform == null) return;

            ScrollRect scrollRect = scrollTransform.GetComponent<ScrollRect>();
            scrollRect.verticalNormalizedPosition = 1f;
        }

        private static void ClearAuthorizationMenu(GameObject authorizationMenu)
        {
            string[] inputsNames = new string[] {"EmailInput", "PasswordInput"};
            ClearMenuInputs(authorizationMenu, inputsNames);

            string[] validationsNames = new string[] {"EmailValidationText", "PasswordValidationText"};
            ClearMenuValidations(authorizationMenu, validationsNames);
        }

        private static void ClearRegistrationMenu(GameObject registrationMenu)
        {
            string[] inputsNames = new string[] {"LastNameInput", "FirstNameInput", "PatronymicInput", 
                                                 "EmailInput", "PasswordInput", "WorkPlaceInput"};
            ClearMenuInputs(registrationMenu, inputsNames);

            string[] validationsNames = new string[] {"LastNameValidationText", "FirstNameValidationText", "EmailValidationText",
                                                      "PasswordValidationText", "WorkPlaceValidationText"};
            ClearMenuValidations(registrationMenu, validationsNames);

            Transform dropdownTransform = registrationMenu.transform.Find(commonContainerInputPrefix + "RolesDropdown");
            if (dropdownTransform == null) return;

            TMP_Dropdown dropdown = dropdownTransform.GetComponent<TMP_Dropdown>();
            dropdown.ClearOptions();
        }

        private static void ClearMenuInputs(GameObject menu, string[] inputsNames)
        {
            foreach (string inputName in inputsNames)
            {
                Transform inputTransform = menu.transform.Find(commonContainerInputPrefix + inputName);
                if (inputTransform == null) continue;

                TMP_InputField input = inputTransform.GetComponent<TMP_InputField>();
                input.text = "";
                input.DeactivateInputField();
            }

            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);
        }

        private static void ClearMenuValidations(GameObject menu, string[] validationsNames)
        {
            foreach (string validationName in validationsNames)
            {
                Transform validationTransform = menu.transform.Find(commonContainerInputPrefix + validationName);
                if (validationTransform == null) continue;

                validationTransform.gameObject.SetActive(false);
            }
        }
    }
}