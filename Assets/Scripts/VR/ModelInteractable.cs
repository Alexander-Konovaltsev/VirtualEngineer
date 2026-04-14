using UnityEngine;
using VirtualEngineer.Controllers;
using VirtualEngineer.Services;
using VirtualEngineer.Enums;
using UnityEngine.XR.Interaction.Toolkit;
using VirtualEngineer.Models;
using System.Threading.Tasks;

namespace VirtualEngineer.VR
{
    public class ModelInteractable : MonoBehaviour
    {
        private GameObject card;
        private InfoModelCardController cardController;
        private Renderer[] renderers;
        private void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>();
        }
        
        public void Init(GameObject card)
        {
            this.card = card;
            cardController = card.GetComponent<InfoModelCardController>();
        }

        public void OnSelect()
        {
            if (card == null) return;

            if (!cardController.isViewed)
                CreateModelView();

            card.SetActive(!card.activeSelf);
        }

        public void OnHoverEnter()
        {
            Highlight(true);
        }

        public void OnHoverExit()
        {
            Highlight(false);
        }

        private void Highlight(bool value)
        {
            foreach (var renderer in renderers)
            {
                foreach (var mat in renderer.materials)
                {
                    if (value)
                    {
                        mat.EnableKeyword("_EMISSION");
                        mat.SetColor("_EmissionColor", new Color(1f, 0.8f, 0.3f) * 0.3f);
                    }
                    else
                    {
                        mat.SetColor("_EmissionColor", Color.black);
                        mat.DisableKeyword("_EMISSION");
                    }
                }
            }
        }

        private async void CreateModelView()
        { 
            UserModelViewCreateRequest userModelView = new UserModelViewCreateRequest
            {
                scene_id=(int)AppDataService.SelectedSceneId,
                model_id=cardController.ModelId
            };

            UserModelViewCreateResult result = await ApiService.CreateUserModelView(userModelView);
            if (result == UserModelViewCreateResult.Success)
                cardController.isViewed = true;

            Debug.Log($"Изучено: {cardController.ModelId}"); 
        }
    }
}
