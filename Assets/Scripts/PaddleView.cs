using UnityEngine;

public class PaddleView : MonoBehaviour
{
    [SerializeField] private Transform chassis;

    // [SerializeField] private Transform gun;

    [SerializeField] private float speed = 1f;

    [SerializeField] private float fireCooldown = 1f;

    private float timeSinceLastFire;

    private GameBounds GameBounds => GameView.Instance.GameBounds;

    private void Update()
    {
        var inputHorizontal = UIController.Instance.InputHorizontal;
        Move(inputHorizontal, Time.deltaTime);

        KeepInBounds(GameBounds);

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

    private void KeepInBounds(GameBounds gameBounds)
    {
        var position = transform.position;
        var chassisScale = chassis.lossyScale;
        if (gameBounds.KeepInBounds(ref position, chassisScale))
        {
            transform.position = position;
        }
    }

    private void FireIfPossible(bool input, float deltaTime)
    {
        timeSinceLastFire += deltaTime;
        if (!input || timeSinceLastFire < fireCooldown) return;

        timeSinceLastFire = 0f;
        Debug.Log("PEW");
    }
}
