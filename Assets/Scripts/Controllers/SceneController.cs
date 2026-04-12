using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.Enums;
using VirtualEngineer.VR;
using System.Linq;
using System.Collections.Generic;

namespace VirtualEngineer.Controllers
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] 
        private GameObject infoModelCardPrefab;

        private async void Awake()
        {
            Model[] allModels = await ApiService.GetAsyncPrivate<Model>(Endpoint.AllModelsByScene(AppDataService.SelectedSceneId));

            Dictionary<int?, List<Model>> tree = BuildModelsTree(allModels.ToList());

            RecursiveDescentModelsTree(tree, ConstCode.ModelWithoutParent, transform);
        }
        
        private Dictionary<int?, List<Model>> BuildModelsTree(List<Model> models)
        {
            Dictionary<int?, List<Model>> tree = new Dictionary<int?, List<Model>>();

            foreach (Model model in models)
            {
                if (model.parent_id == null) model.parent_id = ConstCode.ModelWithoutParent;
                
                if (!tree.ContainsKey(model.parent_id))
                    tree[model.parent_id] = new List<Model>();

                tree[model.parent_id].Add(model);
            }

            return tree;
        }

        private void RecursiveDescentModelsTree(Dictionary<int?, List<Model>> tree, int? parentId, Transform parentTransform)
        {
            if (!tree.ContainsKey(parentId))
                return;

            foreach (Model model in tree[parentId])
            {
                Transform currentTransform = parentTransform.Find(model.name);

                if (currentTransform == null)
                    continue;

                SelectActions(model, currentTransform);

                RecursiveDescentModelsTree(tree, model.id, currentTransform);
            }
        }
        
        private void SelectActions(Model model, Transform transform)
        {
            if (model.is_informational)
                CreateInfoModelCard(model, transform);
        }

        private void CreateInfoModelCard(Model model, Transform transform)
        {
            GameObject card = Instantiate(infoModelCardPrefab);
            card.SetActive(false);

            Vector3 cardPos = transform.position + Vector3.up * 1f;
            card.transform.SetPositionAndRotation(cardPos, Quaternion.identity);

            InfoModelCardController cardController = card.GetComponent<InfoModelCardController>();
            cardController.Init(model.title, model.description);

            InitXR(transform, card);
        }

        private void InitXR(Transform transform, GameObject card)
        {
            Collider collider = transform.GetComponent<Collider>();
            if (collider == null)
                collider = transform.gameObject.AddComponent<BoxCollider>();
            
            XRSimpleInteractable interactable = transform.GetComponent<XRSimpleInteractable>();
            if (interactable == null)
                interactable = transform.gameObject.AddComponent<XRSimpleInteractable>();

            ModelInteractable modelController = transform.GetComponent<ModelInteractable>();
            if (modelController == null)
                modelController = transform.gameObject.AddComponent<ModelInteractable>();
            modelController.Init(card);

            interactable.selectEntered.AddListener(_ => modelController.OnSelect());
            interactable.hoverEntered.AddListener(_ => modelController.OnHoverEnter());
            interactable.hoverExited.AddListener(_ => modelController.OnHoverExit());
        }
    }
}