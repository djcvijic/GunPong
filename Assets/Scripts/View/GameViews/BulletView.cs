using System;
using Logic.Core;
using UnityEngine;
using Util.GenericPools;

namespace View.GameViews
{
    public class BulletView : MonoBehaviour
    {
        private Player owner;

        private Vector3 velocity;

        private GameBounds gameBounds;

        public void Initialize(Player newOwner, Vector3 newVelocity)
        {
            owner = newOwner;
            velocity = newVelocity;
            gameObject.SetActive(true);
        }

        private void Start()
        {
            gameBounds = GameViewController.Instance.GameBoundsFactory.Create(false, true, transform.lossyScale);
        }

        private void Update()
        {
            if (GameViewController.Instance.GameState != GameState.Playing) return;

            Move(Time.deltaTime);

            DieIfOutOfBounds();
        }

        private void Move(float deltaTime)
        {
            transform.localPosition += deltaTime * velocity;
        }

        private void DieIfOutOfBounds()
        {
            if (gameBounds.IsOutOfBounds(transform.position))
            {
                Die();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (GameViewController.Instance.GameState != GameState.Playing) return;

            var paddle = other.GetComponentInParent<PaddleView>();
            if (paddle != null && paddle.Owner != owner)
            {
                paddle.GetHitBy(this);
                Die();
            }

            var obstacle = other.GetComponentInParent<ObstacleView>();
            if (obstacle != null)
            {
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
}