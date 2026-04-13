using UnityEngine;
using UnityEngine.InputSystem;

namespace VirtualEngineer.VR
{
    public class PauseVRMenu : MonoBehaviour
    {
        public InputActionReference menuAction;

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
            Debug.Log("Menu button pressed!");
        }
    }
}