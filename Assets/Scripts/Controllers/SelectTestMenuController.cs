using UnityEngine;
using TMPro;

namespace VirtualEngineer.Controllers
{
    public class SelectTestMenuController : BaseMenuController
    {
        [SerializeField]
        private TMP_Text loadText;
        private Transform pauseMenuTransform;

        private void OnEnable()
        {
            ResizeMenu(transform, 160, 140);
            gameObject.transform.SetPositionAndRotation(pauseMenuTransform.position, pauseMenuTransform.rotation);

            loadText.gameObject.SetActive(true);
        }

        public void Init(Transform pauseMenuTransform)
        {
            this.pauseMenuTransform = pauseMenuTransform;
        }

        public void BackToPauseMenuAction()
        {
            pauseMenuTransform.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}