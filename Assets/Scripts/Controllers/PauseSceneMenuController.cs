using UnityEngine;
using UnityEngine.SceneManagement;
using VirtualEngineer.Enums;
using VirtualEngineer.VR;
using VirtualEngineer.Controllers;
using Unity.XR.CoreUtils;
using Unity.VisualScripting;

namespace VirtualEngineer.Controllers
{
    public class PauseSceneMenuController: BaseMenuController
    {
        [SerializeField]
        private PauseVRMenu pauseMenu;
        [SerializeField]
        private GameObject statsMenu;
        [SerializeField]
        private GameObject selectTestMenu;
        StatsMenuController statsMenuController;
        SelectTestMenuController selectTestMenuController;

        private void Start()
        {
            statsMenuController = statsMenu.GetComponent<StatsMenuController>();
            selectTestMenuController = selectTestMenu.GetComponent<SelectTestMenuController>();
        }

        private void OnEnable()
        {
            ResizeMenu(transform);
        }

        public void ResumeAction()
        {
            pauseMenu.CloseMenu();
        }
        
        public void BackMenuAction()
        {
            SceneManager.LoadScene(ConstCode.StartMenuScene);
        }

        public void ShowStatsMenuAction()
        {   
            gameObject.SetActive(false);

            statsMenuController.Init(transform);
            statsMenu.SetActive(true);
        }

        public void ShowTestsMenuAction()
        {
            gameObject.SetActive(false);

            selectTestMenuController.Init(transform);
            selectTestMenu.SetActive(true);
        }
    }
}