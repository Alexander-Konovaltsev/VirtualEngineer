using UnityEngine;
using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Data;
using System.IO;

namespace VirtualEngineer.Controllers
{
    public class RegistrationMenuController : MonoBehaviour
    {
        private MyDropdown rolesDropdown;
        private Button regBtn;

        private string pathToInputContainer = "MainContainer/ContainerInput/";
        private string pathToRegBtn = "MainContainer/ContainerBtn/ContainerRegBtn/RegBtn";

        private void Awake()
        {
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
                SetValidationText("WorkPlaceValidationText", "Проверьте подключение к интернету");
                return;
            }

            regBtn.interactable = true;
            rolesDropdown.SetOptions(roles.Select(r => r.name).ToList());
            rolesDropdown.Dropdown.interactable = true;
        }

        private void SetValidationText(string validationName, string validationText)
        {
            TMP_Text validation = transform.Find(pathToInputContainer + validationName).GetComponent<TMP_Text>();
            validation.text = validationText;
            validation.gameObject.SetActive(true);
        }
    }
}
