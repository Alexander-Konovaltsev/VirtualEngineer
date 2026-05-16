using TMPro;
using UnityEngine;
using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.Enums;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VirtualEngineer.Validation;
using System.Threading.Tasks;

namespace VirtualEngineer.Controllers
{
    public class StatsMenuController : BaseMenuController
    {
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private TMP_Text descripiton;
        [SerializeField]
        private TMP_Text loadText;
        private Model[] allModels;
        private UserModelView[] userViewedModels;
        private Transform pauseMenuTransform;

        private void Awake()
        {
            title.text = "Прогресс";
        }

        private async void OnEnable()
        {
            ResizeMenu(transform);

            descripiton.text = "";
            gameObject.transform.SetPositionAndRotation(pauseMenuTransform.position, pauseMenuTransform.rotation);

            loadText.gameObject.SetActive(true);

            if (!await LoadSceneStats())
            {
                return;
            }

            loadText.gameObject.SetActive(false);

            SetStatsText();
        }

        public void Init(Transform pauseMenuTransform)
        {
            this.pauseMenuTransform = pauseMenuTransform;
        }

        private async Task<bool> LoadSceneStats()
        {
            ApiResponse<UserModelView[]> getUserViewedModelsResponse = 
                await ApiService.GetAsync<UserModelView>(
                    Endpoint.AllViewedModelsByScene((int)AppDataService.SelectedSceneId)
                );
            
            if (!ResponseValidator.CheckResponseSuccess(getUserViewedModelsResponse))
            {
                return false;
            }

            ApiResponse<Model[]> getAllModelsResponse = 
                await ApiService.GetAsync<Model>(
                    Endpoint.AllModelsByScene((int)AppDataService.SelectedSceneId)
                );
            
            if (!ResponseValidator.CheckResponseSuccess(getAllModelsResponse))
            {
                return false;
            }

            userViewedModels = getUserViewedModelsResponse.data;
            allModels = getAllModelsResponse.data;

            return true;
        }

        private void SetStatsText()
        {
            int allModelsCount = allModels.Count(m => m.is_informational);

            var viewedModelIds = new HashSet<int>(
                userViewedModels.Select(v => v.model_id)
            );

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"<b>Изучено объектов:</b> {viewedModelIds.Count}/{allModelsCount}");

            foreach (var model in allModels)
            {
                if (!model.is_informational)
                    continue;

                bool isViewed = viewedModelIds.Contains(model.id);

                string prefix = isViewed
                    ? "<color=green><b>+</b></color>"
                    : "<color=red><b>-</b></color>";

                sb.AppendLine($"{prefix} {model.title}");
            }

            descripiton.text = sb.ToString();
        }

        public void BackToPauseMenuAction()
        {
            pauseMenuTransform.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}