using UnityEngine;
using View.Common;

namespace Logic.Core
{
    public class GameBounds
    {
        private readonly Vector3 gameBoundsPosition;

        private readonly Vector3 gameBoundsScale;

        private readonly Vector3 scale;

        private readonly bool constrainX;

        private readonly bool constrainY;

        private readonly bool constrainZ;

        private float MinX => gameBoundsPosition.x - 0.5f * gameBoundsScale.x + 0.5f * scale.x;

        private float MaxX => gameBoundsPosition.x + 0.5f * gameBoundsScale.x - 0.5f * scale.x;

        private float MinY => gameBoundsPosition.y - 0.5f * gameBoundsScale.y + 0.5f * scale.y;

        private float MaxY => gameBoundsPosition.y + 0.5f * gameBoundsScale.y - 0.5f * scale.y;

        private float MinZ => gameBoundsPosition.z - 0.5f * gameBoundsScale.z + 0.5f * scale.z;

        private float MaxZ => gameBoundsPosition.z + 0.5f * gameBoundsScale.z - 0.5f * scale.z;

        public GameBounds(Vector3 gameBoundsPosition, Vector3 gameBoundsScale, Vector3 scale, 
            bool constrainX, bool constrainY, bool constrainZ)
        {
            this.gameBoundsPosition = gameBoundsPosition;
            this.gameBoundsScale = gameBoundsScale;
            this.scale = scale;
            this.constrainX = constrainX;
            this.constrainY = constrainY;
            this.constrainZ = constrainZ;
        }

        public BoundingCuboid GetBoundingCuboid()
        {
            return new BoundingCuboid(MinX, MaxX, MinY, MaxY, MinZ, MaxZ);
        }

        public bool IsLeavingBounds(Vector3 position, out GameBoundsEdge edge, out Vector3 clampedPosition,
            out Vector3 reflectedPosition)
        {
            edge = GameBoundsEdge.None;
            clampedPosition = position;
            reflectedPosition = position;

            if (constrainX)
            {
                var minX = MinX;
                if (position.x < minX)
                {
                    clampedPosition.x = minX;
                    reflectedPosition.x = 2f * minX - position.x;
                    edge = GameBoundsEdge.Left;
                }
                else
                {
                    var maxX = MaxX;
                    if (position.x > maxX)
                    {
                        clampedPosition.x = maxX;
                        reflectedPosition.x = 2f * maxX - position.x;
                        edge = GameBoundsEdge.Right;
                    }
                }
            }

            if (constrainY)
            {
                var minY = MinY;
                if (position.y < minY)
                {
                    clampedPosition.y = minY;
                    reflectedPosition.y = 2f * minY - position.y;
                    edge = GameBoundsEdge.Bottom;
                }
                else
                {
                    var maxY = MaxY;
                    if (position.y > maxY)
                    {
                        clampedPosition.y = maxY;
                        reflectedPosition.y = 2f * maxY - position.y;
                        edge = GameBoundsEdge.Top;
                    }
                }
            }

            if (constrainZ)
            {
                var minZ = MinZ;
                if (position.z < minZ)
                {
                    clampedPosition.z = minZ;
                    reflectedPosition.z = 2f * minZ - position.z;
                    edge = GameBoundsEdge.Rear;
                }
                else
                {
                    var maxZ = MaxZ;
                    if (position.z > maxZ)
                    {
                        clampedPosition.z = maxZ;
                        reflectedPosition.z = 2f * maxZ - position.z;
                        edge = GameBoundsEdge.Front;
                    }
                }
            }

            return edge != GameBoundsEdge.None;
        }
    }
}