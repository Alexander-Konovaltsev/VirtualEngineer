using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace VirtualEngineer.Controllers
{
    public class AboutSceneMenuController : BaseMenuController
    {
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private TMP_Text descripiton;
        [SerializeField]
        private Image image;

        public void Init(string title, string descripiton, Sprite image)
        {
            this.title.text = title;
            this.descripiton.text = descripiton;
            this.image.sprite = image;
        }
    }
}