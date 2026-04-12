using TMPro;
using UnityEngine;

namespace VirtualEngineer.Controllers
{
    public class InfoModelCardController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text title;
        [SerializeField]
        private TMP_Text descripiton;

        public void Init(string title, string descripiton)
        {
            this.title.text = title;
            this.descripiton.text = descripiton;
        }
    }
}