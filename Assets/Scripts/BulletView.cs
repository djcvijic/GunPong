using UnityEngine;

public class BulletView : MonoBehaviour
{
    private PlayerEnum owner;

    private Vector3 velocity;

    public void Initialize(PlayerEnum newOwner, Vector3 newVelocity)
    {
        owner = newOwner;
        velocity = newVelocity;
    }

    private void Update()
    {
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
        var paddle = other.GetComponentInParent<PaddleView>();
        if (paddle != null && paddle.Owner != owner)
        {
            paddle.GetHitBy(this);
            Die();
        }
    }

    private void Die()
    {
        GenericMonoPool<BulletView>.Instance.Return(this);
    }
}