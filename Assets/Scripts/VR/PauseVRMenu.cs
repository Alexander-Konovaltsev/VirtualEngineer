using UnityEngine;
using UnityEngine.InputSystem;
using VirtualEngineer.Services;

namespace VirtualEngineer.VR
{
    public class PauseVRMenu : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference menuAction;
        [SerializeField]
        private GameObject menu;
        [SerializeField]
        private Transform allMenusParent;
        [SerializeField]
        private MonoBehaviour moveProvider;
        private Transform cameraTransform;
        private bool isMenuOpen = false;

        private void Start()
        {
            cameraTransform = Camera.main.transform;
            menu.SetActive(false);
        }
        
        private void OnEnable()
        {
            menuAction.action.performed += OnMenuPressed;
            menuAction.action.Enable();
        }

        private void OnDisable()
        {
            menuAction.action.performed -= OnMenuPressed;
            menuAction.action.Disable();
        }

        private void OnMenuPressed(InputAction.CallbackContext ctx)
        {
            if (AppDataService.IsTestMode)
                return;

            if (isMenuOpen)
                CloseMenu();
            else
                OpenMenu();
        }

        private void OpenMenu()
        {
            isMenuOpen = true;

            Vector3 forward = cameraTransform.forward;
            Vector3 position = cameraTransform.position + forward * 2f;

            menu.transform.position = position;

            Vector3 lookPos = cameraTransform.position;
            lookPos.y = position.y;
            menu.transform.LookAt(lookPos);
            menu.transform.Rotate(0, 180f, 0);

            menu.SetActive(true);
            LockMovement(true);
        }

        public void CloseMenu()
        {
            isMenuOpen = false;

            foreach (Transform menu in allMenusParent)
            {
                menu.gameObject.SetActive(false);
            }

            LockMovement(false);
        }

        private void LockMovement(bool value)
        {
            moveProvider.enabled = !value;
        }
    }
}