using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.UI;
using TMPro;
using UnityEngine;

namespace VirtualEngineer.Controllers
{
    public class SelectSceneMenuController : BaseMenuController
    {
        private Transform content;
        [SerializeField] 
        private GameObject sceneCardPrefab;
        private TMP_Text loadText;
        private Scene[] scenes;
        private MenusManager menusManager;

        private void Awake()
        {
            content = transform.Find(pathToViewportInSceneCard + "Content");
            loadText = transform.Find(pathToViewportInSceneCard + "LoadText").GetComponent<TMP_Text>();
            menusManager = GetMenusManager();
        }

        private async void OnEnable()
        {
            loadText.gameObject.SetActive(true);

            scenes = await ApiService.GetScenes();

            if (scenes == null)
            {
                menusManager.ShowAuthorizationMenu();
            }

            loadText.gameObject.SetActive(false);
            
            foreach (Scene scene in scenes)
            {
                GameObject sceneObj = Instantiate(sceneCardPrefab, content);

                SceneCardController sceneController = sceneObj.GetComponent<SceneCardController>();
                sceneController.Init(scene);
            }
        }
    }
}