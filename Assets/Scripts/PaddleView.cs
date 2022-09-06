using UnityEngine;

public class PaddleView : MonoBehaviour
{
    [SerializeField] private Joystick joystick;

    [SerializeField] private float speed = 1f;

    private void Update()
    {
        Move(joystick.Horizontal, Time.deltaTime);
    }

    private void Move(float amount, float deltaTime)
    {
        var t = transform;
        var position = t.position;
        position.x += speed * amount * deltaTime;
        t.position = position;
    }
}
