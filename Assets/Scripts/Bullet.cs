using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private PlayerEnum owner;

    [SerializeField] private Vector3 velocity;

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

    private void DieIfOutOfBounds(GameBounds gameBounds)
    {
        var t = transform;
        var position = t.position;
        var scale = t.lossyScale;
        if (gameBounds.IsOutOfBounds(position, scale))
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherPaddleView = other.GetComponent<PaddleView>();
        if (otherPaddleView != null && otherPaddleView.Owner != owner)
        {
            otherPaddleView.GetHitBy(this);
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}