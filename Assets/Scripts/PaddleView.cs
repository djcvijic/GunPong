using UnityEngine;

public class PaddleView : MonoBehaviour
{
    [SerializeField] private Transform chassis;

    [SerializeField] private Transform muzzle;

    [SerializeField] private float speed = 1f;

    [SerializeField] private float fireCooldown = 1f;

    [SerializeField] private bool isLocalPlayer;

    [SerializeField] private BulletView bulletPrefab;

    [SerializeField] private float bulletSpeed = 1f;

    private float timeSinceLastFire;

    public bool IsLocalPlayer => isLocalPlayer;

    public PlayerEnum Owner => GameView.Instance.GetOwner(this);

    private void Update()
    {
        var inputHorizontal = 0f;
        var inputFire = false;
        if (isLocalPlayer)
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
        var bulletPosition = muzzle.position;
        var bulletRotation = muzzle.rotation;
        var bulletDirection = muzzle.up;
        var bulletVelocity = bulletSpeed * bulletDirection;
        var bullet = GenericMonoPool<BulletView>.Instance.GetOrCreate(bulletPrefab, bulletPosition, bulletRotation);
        bullet.Initialize(Owner, bulletVelocity);
    }

    public void GetHitBy(BulletView bulletView)
    {
        Debug.Log("OUCH");
    }
}
