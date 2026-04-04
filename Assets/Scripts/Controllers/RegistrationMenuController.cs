using UnityEngine;
using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.UI;
using TMPro;
using System.Linq;

namespace VirtualEngineer.Controllers
{
    public class RegistrationMenuController : MonoBehaviour
    {
        private Dropdown rolesDropdown;
        private string pathToRolesDropdown = "MainContainer/ContainerInput/RolesDropdown";

        private void Awake()
        {
            rolesDropdown = new Dropdown(transform.Find(pathToRolesDropdown).GetComponent<TMP_Dropdown>());
        }

        private async void Start()
        {
            Role[] roles = await ApiService.GetRoles();

            rolesDropdown.SetOptions(roles.Select(r => r.name).ToList());
        }
    }
}
