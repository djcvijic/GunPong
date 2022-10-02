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

        public bool KeepInBoundsPadded(Vector3 position, out Vector3 clampedPosition)
        {
            return IsLeavingBounds(position, out clampedPosition, out _) != GameBoundsEdge.None;
        }

        public GameBoundsEdge Reflect(Vector3 position, out Vector3 reflectedPosition)
        {
            return IsLeavingBounds(position, out _, out reflectedPosition);
        }

        public bool IsOutOfBounds(Vector3 position)
        {
            return IsLeavingBounds(position, out _, out _) != GameBoundsEdge.None;
        }

        private GameBoundsEdge IsLeavingBounds(Vector3 position, out Vector3 clampedPosition, out Vector3 reflectedPosition)
        {
            var result = GameBoundsEdge.None;
            clampedPosition = position;
            reflectedPosition = position;

            if (constrainX)
            {
                var minX = gameBoundsPosition.x - 0.5f * gameBoundsScale.x + 0.5f * scale.x;
                if (position.x < minX)
                {
                    clampedPosition.x = minX;
                    reflectedPosition.x = 2f * minX - position.x;
                    result = GameBoundsEdge.Left;
                }
                else
                {
                    var maxX = gameBoundsPosition.x + 0.5f * gameBoundsScale.x - 0.5f * scale.x;
                    if (position.x > maxX)
                    {
                        clampedPosition.x = maxX;
                        reflectedPosition.x = 2f * maxX - position.x;
                        result = GameBoundsEdge.Right;
                    }
                }
            }

            if (constrainY)
            {
                var minY = gameBoundsPosition.y - 0.5f * gameBoundsScale.y + 0.5f * scale.y;
                if (position.y < minY)
                {
                    clampedPosition.y = minY;
                    reflectedPosition.y = 2f * minY - position.y;
                    result = GameBoundsEdge.Bottom;
                }
                else
                {
                    var maxY = gameBoundsPosition.y + 0.5f * gameBoundsScale.y - 0.5f * scale.y;
                    if (position.y > maxY)
                    {
                        clampedPosition.y = maxY;
                        reflectedPosition.y = 2f * maxY - position.y;
                        result = GameBoundsEdge.Top;
                    }
                }
            }

            if (constrainZ)
            {
                var minZ = gameBoundsPosition.z - 0.5f * gameBoundsScale.z + 0.5f * scale.z;
                if (position.z < minZ)
                {
                    clampedPosition.z = minZ;
                    reflectedPosition.z = 2f * minZ - position.z;
                    result = GameBoundsEdge.Rear;
                }
                else
                {
                    var maxZ = gameBoundsPosition.z + 0.5f * gameBoundsScale.z - 0.5f * scale.z;
                    if (position.z > maxZ)
                    {
                        clampedPosition.z = maxZ;
                        reflectedPosition.z = 2f * maxZ - position.z;
                        result = GameBoundsEdge.Front;
                    }
                }
            }

            return result;
        }
    }
}