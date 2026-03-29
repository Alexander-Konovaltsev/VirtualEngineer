using System.Collections.Generic;
using VirtualEngineer.Enums;
using UnityEngine;
using System;

namespace VirtualEngineer.UI
{
    public class MenusManager : MonoBehaviour
    {
        private GameObject currentMenu;
        private Dictionary<Menu, GameObject> allMenus = new Dictionary<Menu, GameObject>();

        private void Start()
        {
            GetAllMenus();
            ShowMenu(Menu.StartMenu);
        }

        private void GetAllMenus()
        {
            foreach (Menu menu in Enum.GetValues(typeof(Menu)))
            {
                GameObject menuGameObject = transform.Find(menu.ToString())?.gameObject;
                if (menuGameObject != null) allMenus.Add(menu, menuGameObject);
            }
        }

        private void ShowMenu(Menu menu)
        {
            HideAllMenus();
            allMenus[menu].SetActive(true);
            currentMenu = allMenus[menu];
        }

        private void HideAllMenus()
        {
            foreach (GameObject menu in allMenus.Values)
            {
                menu.SetActive(false);
            }
        }
    }
}