using UnityEngine;

public class PaddleView : MonoBehaviour
{
    [SerializeField] private Transform chassis;

    [SerializeField] private Transform gun;

    [SerializeField] private float speed = 1f;

    [SerializeField] private float fireCooldown = 1f;

    [SerializeField] private PlayerEnum owner;

    [SerializeField] private BulletView bulletPrefab;

    [SerializeField] private float bulletSpeed = 1f;

    private float timeSinceLastFire;

    public PlayerEnum Owner => owner;

    private void Update()
    {
        var inputHorizontal = 0f;
        var inputFire = false;
        if (owner == GameView.Instance.LocalPlayer)
        {
            inputHorizontal = UIController.Instance.InputHorizontal;
            inputFire = UIController.Instance.InputFire;
        }

        Move(inputHorizontal, Time.deltaTime);

        KeepInBounds(GameView.Instance.GameBounds);

        FireIfPossible(inputFire, Time.deltaTime);
    }

    private void Move(float input, float deltaTime)
    {
        var t = transform;
        var position = t.position;
        position.x += speed * input * deltaTime;
        t.position = position;
    }

    private void KeepInBounds(GameBoundsView gameBounds)
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
        Fire();
    }

    private void Fire()
    {
        var bulletDirection = gun.up;
        var bulletPosition = gun.position + gun.lossyScale.y * bulletDirection;
        var bulletRotation = gun.rotation;
        var bulletVelocity = bulletSpeed * bulletDirection;
        var bullet = Instantiate(bulletPrefab, bulletPosition, bulletRotation);
        bullet.Initialize(owner, bulletVelocity);
    }

    public void GetHitBy(BulletView bulletView)
    {
        Debug.Log("OUCH");
    }
}
