using UnityEngine;
using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.Enums;
using System.Threading.Tasks;

namespace VirtualEngineer.Controllers
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] 
        private GameObject infoModelCardPrefab;

        private async void Awake()
        {
            Model[] models = await ApiService.GetAsyncPrivate<Model>(Endpoint.ModelsByScene(AppDataService.SelectedSceneId));

            foreach (Model model in models)
            {
                await RecursiveDescentModels(model, transform);
            }
        }
        
        private async Task RecursiveDescentModels(Model model, Transform parentTransform)
        {
            Transform currentTransform = parentTransform.Find(model.name);

            if (currentTransform == null) 
                return;

            SelectActions(model, currentTransform);

            Model[] modelChildren = await ApiService.GetAsyncPrivate<Model>(Endpoint.ModelChildren(model.id)); 

            foreach (Model modelChild in modelChildren)
            {
                await RecursiveDescentModels(modelChild, currentTransform);
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

            Vector3 cardPos = transform.position + Vector3.up * 1f;
            card.transform.SetPositionAndRotation(cardPos, Quaternion.identity);

            InfoModelCardController cardController = card.GetComponent<InfoModelCardController>();
            cardController.Init(model.title, model.description);
        }
    }
}