using TMPro;
using UnityEngine;
using VirtualEngineer.Models;
using System.Text;

namespace VirtualEngineer.Controllers
{
    public class TestInfoMenuController : BaseMenuController
    {
        [SerializeField]
        private TMP_Text descripiton;
        private Transform selectTestMenuTransform;
        private Quiz quiz;

        private void OnEnable()
        {
            ResizeMenu(transform);

            gameObject.transform.SetPositionAndRotation(selectTestMenuTransform.position, selectTestMenuTransform.rotation);

            SetTestInfo();
        }

        public void Init(Quiz quiz, Transform selectTestMenuTransform)
        {
            this.quiz = quiz;
            this.selectTestMenuTransform = selectTestMenuTransform;
        }

        private void SetTestInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"<b>Название:</b> {quiz.title}");
            sb.AppendLine($"<b>Вопросов:</b> {quiz.questions_count}");
            sb.AppendLine($"<b>Время:</b> {quiz.time} мин");
            sb.AppendLine($"<b>Попыток:</b> {quiz.attempts_count}");
            sb.AppendLine($"<b>Описание:</b> {quiz.description}");

            descripiton.text = sb.ToString();
        }

        public void BackToSelectTestMenuAction()
        {
            selectTestMenuTransform.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}