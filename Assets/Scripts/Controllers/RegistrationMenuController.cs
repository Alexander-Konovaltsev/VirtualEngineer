using UnityEngine;
using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.UI;
using VirtualEngineer.Validation;
using VirtualEngineer.Validation.Rules;
using VirtualEngineer.Helpers;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Data;

namespace VirtualEngineer.Controllers
{
    public class RegistrationMenuController : MonoBehaviour
    {
        private MyDropdown rolesDropdown;
        private Button regBtn;

        private InputValidator lastNameInputValidator;
        private InputValidator firstNameInputValidator;
        private InputValidator emailInputValidator;
        private InputValidator passwordInputValidator;
        private InputValidator workplaceInputValidator;

        private string pathToInputContainer = "MainContainer/ContainerInput/";
        private string pathToRegBtn = "MainContainer/ContainerBtn/ContainerRegBtn/RegBtn";

        private void Awake()
        {
            lastNameInputValidator = new InputValidator(
                transform.Find(pathToInputContainer + "LastNameInput").GetComponent<TMP_InputField>(),
                transform.Find(pathToInputContainer + "LastNameValidationText").GetComponent<TMP_Text>()
            );
            lastNameInputValidator.AddRule(new RequiredValidator("Фамилия"));

            firstNameInputValidator = new InputValidator(
                transform.Find(pathToInputContainer + "FirstNameInput").GetComponent<TMP_InputField>(),
                transform.Find(pathToInputContainer + "FirstNameValidationText").GetComponent<TMP_Text>()
            );
            firstNameInputValidator.AddRule(new RequiredValidator("Имя"));

            emailInputValidator = new InputValidator(
                transform.Find(pathToInputContainer + "EmailInput").GetComponent<TMP_InputField>(),
                transform.Find(pathToInputContainer + "EmailValidationText").GetComponent<TMP_Text>()
            );
            emailInputValidator.AddRule(new RequiredValidator("Email"));
            emailInputValidator.AddRule(new EmailValidator());

            passwordInputValidator = new InputValidator(
                transform.Find(pathToInputContainer + "PasswordInput").GetComponent<TMP_InputField>(),
                transform.Find(pathToInputContainer + "PasswordValidationText").GetComponent<TMP_Text>()
            );
            passwordInputValidator.AddRule(new RequiredValidator("Пароль"));

            workplaceInputValidator = new InputValidator(
                transform.Find(pathToInputContainer + "WorkPlaceInput").GetComponent<TMP_InputField>(),
                transform.Find(pathToInputContainer + "WorkPlaceValidationText").GetComponent<TMP_Text>()
            );
            workplaceInputValidator.AddRule(new RequiredValidator("Место работы"));

            rolesDropdown = new MyDropdown(transform.Find(pathToInputContainer + "RolesDropdown").GetComponent<TMP_Dropdown>());
            regBtn = transform.Find(pathToRegBtn).GetComponent<Button>();
        }

        private async void OnEnable()
        {
            regBtn.interactable = false;
            rolesDropdown.SetOptions(new List<string> {"Загрузка..."});
            rolesDropdown.Dropdown.interactable = false;

            Role[] roles = await ApiService.GetRoles();

            if (roles == null)
            {
                rolesDropdown.SetOptions(new List<string> {"Ошибка"});
                BaseHelper.SetText(
                    transform.Find(pathToInputContainer + "WorkPlaceValidationText").GetComponent<TMP_Text>(), 
                    "Проверьте подключение к интернету");
                
                return;
            }

            regBtn.interactable = true;
            rolesDropdown.SetOptions(roles.Select(r => r.name).ToList());
            rolesDropdown.Dropdown.interactable = true;
        }

        public void RegistrationAction()
        {
            bool isValidForm =
                lastNameInputValidator.Validate() &
                firstNameInputValidator.Validate() &
                emailInputValidator.Validate() &
                passwordInputValidator.Validate() &
                workplaceInputValidator.Validate();

            if (isValidForm)
            {
                Debug.Log("Форма валидна");
            }
        }
    }
}
