using UnityEngine;
using TMPro;
using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.Enums;

namespace VirtualEngineer.Controllers
{
    public class SelectTestMenuController : BaseMenuController
    {
        [SerializeField]
        private TMP_Text loadText;
        private Transform pauseMenuTransform;
        [SerializeField]
        private Transform content;
        [SerializeField] 
        private GameObject testCardPrefab;
        private Quiz[] quizzes;

        private async void OnEnable()
        {
            ResizeMenu(transform, 180, 140);
            gameObject.transform.SetPositionAndRotation(pauseMenuTransform.position, pauseMenuTransform.rotation);

            loadText.gameObject.SetActive(true);

            quizzes = await ApiService.GetAsyncPrivate<Quiz>(Endpoint.QuizzesBySceneId((int)AppDataService.SelectedSceneId));

            loadText.gameObject.SetActive(false);

            foreach (Quiz quiz in quizzes)
            {
                GameObject quizObj = Instantiate(testCardPrefab, content);

                TestCardController testController = quizObj.GetComponent<TestCardController>();
                testController.Init(quiz);
            }
        }

        public void Init(Transform pauseMenuTransform)
        {
            this.pauseMenuTransform = pauseMenuTransform;
        }

        public void BackToPauseMenuAction()
        {
            pauseMenuTransform.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}