using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.UI;
using VirtualEngineer.Validation;
using VirtualEngineer.Validation.Rules;
using VirtualEngineer.Helpers;
using VirtualEngineer.Enums;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Data;

namespace VirtualEngineer.Controllers
{
    public class RegistrationMenuController : BaseMenuController
    {
        private MyDropdown rolesDropdown;
        private Button regBtn;
        private Role[] roles;

        private InputValidator lastNameInputValidator;
        private InputValidator firstNameInputValidator;
        private InputValidator emailInputValidator;
        private InputValidator passwordInputValidator;
        private InputValidator workplaceInputValidator;

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
            regBtn = transform.Find(pathToBtnContainer + "ContainerRegBtn/RegBtn").GetComponent<Button>();
        }

        private async void OnEnable()
        {
            regBtn.GetComponentInChildren<TMP_Text>().text = "Зарегистрироваться";
            regBtn.interactable = false;
            rolesDropdown.SetOptions(new List<string> {"Загрузка..."});
            rolesDropdown.Dropdown.interactable = false;

            roles = await ApiService.GetRoles();

            if (roles == null)
            {
                rolesDropdown.SetOptions(new List<string> {"Ошибка"});
                BaseHelper.SetText(
                    workplaceInputValidator.ErrorText, 
                    "Проверьте подключение к интернету"
                );
                
                return;
            }

            regBtn.interactable = true;
            rolesDropdown.SetOptions(roles.Select(r => r.name).ToList());
            rolesDropdown.Dropdown.interactable = true;
        }

        public async void RegistrationAction()
        {
            bool isValidForm =
                lastNameInputValidator.Validate() &
                firstNameInputValidator.Validate() &
                emailInputValidator.Validate() &
                passwordInputValidator.Validate() &
                workplaceInputValidator.Validate();

            if (!isValidForm) return;

            UserCreateRequest user = new UserCreateRequest
            {
                last_name = lastNameInputValidator.InputField.text.Trim(),
                first_name = firstNameInputValidator.InputField.text.Trim(),
                patronymic = transform.Find(pathToInputContainer + "PatronymicInput").GetComponent<TMP_InputField>().text.Trim(),
                email = emailInputValidator.InputField.text.Trim(),
                password = passwordInputValidator.InputField.text,
                role_id = roles.First(r => r.name == rolesDropdown.GetSelectOption()).id,
                workplace = workplaceInputValidator.InputField.text.Trim(),
            };

            TMP_Text regBtnText = regBtn.GetComponentInChildren<TMP_Text>();
            regBtn.interactable = false;
            regBtnText.text = "Обработка...";
            
            UserCreateResult createResult = await ApiService.CreateUser(user);
            
            switch (createResult)
            {
                case UserCreateResult.Success:
                    regBtnText.text = "Успешно";
                    break;
                case UserCreateResult.EmailAlreadyExists:
                    regBtnText.text = "Зарегистрироваться";
                    regBtn.interactable = true;
                    BaseHelper.SetText(
                        emailInputValidator.ErrorText, 
                        "Email уже используется"
                    );
                    break;
                case UserCreateResult.DataError:
                    regBtnText.text = "Зарегистрироваться";
                    regBtn.interactable = true;
                    BaseHelper.SetText(
                        workplaceInputValidator.ErrorText, 
                        "Проверьте корректность данных"
                    );
                    break;
                default:
                    regBtnText.text = "Ошибка";
                    BaseHelper.SetText(
                        workplaceInputValidator.ErrorText, 
                        "Проверьте подключение к интернету"
                    );
                    break;
            }
        }
    }
}
