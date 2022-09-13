using UnityEngine;

public class BallView : MonoBehaviour
{
    private Vector3 velocity;

    private void Update()
    {
        Move(Time.deltaTime);

        ReactIfLeavingBounds(GameView.Instance.GameBounds);
    }

    private void Move(float deltaTime)
    {
        transform.localPosition += deltaTime * velocity;
    }

    private void ReactIfLeavingBounds(GameBoundsView gameBounds)
    {
        var t = transform;
        var position = t.position;
        var scale = t.lossyScale;
        var gameBoundsEdge = gameBounds.Reflect(position, scale, out var reflectedPosition);
        switch (gameBoundsEdge)
        {
            case GameBoundsEdge.Left:
            case GameBoundsEdge.Right:
            case GameBoundsEdge.Bottom:
            case GameBoundsEdge.Top:
                t.position = reflectedPosition;
                break;
        }
    }
}