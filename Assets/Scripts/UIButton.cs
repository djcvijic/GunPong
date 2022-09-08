using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnChange;

    [SerializeField, Range(0f, 1f)] private float scaleWhenPressed = 1f;

    private Vector3 originalScale;

    public bool Input { get; private set; }

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Input = true;
        OnChange?.Invoke();
        transform.localScale = scaleWhenPressed * originalScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Input = false;
        OnChange?.Invoke();
        transform.localScale = originalScale;
    }
}