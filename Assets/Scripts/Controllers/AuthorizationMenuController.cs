using VirtualEngineer.Validation;
using VirtualEngineer.Validation.Rules;
using VirtualEngineer.Models;
using VirtualEngineer.Enums;
using VirtualEngineer.Services;
using VirtualEngineer.Helpers;
using VirtualEngineer.UI;
using TMPro;
using UnityEngine.UI;
using System.Net;

namespace VirtualEngineer.Controllers
{
    public class AuthorizationMenuController : BaseMenuController
    {
        private Button authBtn;
        
        private InputValidator emailInputValidator;
        private InputValidator passwordInputValidator;

        private MenusManager menusManager;

        private void Awake()
        {
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

            authBtn = transform.Find(pathToBtnContainer + "ContainerAuthBtn/AuthBtn").GetComponent<Button>();
            menusManager = GetMenusManager();
        }

        private void OnEnable()
        {
            authBtn.GetComponentInChildren<TMP_Text>().text = "Войти";
            authBtn.interactable = true;
        }

        public async void AuthorizationAction()
        {
            bool isValidForm =
                emailInputValidator.Validate() &
                passwordInputValidator.Validate();

            if (!isValidForm) return;

            UserAuthorizationRequest auth = new UserAuthorizationRequest
            {
                email = emailInputValidator.InputField.text.Trim(),
                password = passwordInputValidator.InputField.text,
            };

            TMP_Text authBtnText = authBtn.GetComponentInChildren<TMP_Text>();
            authBtn.interactable = false;
            authBtnText.text = "Обработка...";

            ApiResponse<UserAuthorizationResponse> authorizationResponse = 
                await ApiService.PostAsync<UserAuthorizationRequest, UserAuthorizationResponse>(
                    Endpoint.UserAuthorization, 
                    auth, 
                    false
                );

            switch (authorizationResponse.statusCode)
            {
                case HttpStatusCode.OK:
                    SessionService.SetToken(authorizationResponse.data.access_token);

                    menusManager.ShowSelectSceneMenu();

                    break;

                case HttpStatusCode.Unauthorized:
                authBtnText.text = "Войти";
                authBtn.interactable = true;
                    BaseHelper.SetText(
                        passwordInputValidator.ErrorText, 
                        "Неправильный email или пароль"
                    );

                    break;

                case HttpStatusCode.RequestTimeout:
                    authBtnText.text = "Ошибка";
                    BaseHelper.SetText(
                        passwordInputValidator.ErrorText, 
                        "Проверьте подключение к интернету"
                    );

                    break;

                case HttpStatusCode.UnprocessableEntity:
                    authBtnText.text = "Войти";
                    authBtn.interactable = true;
                    BaseHelper.SetText(
                        passwordInputValidator.ErrorText, 
                        "Проверьте корректность данных"
                    );

                    break;

                default:
                    authBtnText.text = "Ошибка";
                    BaseHelper.SetText(
                        passwordInputValidator.ErrorText, 
                        "Ошибка"
                    );

                    break;
            }
        }
    }
}