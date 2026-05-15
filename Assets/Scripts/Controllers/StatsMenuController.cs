using TMPro;
using UnityEngine;
using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.Enums;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VirtualEngineer.Validation;

namespace VirtualEngineer.Controllers
{
    public class StatsMenuController : BaseMenuController
    {
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private TMP_Text descripiton;
        private Model[] allModels;
        private UserModelView[] userViewedModels;
        private Transform pauseMenuTransform;

        private void Awake()
        {
            title.text = "Прогресс";
        }

        private void OnEnable()
        {
            ResizeMenu(transform);

            descripiton.text = "";
            gameObject.transform.SetPositionAndRotation(pauseMenuTransform.position, pauseMenuTransform.rotation);

            LoadSceneStats();
        }

        public void Init(Transform pauseMenuTransform)
        {
            this.pauseMenuTransform = pauseMenuTransform;
        }

        private async void LoadSceneStats()
        {
            ApiResponse<UserModelView[]> getUserViewedModelsResponse = 
                await ApiService.GetAsync<UserModelView>(
                    Endpoint.AllViewedModelsByScene((int)AppDataService.SelectedSceneId)
                );
            
            if (!ResponseValidator.CheckResponseSuccess(getUserViewedModelsResponse))
            {
                return;
            }

            ApiResponse<Model[]> getAllModelsResponse = 
                await ApiService.GetAsync<Model>(
                    Endpoint.AllModelsByScene((int)AppDataService.SelectedSceneId)
                );
            
            if (!ResponseValidator.CheckResponseSuccess(getAllModelsResponse))
            {
                return;
            }

            userViewedModels = getUserViewedModelsResponse.data;
            allModels = getAllModelsResponse.data;

            SetStatsText();
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