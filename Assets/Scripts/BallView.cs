using UnityEngine;

public class BallView : MonoBehaviour
{
    [SerializeField] private Transform chassis;

    [SerializeField] private Vector3 velocity;

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
        var position = chassis.position;
        var scale = chassis.lossyScale;
        var gameBoundsEdge = gameBounds.Reflect(position, scale, out var reflectedPosition);
        switch (gameBoundsEdge)
        {
            case GameBoundsEdge.Left:
            case GameBoundsEdge.Right:
            case GameBoundsEdge.Bottom:
            case GameBoundsEdge.Top:
                chassis.position = reflectedPosition;
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
}