using UnityEngine;
using TMPro;
using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.Enums;
using System.Linq;
using System.Threading.Tasks;
using VirtualEngineer.Validation;

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
        private TestInfoMenuController testInfoMenuController;
        private TestMenuController testMenuController;

        private void Awake()
        {
            testInfoMenuController = transform.parent.Find("TestInfoMenu").GetComponent<TestInfoMenuController>();
            testMenuController = transform.parent.Find("TestMenu").GetComponent<TestMenuController>();
        }

        private async void OnEnable()
        {
            ResizeMenu(transform, 180, 140);
            gameObject.transform.SetPositionAndRotation(pauseMenuTransform.position, pauseMenuTransform.rotation);

            ClearContent();

            loadText.gameObject.SetActive(true);

            if (!await CheckUserAllTestLearned())
            {
                loadText.text = "Для прохождения тестирования необходимо изучить все объекты на сцене";

                return;
            }

            await GetUserResults();
            await GetSceneTests();

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

        private async Task GetSceneTests()
        {
            ApiResponse<Quiz[]> getQuizzesResponse = 
                await ApiService.GetAsync<Quiz>(Endpoint.QuizzesBySceneId((int)AppDataService.SelectedSceneId));

            if (!ResponseValidator.CheckResponseSuccess(getQuizzesResponse))
            {
                return;
            }

            quizzes = getQuizzesResponse.data;

            foreach (Quiz quiz in quizzes)
            {
                GameObject quizObj = Instantiate(testCardPrefab, content);

                TestCardController testController = quizObj.GetComponent<TestCardController>();
                testController.Init(quiz, GetUserResultsByQuiz(quiz), testInfoMenuController, testMenuController, transform, pauseMenuTransform);
            }
        }

        private async Task GetUserResults()
        {
            ApiResponse<Result[]> getResultsResponse = 
                await ApiService.GetAsync<Result>(Endpoint.ResultsByUser);

            if (!ResponseValidator.CheckResponseSuccess(getResultsResponse))
            {
                return;
            }

            results = getResultsResponse.data;
        }
        
        private Result[] GetUserResultsByQuiz(Quiz quiz)
        {
            return results
                .Where(r => r.quiz_id == quiz.id)
                .OrderBy(r => r.id)
                .ToArray();
        }

        private async Task<bool> CheckUserAllTestLearned()
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

            UserModelView[] userViewedModels = getUserViewedModelsResponse.data;
            Model[] allModels = getAllModelsResponse.data;

            if (userViewedModels.Length < allModels.Count(m => m.is_informational))
                return false;
            
            return true;
        }
    }
}