using UnityEngine;
using TMPro;
using VirtualEngineer.Models;
using UnityEngine.UI;
using System;

namespace VirtualEngineer.Controllers
{
    public class TestCardController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private TMP_Text result;
        [SerializeField]
        private TMP_Text attempt;
        [SerializeField]
        private Button passBtn;
        [SerializeField]
        private Button aboutBtn;
        private Quiz quiz;
        private Result[] results;

        public void Init(Quiz quiz, Result[] results)
        {
            this.quiz = quiz;
            this.results = results;

            InitCardInfo();
        }

        private void InitCardInfo()
        {
            title.text = quiz.title;

            CalcUserResult();
            CalcUserAttempts();
        }

        private void CalcUserResult()
        {
            if (results.Length == 0)
            {
                result.text = "Результат: -";
                return;
            }

            result.text = $"Результат: {results[^1].percent}%";
        }
        
        private void CalcUserAttempts()
        {
            int userAttempts = quiz.attempts_count - results.Length;
            if (userAttempts <= 0)
            {
                userAttempts = 0;
                passBtn.interactable = false;
            }

            attempt.text = $"Попытки: {userAttempts}/{quiz.attempts_count}";
        }
    }
}