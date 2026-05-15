using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.UI;
using TMPro;
using UnityEngine;
using VirtualEngineer.Enums;
using VirtualEngineer.Validation;

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
        private AboutSceneMenuController aboutScene;

        private void Awake()
        {
            content = transform.Find(pathToViewportInSceneCard + "Content");
            loadText = transform.Find(pathToViewportInSceneCard + "LoadText").GetComponent<TMP_Text>();
            menusManager = GetMenusManager();
            aboutScene = transform.parent.Find("AboutSceneMenu").GetComponent<AboutSceneMenuController>();
        }

        private async void OnEnable()
        {
            loadText.gameObject.SetActive(true);

            ApiResponse<Scene[]> getScenesResponse = await ApiService.GetAsync<Scene>(Endpoint.Scenes);

            if (!ResponseValidator.CheckResponseSuccess(getScenesResponse))
            {
                return;
            }

            scenes = getScenesResponse.data;

            loadText.gameObject.SetActive(false);
            
            foreach (Scene scene in scenes)
            {
                GameObject sceneObj = Instantiate(sceneCardPrefab, content);

                SceneCardController sceneController = sceneObj.GetComponent<SceneCardController>();
                sceneController.Init(scene, menusManager, aboutScene);
            }
        }
    }
}