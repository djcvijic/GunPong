using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnChange;

    [SerializeField, Range(0f, 1f)] private float scaleWhenPressed = 1f;

    public bool Input { get; private set; }

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Input = true;
        OnChange?.Invoke();
        ScaleFromCenter(rectTransform, scaleWhenPressed);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Input = false;
        OnChange?.Invoke();
        ScaleFromCenter(rectTransform, 1f / scaleWhenPressed);
    }

    private static void ScaleFromCenter(RectTransform t, float amount)
    {
        var rect = t.rect;
        var pivot = t.pivot;
        var scale = t.localScale;
        var newScale = scale * amount;
        var offset = 0.5f * (scale - newScale);
        offset.x *= rect.width * (1f - 2f * pivot.x);
        offset.y *= rect.height * (1f - 2f * pivot.y);
        t.localPosition += offset;
        t.localScale = newScale;
    }
}