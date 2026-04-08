using TMPro;
using UnityEngine;

namespace VirtualEngineer.Controllers
{
    public class SelectSceneMenuController : BaseMenuController
    {
        private Transform content;
        private GameObject sceneCard;
        private TMP_Text loadText;

        private void Awake()
        {
            content = transform.Find(pathToViewportInSceneCard + "Content");
            loadText = transform.Find(pathToViewportInSceneCard + "LoadText").GetComponent<TMP_Text>();
        }
    }
}