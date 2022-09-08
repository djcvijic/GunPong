using UnityEngine;

public class PaddleView : MonoBehaviour
{
    [SerializeField] private Transform chassis;

    // [SerializeField] private Transform gun;

    [SerializeField] private float speed = 1f;

    [SerializeField] private float fireCooldown = 1f;

    private float timeSinceLastFire;

    private Transform GameBounds => GameView.Instance.GameBounds;

    private void Update()
    {
        var inputHorizontal = UIController.Instance.InputHorizontal;
        Move(inputHorizontal, Time.deltaTime);
        KeepInBounds(GameBounds.transform);

        var inputFire = UIController.Instance.InputFire;
        FireIfPossible(inputFire, Time.deltaTime);
    }

    private void Move(float input, float deltaTime)
    {
        var t = transform;
        var position = t.position;
        position.x += speed * input * deltaTime;
        t.position = position;
    }

    private void KeepInBounds(Transform gameBounds)
    {
        var position = transform.position;
        var chassisPositionX = chassis.position.x;
        var chassisScaleX = chassis.localScale.x;
        var gameBoundsPositionX = gameBounds.position.x;
        var gameBoundsScaleX = gameBounds.localScale.x;
        var minX = gameBoundsPositionX - 0.5f * gameBoundsScaleX + 0.5f * chassisScaleX;
        var maxX = gameBoundsPositionX + 0.5f * gameBoundsScaleX - 0.5f * chassisScaleX;
        if (chassisPositionX < minX)
        {
            position.x = minX;
        }
        else if (chassisPositionX > maxX)
        {
            position.x = maxX;
        }

        transform.position = position;
    }

    private void FireIfPossible(bool input, float deltaTime)
    {
        timeSinceLastFire += deltaTime;
        if (!input || timeSinceLastFire < fireCooldown) return;

        timeSinceLastFire = 0f;
        Debug.Log("PEW");
    }
}
