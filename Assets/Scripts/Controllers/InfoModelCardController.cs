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

        public bool isViewed;
        private int modelId;
        public int ModelId => modelId;
        private Transform cameraTransform;
        private Vector3 basePosition;

        private void Start()
        {
            cameraTransform = Camera.main.transform;
            basePosition = transform.position;
        }

        private void LateUpdate()
        {
            if (cameraTransform == null) return;

            Vector3 dirToCamera = (cameraTransform.position - basePosition).normalized;

            float offset = 0.5f;
            float heightOffset = 0.15f;

            Vector3 targetPos = basePosition + dirToCamera * offset;
            targetPos.y += heightOffset;

            Vector3 lookPos = cameraTransform.position;
            lookPos.y = targetPos.y;

            transform.position = targetPos;

            transform.LookAt(lookPos);
            transform.Rotate(0, 180f, 0);
        }
  
        public void Init(string title, string descripiton, bool isViewed, int modelId)
        {
            this.title.text = title;
            this.descripiton.text = descripiton;
            this.isViewed = isViewed;
            this.modelId = modelId;
        }
    }
}