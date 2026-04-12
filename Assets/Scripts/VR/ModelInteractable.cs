using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VirtualEngineer.VR
{
    public class ModelInteractable : MonoBehaviour
    {
        private GameObject card;
        private Renderer[] renderers;
        private void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>();
        }
        
        public void Init(GameObject card)
        {
            this.card = card;
        }

        public void OnSelect()
        {
            if (card == null) return;

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
    }
}
