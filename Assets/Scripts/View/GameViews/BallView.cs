using System;
using Logic.Core;
using UnityEngine;
using View.Audio;
using View.Common;

namespace View.GameViews
{
    public class BallView : MonoBehaviour
    {
        [SerializeField] private Transform chassis;

        [SerializeField] private float speed;

        [SerializeField] private AudioClipSettings reflectSoundSettings;

        private Vector3 velocity;

        private Transform attachPoint;

        private bool IsAttached => attachPoint != null;

        private void Update()
        {
            if (IsAttached)
            {
                FollowAttachPoint();
            }

            if (GameViewController.Instance.GameState != GameState.Playing) return;

            if (!IsAttached)
            {
                Move(Time.deltaTime);
            }

            AttemptCollideWithBounds(GameViewController.Instance.GameBounds);
        }

        private void FollowAttachPoint()
        {
            transform.position = attachPoint.position;
        }

        private void Move(float deltaTime)
        {
            transform.localPosition += deltaTime * velocity;
        }

        private void AttemptCollideWithBounds(GameBoundsView gameBounds)
        {
            var position = transform.position;
            var scale = chassis.lossyScale;
            var gameBoundsEdge = gameBounds.Reflect(position, scale, out var reflectedPosition);
            switch (gameBoundsEdge)
            {
                case GameBoundsEdge.Left:
                case GameBoundsEdge.Right:
                    transform.position = reflectedPosition;
                    ReflectVelocity(gameBoundsEdge);
                    GameViewController.Instance.AudioManager.PlayAudio(reflectSoundSettings);
                    break;
                case GameBoundsEdge.Bottom:
                    GameViewController.Instance.BallHitBottom();
                    break;
                case GameBoundsEdge.Top:
                    GameViewController.Instance.BallHitTop();
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
            if (GameViewController.Instance.GameState != GameState.Playing) return;

            var paddle = other.GetComponentInParent<PaddleView>();
            if (paddle != null)
            {
                paddle.GetHitBy(this);
                ReflectFrom(paddle);
                GameViewController.Instance.AudioManager.PlayAudio(reflectSoundSettings);
                return;
            }

            var obstacle = other.GetComponentInParent<ObstacleView>();
            if (obstacle != null)
            {
                ReflectFrom(obstacle, other);
                GameViewController.Instance.AudioManager.PlayAudio(reflectSoundSettings);
                return;
            }
        }

        private void ReflectFrom(PaddleView paddle)
        {
            var newDirection = (transform.position - paddle.transform.position).normalized;
            velocity = speed * newDirection;
        }

        private void ReflectFrom(ObstacleView obstacle, Collider2D col)
        {
            var obstacleEdge = obstacle.DetectEdge(col);
            switch (obstacleEdge)
            {
                case CollisionEdge.None:
                    break;
                case CollisionEdge.Left:
                    velocity.x = -Mathf.Abs(velocity.x);
                    break;
                case CollisionEdge.Right:
                    velocity.x = Mathf.Abs(velocity.x);
                    break;
                case CollisionEdge.Bottom:
                    velocity.y = -Mathf.Abs(velocity.y);
                    break;
                case CollisionEdge.Top:
                    velocity.y = Mathf.Abs(velocity.y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AttachTo(Transform newAttachPoint)
        {
            attachPoint = newAttachPoint;
        }

        public void UnattachFrom(PaddleView paddle)
        {
            attachPoint = null;
            ReflectFrom(paddle);
            GameViewController.Instance.AudioManager.PlayAudio(reflectSoundSettings);
        }
    }
}