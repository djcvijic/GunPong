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

    public PlayerEnum Owner { get; private set; }

    private PaddleBrain brain;

    public bool IsLocalPlayer => isLocalPlayer;

    public float Speed => speed;

    private void Start()
    {
        GameView.Instance.InitializeMe(this);
    }

    public void Initialize(PlayerEnum newOwner, PaddleBrain newBrain)
    {
        Owner = newOwner;
        brain = newBrain;
    }

    private void Update()
    {
        var inputParams = isLocalPlayer
            ? UIController.Instance.InputParams
            : brain.Act(Time.deltaTime);

        Move(inputParams.Horizontal, Time.deltaTime);

        KeepInBounds(GameView.Instance.GameBounds);

        FireIfPossible(inputParams.Fire, Time.deltaTime);
    }

    private void Move(float input, float deltaTime)
    {
        var clampedInput = Mathf.Clamp(input, -1f, 1f);
        var t = transform;
        var position = t.position;
        position.x += speed * clampedInput * deltaTime;
        t.position = position;
    }

    private void KeepInBounds(GameBoundsView gameBounds)
    {
        var position = transform.position;
        var chassisScale = chassis.lossyScale;
        if (gameBounds.KeepInBoundsPadded(position, chassisScale, out var constrainedPosition))
        {
            transform.position = constrainedPosition;
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

    public void GetHitBy(BulletView bullet)
    {
        Debug.Log("OUCH");
    }

    public void GetHitBy(BallView ball)
    {
        Debug.Log(isLocalPlayer ? "PING" : "PONG");
    }
}