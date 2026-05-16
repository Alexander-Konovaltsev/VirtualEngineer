using UnityEngine;
using VirtualEngineer.Models;
using VirtualEngineer.Services;
using VirtualEngineer.Enums;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using VirtualEngineer.Validation;

namespace VirtualEngineer.Controllers
{
    public class TestMenuController : BaseMenuController
    {
        [SerializeField]
        private Transform testContainer;
        [SerializeField] 
        private GameObject checkBox;
        [SerializeField] 
        private GameObject radioBtn;
        [SerializeField]
        private TMP_Text loadText;
        [SerializeField]
        private TMP_Text questionContent;
        [SerializeField]
        private Transform answersContent;
        [SerializeField]
        private TMP_Text timerText;
        [SerializeField]
        private TMP_Text questionCountText;
        [SerializeField]
        private Button answerBtn;
        [SerializeField]
        private Button skipBtn;
        [SerializeField]
        private Button backBtn;
        private Quiz quiz;
        private Transform selectTestMenuTransform;
        private Transform pauseMenuTransform;
        private List<Question> questions;
        private List<Question> selectedQuestions;
        private HashSet<int> answeredIds;
        private int currentQuestionIndex = 0;
        private Coroutine timerCoroutine;
        private bool isFinished = false;
        private float remainingTime;
        private List<UserAnswer> userAnswers = new List<UserAnswer>();
        private int correctCount;

        public void Init(Quiz quiz, Transform selectTestMenuTransform, Transform pauseMenuTransform)
        {
            this.quiz = quiz;
            this.selectTestMenuTransform = selectTestMenuTransform;
            this.pauseMenuTransform = pauseMenuTransform;
        }

        private void Awake()
        {
            answerBtn.onClick.AddListener(SubmitAnswerAction);
            skipBtn.onClick.AddListener(SkipQuestionAction);
            backBtn.onClick.AddListener(BackToSelectTestMenuAction);
        }
        
        private async void OnEnable()
        {
            PrepareMenu();

            PrepareFields();
            
            await LoadQuestions();

            SelectRandomQuestions();

            StartTimer();

            GenerateQuestion();
        }

        private async Task LoadQuestions()
        {
            testContainer.gameObject.SetActive(false);
            loadText.gameObject.SetActive(true);
            
            ApiResponse<Question[]> getQuestionsResponse = 
                await ApiService.GetAsync<Question>(Endpoint.QuestionsByQuizId(quiz.id));

            if (!ResponseValidator.CheckResponseSuccess(getQuestionsResponse))
            {
                return;
            }

            Question[] questionsArr = getQuestionsResponse.data;

            questions = questionsArr.ToList();

            loadText.gameObject.SetActive(false);
            testContainer.gameObject.SetActive(true);
        }

        private void SelectRandomQuestions()
        {
            selectedQuestions = questions.OrderBy(x => UnityEngine.Random.value).Take(quiz.questions_count).ToList();
        }

        private void StartTimer()
        {
            remainingTime = quiz.time * 60;

            timerCoroutine = StartCoroutine(TimerCoroutine());
        }

        private IEnumerator TimerCoroutine()
        {
            while (remainingTime > 0)
            {
                remainingTime -= 1f;

                UpdateTimerText();

                yield return new WaitForSeconds(1f);
            }

            FinishTest();
        }

        private void UpdateTimerText()
        {
            TimeSpan time = TimeSpan.FromSeconds(remainingTime);

            timerText.text = time.ToString(@"hh\:mm\:ss");
        }

        private void GenerateQuestion()
        {
            ClearAnswers();

            answerBtn.interactable = false;

            if (selectedQuestions.Count - userAnswers.Count == 1)
            {
                skipBtn.interactable = false;
            }

            Question question = selectedQuestions[currentQuestionIndex];

            questionContent.text = question.question_text;
            questionCountText.text = $"{currentQuestionIndex + 1}/{selectedQuestions.Count}";

            GenerateAnswers(question);
        }

        private void GenerateAnswers(Question question)
        {
            bool isMultiple = question.question_type.name == "MultipleChoice";

            ToggleGroup toggleGroup = answersContent.GetComponent<ToggleGroup>();

            toggleGroup.enabled = !isMultiple;

            foreach (Answer answer in question.answers)
            {
                GameObject prefab = isMultiple ? checkBox : radioBtn;

                GameObject obj = Instantiate(prefab, answersContent);

                Toggle toggle = obj.GetComponent<Toggle>();

                if (!isMultiple)
                {
                    toggle.group = toggleGroup;
                }

                obj.transform.Find("Label").GetComponent<TMP_Text>().text = answer.text;

                toggle.onValueChanged.AddListener(_ => OnAnswerSelected());
            }
        }

        private void ClearAnswers()
        {
            foreach (Transform child in answersContent)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnAnswerSelected()
        {
            bool hasSelected = false;

            foreach (Transform child in answersContent)
            {
                Toggle toggle = child.GetComponent<Toggle>();

                if (toggle.isOn)
                {
                    hasSelected = true;
                    break;
                }
            }

            answerBtn.interactable = hasSelected;
        }

        private void SubmitAnswerAction()
        {
            Question question = selectedQuestions[currentQuestionIndex];

            List<int> selectedIds = new List<int>();

            int index = 0;

            foreach (Transform child in answersContent)
            {
                Toggle toggle = child.GetComponent<Toggle>();

                if (toggle.isOn)
                {
                    selectedIds.Add(question.answers[index].id);
                }

                index++;
            }

            userAnswers.Add(new UserAnswer
            {
                question_id = question.id,
                selected_answer_ids = selectedIds.ToArray(),
                created_at = DateTime.UtcNow
            });

            answeredIds.Add(question.id);

            NextQuestion();
        }

        private void NextQuestion()
        {
            int nextIndex = GetNextQuestionIndex(currentQuestionIndex);

            if (nextIndex == -1)
            {
                FinishTest();
                return;
            }

            currentQuestionIndex = nextIndex;

            GenerateQuestion();
        }

        private void SkipQuestionAction()
        {
            NextQuestion();
        }

        private int CalculateResultPercent()
        {
            correctCount = 0;

            foreach (Question question in selectedQuestions)
            {
                UserAnswer userAnswer = userAnswers.Find(x => x.question_id == question.id);

                if (userAnswer == null)
                    continue;

                int[] correctIds =
                    question.answers
                    .Where(x => x.is_correct)
                    .Select(x => x.id)
                    .OrderBy(x => x)
                    .ToArray();

                int[] userIds =
                    userAnswer.selected_answer_ids
                    .OrderBy(x => x)
                    .ToArray();

                bool isCorrect = correctIds.SequenceEqual(userIds);

                if (isCorrect)
                {
                    correctCount++;
                }
            }

            return Mathf.RoundToInt(
                (float)correctCount
                / selectedQuestions.Count
                * 100f
            );
        }

        private async void FinishTest()
        {
            if (isFinished)
                return;

            isFinished = true;

            if(timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }

            int percent = CalculateResultPercent();

            Result result = await CreateResult(percent);

            if (result == null)
            {
                return;
            }

            ResultDetail[] resultDetails = await CreateResultsDetails(result);

            if (resultDetails == null)
            {
                return;
            }

            FormResultText(percent);
        }

        private int GetNextQuestionIndex(int currentIndex)
        {
            int start = currentIndex;

            do
            {
                currentIndex++;

                if (currentIndex >= selectedQuestions.Count)
                    currentIndex = 0;

                int id = selectedQuestions[currentIndex].id;

                if (!answeredIds.Contains(id))
                    return currentIndex;

            } while (currentIndex != start);

            return -1;
        }

        private void FormResultText(int percent)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"<b>Результат:</b> {percent}%");
            sb.AppendLine($"{correctCount}/{quiz.questions_count}");

            loadText.text = sb.ToString();

            backBtn.gameObject.SetActive(true);
            testContainer.gameObject.SetActive(false);
            loadText.gameObject.SetActive(true);

            AppDataService.IsTestMode = false;
        }

        private void PrepareMenu()
        {
            ResizeMenu(transform, 180, 140);
            gameObject.transform.SetPositionAndRotation(selectTestMenuTransform.position, selectTestMenuTransform.rotation);
        }

        private void PrepareFields()
        {
            answeredIds = new HashSet<int>();
            userAnswers.Clear();
            currentQuestionIndex = 0;
            loadText.text = "Загрузка...";
            backBtn.gameObject.SetActive(false);
            AppDataService.IsTestMode = true;
            isFinished = false;
            skipBtn.interactable = true;
        }

        private void BackToSelectTestMenuAction()
        {
            gameObject.SetActive(false);

            SelectTestMenuController selectTestMenuController = 
                selectTestMenuTransform.GetComponent<SelectTestMenuController>();

            selectTestMenuController.Init(pauseMenuTransform);
            selectTestMenuTransform.gameObject.SetActive(true);
        }

        private async Task<Result> CreateResult(int percent)
        {
            ResultCreateRequest resultRequest = new ResultCreateRequest
            {
                percent = percent,
                total_answers = userAnswers.Count,
                correct_answers = correctCount,
                quiz_id = quiz.id
            };

            ApiResponse<Result> resultResponse = 
                await ApiService.PostAsync<ResultCreateRequest, Result>(
                    Endpoint.CreateResult, 
                    resultRequest
                );

            if (!ResponseValidator.CheckResponseSuccess(resultResponse))
            {
                return null;
            }

            return resultResponse.data;
        }

        private async Task<ResultDetail[]> CreateResultsDetails(Result result)
        {
            List<ResultDetailCreateRequest> resultDetails = new List<ResultDetailCreateRequest>();

            foreach (UserAnswer userAnswer in userAnswers)
            {
                foreach (int answerId in userAnswer.selected_answer_ids)
                {
                    resultDetails.Add(new ResultDetailCreateRequest
                    {
                        created_at = userAnswer.created_at,
                        result_id = result.id,
                        question_id = userAnswer.question_id,
                        answer_id = answerId
                    });
                }
            }

            ApiResponse<ResultDetail[]> resultDetailsResponse = 
                await ApiService.PostAsync<List<ResultDetailCreateRequest>, ResultDetail[]>(
                    Endpoint.CreateResultDetail, 
                    resultDetails
                );

            if (!ResponseValidator.CheckResponseSuccess(resultDetailsResponse))
            {
                return null;
            }

            return resultDetailsResponse.data;
        }
    }
}