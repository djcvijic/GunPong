using Logic.Brains;
using Logic.Core;
using UnityEngine;
using Util.GameBounds;
using Util.GenericPools;
using View.Audio;

namespace View.GameViews
{
    public class PaddleView : MonoBehaviour
    {
        [SerializeField] private Transform chassis;

        [SerializeField] private Transform muzzle;

        [SerializeField] private Transform ballHolder;

        [SerializeField] private float speed = 1f;

        [SerializeField] private float fireCooldown = 1f;

        [SerializeField] private BulletView bulletPrefab;

        [SerializeField] private float bulletSpeed = 1f;

        [SerializeField] private AudioClipSettings fireSoundSettings;

        [SerializeField] private AudioClipSettings hurtSoundSettings;

        private float timeSinceLastFire;

        public Player Owner { get; set; }

        public IPaddleBrain Brain { get; set; }

        private BallView attachedBall;

        private GameBounds gameBounds;

        public float Speed => speed;

        public bool IsBallAttached => attachedBall != null;

        private void Start()
        {
            gameBounds = GameViewController.Instance.GameBoundsFactory.Create(true, false, chassis.lossyScale);
        }

        private void Update()
        {
            if (GameViewController.Instance.GameState != GameState.Playing) return;

            var inputParams = Brain.Act(Time.deltaTime);

            Move(inputParams.Horizontal, Time.deltaTime);

            KeepInBounds();

            FireIfPossible(inputParams.Fire, Time.deltaTime);
        }

        private void Move(float input, float deltaTime)
        {
            var t = transform;
            var position = t.position;
            var rotation = t.rotation;
            var clampedInput = Mathf.Clamp(input, -1f, 1f);
            var deltaX = speed * clampedInput * deltaTime;
            var deltaPosition = new Vector3(deltaX, 0f, 0f);
            var deltaPositionRotated = rotation * deltaPosition;
            position += deltaPositionRotated;
            t.position = position;
        }

        private void KeepInBounds()
        {
            var position = transform.position;
            if (gameBounds.IsOut(position))
            {
                transform.position = gameBounds.Clamp(position);
            }
        }

        private void FireIfPossible(bool input, float deltaTime)
        {
            timeSinceLastFire += deltaTime;
            if (!input || timeSinceLastFire < fireCooldown) return;

            timeSinceLastFire = 0f;
            if (IsBallAttached)
            {
                UnattachBall();
            }
            else
            {
                Fire();
            }
        }

        private void Fire()
        {
            var bulletPosition = muzzle.position;
            var bulletRotation = muzzle.rotation;
            var bulletDirection = muzzle.up;
            var bulletVelocity = bulletSpeed * bulletDirection;
            var bullet = GenericMonoPool<BulletView>.Instance.GetOrCreate(
                bulletPrefab, bulletPosition, bulletRotation, GameViewController.Instance.transform);
            bullet.Initialize(Owner, bulletVelocity);
            GameViewController.Instance.AudioManager.PlayAudio(fireSoundSettings);
        }

        public void GetHitBy(BulletView bullet)
        {
            GameViewController.Instance.BulletHitPaddle(this);
            GameViewController.Instance.AudioManager.PlayAudio(hurtSoundSettings);
        }

        public void GetHitBy(BallView ball)
        {
            GameViewController.Instance.BallHitPaddle(this);
        }

        public void AttachBall(BallView ball)
        {
            ball.AttachTo(ballHolder);
            attachedBall = ball;
        }

        private void UnattachBall()
        {
            attachedBall.UnattachFrom(this);
            attachedBall = null;
        }
    }
}