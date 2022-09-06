using UnityEngine;

public class PaddleView : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    private void Update()
    {
        var input = UIController.Instance.InputHorizontal;
        Move(input, Time.deltaTime);
    }

    private void Move(float input, float deltaTime)
    {
        var t = transform;
        var position = t.position;
        position.x += speed * input * deltaTime;
        t.position = position;
    }
}
