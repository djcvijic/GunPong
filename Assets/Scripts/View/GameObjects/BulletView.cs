using UnityEngine;

public class BulletView : MonoBehaviour
{
    private Player owner;

    private Vector3 velocity;

    public void Initialize(Player newOwner, Vector3 newVelocity)
    {
        owner = newOwner;
        velocity = newVelocity;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (GameView.Instance.GameState != GameState.Playing) return;

        Move(Time.deltaTime);

        DieIfOutOfBounds(GameView.Instance.GameBounds);
    }

    private void Move(float deltaTime)
    {
        transform.localPosition += deltaTime * velocity;
    }

    private void DieIfOutOfBounds(GameBoundsView gameBounds)
    {
        var t = transform;
        var position = t.position;
        var scale = t.lossyScale;
        if (gameBounds.IsOutOfBounds(position, scale))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameView.Instance.GameState != GameState.Playing) return;

        var paddle = other.GetComponentInParent<PaddleView>();
        if (paddle != null && paddle.Owner != owner)
        {
            paddle.GetHitBy(this);
            Die();
        }

        var obstacle = other.GetComponentInParent<ObstacleView>();
        if (obstacle != null)
        {
            obstacle.GetHitBy(this);
            Die();
        }
    }

    private void Die()
    {
        if (!gameObject.activeSelf) return;

        gameObject.SetActive(false);
        GenericMonoPool<BulletView>.Instance.Return(this);
    }

    public void Kill()
    {
        Die();
    }
}