using UnityEngine;
using UnityEngine.SceneManagement;
using VirtualEngineer.Enums;
using VirtualEngineer.VR;
using VirtualEngineer.Controllers;
using Unity.XR.CoreUtils;
using Unity.VisualScripting;

namespace VirtualEngineer.Controllers
{
    public class PauseSceneMenuController: MonoBehaviour
    {
        [SerializeField]
        private PauseVRMenu pauseMenu;
        [SerializeField]
        private GameObject statsMenu;
        StatsMenuController statsMenuController;

        public void Start()
        {
            statsMenuController = statsMenu.GetComponent<StatsMenuController>();
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
            statsMenuController.Init(gameObject.transform);
            statsMenu.SetActive(true);
        }
    }
}