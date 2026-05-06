using VirtualEngineer.UI;
using UnityEngine;

namespace VirtualEngineer.Controllers
{
    public class BaseMenuController : MonoBehaviour
    {
        protected string pathToInputContainer = "MainContainer/ContainerInput/";
        protected string pathToBtnContainer = "MainContainer/ContainerBtn/";
        protected string pathToViewportInSceneCard = "MainContainer/TextBg/ScrollView/Viewport/";

        protected MenusManager GetMenusManager()
        {
            return GetComponentInParent<MenusManager>();
        }

        protected void ResizeMenu(Transform menuTransform, int width=100, int height=100)
        {
            Transform canvas = menuTransform.parent;
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(width, height);

            Transform panel = menuTransform.GetChild(0);
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.sizeDelta = new Vector2(width, height);
        }
    }
}