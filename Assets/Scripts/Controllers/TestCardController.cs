using UnityEngine;
using TMPro;
using VirtualEngineer.Models;

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
        private Quiz quiz;

        public void Init(Quiz quiz)
        {
            this.quiz = quiz;
            title.text = quiz.title;
        }
    }
}