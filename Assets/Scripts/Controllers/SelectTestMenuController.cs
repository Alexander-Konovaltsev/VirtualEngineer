using UnityEngine;
using TMPro;
using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.Enums;
using System.Linq;

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
        private Result[] results;

        private async void OnEnable()
        {
            ResizeMenu(transform, 180, 140);
            gameObject.transform.SetPositionAndRotation(pauseMenuTransform.position, pauseMenuTransform.rotation);

            ClearContent();

            loadText.gameObject.SetActive(true);

            GetUserResults();
            GetSceneTests();

            loadText.gameObject.SetActive(false);
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

        private void ClearContent()
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
        }

        private async void GetSceneTests()
        {
            quizzes = await ApiService.GetAsyncPrivate<Quiz>(Endpoint.QuizzesBySceneId((int)AppDataService.SelectedSceneId));

            foreach (Quiz quiz in quizzes)
            {
                GameObject quizObj = Instantiate(testCardPrefab, content);

                TestCardController testController = quizObj.GetComponent<TestCardController>();
                testController.Init(quiz, GetUserResultsByQuiz(quiz));
            }
        }

        private async void GetUserResults()
        {
            results = await ApiService.GetAsyncPrivate<Result>(Endpoint.ResultsByUser);
        }
        
        private Result[] GetUserResultsByQuiz(Quiz quiz)
        {
            return results
                .Where(r => r.quiz_id == quiz.id)
                .OrderBy(r => r.id)
                .ToArray();
        }
    }
}