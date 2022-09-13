using UnityEngine;

public class BallView : MonoBehaviour
{
    [SerializeField] private Transform chassis;

    [SerializeField] private Vector3 velocity;

    [SerializeField] private float speed;

    private void Update()
    {
        Move(Time.deltaTime);

        AttemptReflectFromBounds(GameView.Instance.GameBounds);
    }

    private void Move(float deltaTime)
    {
        transform.localPosition += deltaTime * velocity;
    }

    private void AttemptReflectFromBounds(GameBoundsView gameBounds)
    {
        var position = transform.position;
        var scale = chassis.lossyScale;
        var gameBoundsEdge = gameBounds.Reflect(position, scale, out var reflectedPosition);
        switch (gameBoundsEdge)
        {
            case GameBoundsEdge.Left:
            case GameBoundsEdge.Right:
            case GameBoundsEdge.Bottom:
            case GameBoundsEdge.Top:
                transform.position = reflectedPosition;
                ReflectVelocity(gameBoundsEdge);
                break;
        }
    }

    private void ReflectVelocity(GameBoundsEdge gameBoundsEdge)
    {
        switch (gameBoundsEdge)
        {
            case GameBoundsEdge.Left:
            case GameBoundsEdge.Right:
                velocity.x = -velocity.x;
                break;
            case GameBoundsEdge.Bottom:
            case GameBoundsEdge.Top:
                velocity.y = -velocity.y;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var paddle = other.GetComponentInParent<PaddleView>();
        if (paddle != null)
        {
            paddle.GetHitBy(this);
            ReflectFromPaddle(paddle);
        }
    }

    private void ReflectFromPaddle(PaddleView paddle)
    {
        var newDirection = (transform.position - paddle.transform.position).normalized;
        velocity = speed * newDirection;
    }
}