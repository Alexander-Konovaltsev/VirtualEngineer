using VirtualEngineer.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public void Init(Scene scene)
        {
            this.scene = scene;
            title.text = scene.title;
            image.sprite = Resources.Load<Sprite>(scene.name + "/1");
        }
    }
}