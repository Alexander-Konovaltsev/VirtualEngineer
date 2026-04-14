using VirtualEngineer.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtualEngineer.UI;
using VirtualEngineer.Helpers;
using UnityEngine.SceneManagement;
using VirtualEngineer.Services;
using VirtualEngineer.Enums;

namespace VirtualEngineer.Controllers
{
    public class SceneCardController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private TMP_Text studyObjectsInfo;
        [SerializeField]
        private Button selectBtn;
        [SerializeField]
        private Button aboutBtn;
        [SerializeField]
        private Image image;
        private Models.Scene scene;
        private MenusManager menusManager;
        private AboutSceneMenuController aboutScene;
        private int objectsCount;
        private int studyObjectsCount;

        private void Awake()
        {
            selectBtn.onClick.AddListener(OnSelectClicked);
            aboutBtn.onClick.AddListener(OnAboutClicked);
        }

        public void Init(Models.Scene scene, MenusManager menusManager, AboutSceneMenuController aboutScene)
        {
            this.scene = scene;
            title.text = scene.title;
            image.sprite = Resources.Load<Sprite>(scene.name + "/1");
            this.menusManager = menusManager;
            this.aboutScene = aboutScene;
            InitStudyObjectsInfo();
        }

        private void OnAboutClicked()
        {
            aboutScene.Init(scene.title, scene.description, image.sprite);
            menusManager.ShowAboutSceneMenu();
        }

        private void OnSelectClicked()
        {
            AppDataService.SelectedSceneId = scene.id;
            SceneManager.LoadScene(scene.name);
        }

        private async void InitStudyObjectsInfo()
        {
            UserModelView[] userViewedModels = await ApiService.GetAsyncPrivate<UserModelView>(Endpoint.AllViewedModelsByScene(scene.id));
            Model[] allModels = await ApiService.GetAsyncPrivate<Model>(Endpoint.AllModelsByScene(scene.id));

            studyObjectsCount = userViewedModels.Length;
            foreach (Model model in allModels)
            {
                if (model.is_informational) objectsCount++;
            }

            studyObjectsInfo.text = $"Изучено: {studyObjectsCount}/{objectsCount}";
        }
    }
}