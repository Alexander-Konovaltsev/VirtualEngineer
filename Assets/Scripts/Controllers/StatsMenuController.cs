using TMPro;
using UnityEngine;
using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.Enums;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace VirtualEngineer.Controllers
{
    public class StatsMenuController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private TMP_Text descripiton;
        private GameObject menu;
        private Model[] allModels;
        private UserModelView[] userViewedModels;
        private Transform pauseMenuTransform;

        private void Awake()
        {
            title.text = "Статистика";
            menu = gameObject;
        }

        private void OnEnable()
        {
            descripiton.text = "";
            menu.SetActive(true);
            menu.transform.SetPositionAndRotation(pauseMenuTransform.position, pauseMenuTransform.rotation);
            LoadSceneStats();
        }

        public void Init(Transform pauseMenuTransform)
        {
            this.pauseMenuTransform = pauseMenuTransform;
        }

        private async void LoadSceneStats()
        {
            allModels = await ApiService.GetAsyncPrivate<Model>(Endpoint.AllModelsByScene((int)AppDataService.SelectedSceneId));
            userViewedModels = await ApiService.GetAsyncPrivate<UserModelView>
                               (Endpoint.AllViewedModelsByScene((int)AppDataService.SelectedSceneId));

            SetStatsText();
        }

        private void SetStatsText()
        {
            int allModelsCount = allModels.Count(m => m.is_informational);

            var viewedModelIds = new HashSet<int>(
                userViewedModels.Select(v => v.model_id)
            );

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Изучено объектов: <b>{viewedModelIds.Count}/{allModelsCount}</b>");

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

        public void BackToPauseMenu()
        {
            menu.SetActive(false);
        }
    }
}