using UnityEngine;
using UnityEngine.SceneManagement;
using VirtualEngineer.Enums;
using VirtualEngineer.VR;

namespace VirtualEngineer.Controllers
{
    public class PauseSceneMenuController: MonoBehaviour
    {
        [SerializeField]
        private PauseVRMenu pauseMenu;
        
        public void ResumeAction()
        {
            pauseMenu.CloseMenu();
        }
        
        public void BackMenuAction()
        {
            SceneManager.LoadScene(ConstCode.StartMenuScene);
        }
    }
}