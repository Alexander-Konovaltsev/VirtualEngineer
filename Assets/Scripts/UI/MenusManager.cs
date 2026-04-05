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
            ShowStartMenu();
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

        public void ClickedExitBtn()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public void ShowStartMenu()
        {
            ResizeMenu(Menu.StartMenu);
            ShowMenu(Menu.StartMenu);
        }

        public void ShowAboutMenu()
        {
            MenusCleaner.ClearMenu(Menu.AboutMenu, allMenus[Menu.AboutMenu]);
            ResizeMenu(Menu.AboutMenu);
            ShowMenu(Menu.AboutMenu);
        }

        public void ShowAuthorizationMenu()
        {
            MenusCleaner.ClearMenu(Menu.AuthorizationMenu, allMenus[Menu.AuthorizationMenu]);
            ResizeMenu(Menu.AuthorizationMenu);
            ShowMenu(Menu.AuthorizationMenu);
        }

        public void ShowRegistrationMenu()
        {
            MenusCleaner.ClearMenu(Menu.RegistrationMenu, allMenus[Menu.RegistrationMenu]);
            ResizeMenu(Menu.RegistrationMenu, height: 173);
            ShowMenu(Menu.RegistrationMenu);
        }

        private void ResizeMenu(Menu menu, int width=100, int height=100)
        {
            RectTransform canvasRect = GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(width, height);

            Transform menuTransform = allMenus[menu].transform;
            if (menuTransform == null) return;

            Transform menuContainerTransform = menuTransform.Find("MainContainer");
            if (menuContainerTransform == null) return;

            RectTransform menuContainerRect = menuContainerTransform.GetComponent<RectTransform>();
            menuContainerRect.sizeDelta = new Vector2(width, height);
        }
    }
}