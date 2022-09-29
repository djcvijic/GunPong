using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace View.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnPress;

        public event Action OnRelease;

        [SerializeField, Range(0f, 1f)] private float scaleWhenPressed = 1f;

        public bool Input { get; private set; }

        private RectTransform rectTransform;

        private Vector3 originalScale;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Input = true;
            OnPress?.Invoke();
            originalScale = rectTransform.localScale;
            ScaleFromCenter(rectTransform, originalScale * scaleWhenPressed);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Input = false;
            OnRelease?.Invoke();
            ScaleFromCenter(rectTransform, originalScale);
        }

        private static void ScaleFromCenter(RectTransform t, Vector3 newScale)
        {
            var rect = t.rect;
            var pivot = t.pivot;
            var scale = t.localScale;
            var offset = 0.5f * (scale - newScale);
            offset.x *= rect.width * (1f - 2f * pivot.x);
            offset.y *= rect.height * (1f - 2f * pivot.y);
            offset.z = 0;
            t.localPosition += offset;
            t.localScale = newScale;
        }
    }
}