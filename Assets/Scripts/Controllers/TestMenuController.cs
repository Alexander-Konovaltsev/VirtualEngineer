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
        private Quiz quiz;
        private Transform selectTestMenuTransform;
        private List<Question> questions;
        private List<Question> selectedQuestions;
        private HashSet<int> answeredIds;
        private int currentQuestionIndex = 0;
        private Coroutine timerCoroutine;
        private bool isFinished = false;
        private float remainingTime;
        private List<UserAnswer> userAnswers = new List<UserAnswer>();

        public void Init(Quiz quiz, Transform selectTestMenuTransform)
        {
            this.quiz = quiz;
            this.selectTestMenuTransform = selectTestMenuTransform;
        }

        private void Awake()
        {
            answerBtn.onClick.AddListener(SubmitAnswer);
            skipBtn.onClick.AddListener(SkipQuestion);
        }
        
        private async void OnEnable()
        {
            ResizeMenu(transform, 180, 140);

            gameObject.transform.SetPositionAndRotation(selectTestMenuTransform.position, selectTestMenuTransform.rotation);

            await LoadQuestions();

            SelectRandomQuestions();

            StartTimer();

            GenerateQuestion();
        }

        private async Task LoadQuestions()
        {
            testContainer.gameObject.SetActive(false);
            loadText.gameObject.SetActive(true);
            
            Question[] questionsArr = await ApiService.GetAsyncPrivate<Question>(Endpoint.QuestionsByQuizId(quiz.id));
            questions = questionsArr.ToList();

            loadText.gameObject.SetActive(false);
            testContainer.gameObject.SetActive(true);
        }

        private void SelectRandomQuestions()
        {
            selectedQuestions = questions.OrderBy(x => UnityEngine.Random.value).Take(quiz.questions_count).ToList();
            
            answeredIds = new HashSet<int>();
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

        private void SubmitAnswer()
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

        private void SkipQuestion()
        {
            NextQuestion();
        }

        private int CalculateResultPercent()
        {
            int correctCount = 0;

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

        private void FinishTest()
        {
            if (isFinished)
                return;

            isFinished = true;

            if(timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }

            int percent = CalculateResultPercent();

            Debug.Log(percent);
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
    }
}