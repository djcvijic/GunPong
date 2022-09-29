using UnityEngine;
using View.Common;

namespace View.GameViews
{
    public class ObstacleView : MonoBehaviour
    {
        [SerializeField] private Collider2D leftCollider;
        [SerializeField] private Collider2D rightCollider;
        [SerializeField] private Collider2D bottomCollider;
        [SerializeField] private Collider2D topCollider;

        public void GetHitBy(BallView ball)
        {
            // todo
        }

        public void GetHitBy(BulletView bullet)
        {
            // todo
        }

        public CollisionEdge DetectEdge(Collider2D col)
        {
            if (col == leftCollider) return CollisionEdge.Left;
            if (col == rightCollider) return CollisionEdge.Right;
            if (col == bottomCollider) return CollisionEdge.Bottom;
            if (col == topCollider) return CollisionEdge.Top;
            return CollisionEdge.None;
        }
    }
}