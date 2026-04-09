using VirtualEngineer.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtualEngineer.UI;

namespace VirtualEngineer.Controllers
{
    public class SceneCardController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private Button selectBtn;
        [SerializeField]
        private Button aboutBtn;
        [SerializeField]
        private Image image;
        private Scene scene;
        private MenusManager menusManager;
        private AboutSceneMenuController aboutScene;

        private void Awake()
        {
            aboutBtn.onClick.AddListener(OnAboutClicked);
        }

        public void Init(Scene scene, MenusManager menusManager, AboutSceneMenuController aboutScene)
        {
            this.scene = scene;
            title.text = scene.title;
            image.sprite = Resources.Load<Sprite>(scene.name + "/1");
            this.menusManager = menusManager;
            this.aboutScene = aboutScene;
        }

        private void OnAboutClicked()
        {
            aboutScene.Init(scene.title, scene.description, image.sprite);
            menusManager.ShowAboutSceneMenu();
        }
    }
}