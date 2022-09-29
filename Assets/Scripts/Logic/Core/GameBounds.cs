using UnityEngine;
using Util.Extensions;
using View.Common;

namespace Logic.Core
{
    public class GameBounds
    {
        private readonly Transform transform;

        private readonly Transform paddedBoundsTransform;

        private readonly bool constrainX;

        private readonly bool constrainY;

        private readonly bool constrainZ;

        public GameBounds(Transform transform, Transform paddedBoundsTransform, bool constrainX, bool constrainY, bool constrainZ)
        {
            this.transform = transform;
            this.paddedBoundsTransform = paddedBoundsTransform;
            this.constrainX = constrainX;
            this.constrainY = constrainY;
            this.constrainZ = constrainZ;
        }

        public bool KeepInBoundsPadded(Vector3 position, Vector3 scale, out Vector3 clampedPosition)
        {
            var result = IsLeavingBounds(transform, position, scale,
                paddedBoundsTransform.localScale, constrainX, constrainY, constrainZ,
                out clampedPosition, out _);
            return result != GameBoundsEdge.None;
        }

        public GameBoundsEdge Reflect(Vector3 position, Vector3 scale, out Vector3 reflectedPosition)
        {
            var result = IsLeavingBounds(transform, position, scale,
                Vector3.one, constrainX, constrainY, constrainZ,
                out _, out reflectedPosition);
            return result;
        }

        private static GameBoundsEdge IsLeavingBounds(Transform bounds, Vector3 position, Vector3 scale,
            Vector3 paddedScale, bool constrainX, bool constrainY, bool constrainZ,
            out Vector3 clampedPosition, out Vector3 reflectedPosition)
        {
            var boundsPosition = bounds.position;
            var boundsScale = bounds.localScale;
            var result = GameBoundsEdge.None;
            clampedPosition = position;
            reflectedPosition = position;

            if (constrainX)
            {
                var minX = boundsPosition.x - 0.5f * paddedScale.x * boundsScale.x + 0.5f * scale.x;
                if (position.x < minX)
                {
                    clampedPosition.x = minX;
                    reflectedPosition.x = 2f * minX - position.x;
                    result = GameBoundsEdge.Left;
                }
                else
                {
                    var maxX = boundsPosition.x + 0.5f * paddedScale.x * boundsScale.x - 0.5f * scale.x;
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
                var minY = boundsPosition.y - 0.5f * paddedScale.y * boundsScale.y + 0.5f * scale.y;
                if (position.y < minY)
                {
                    clampedPosition.y = minY;
                    reflectedPosition.y = 2f * minY - position.y;
                    result = GameBoundsEdge.Bottom;
                }
                else
                {
                    var maxY = boundsPosition.y + 0.5f * paddedScale.y * boundsScale.y - 0.5f * scale.y;
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
                var minZ = boundsPosition.z - 0.5f * paddedScale.z * boundsScale.z + 0.5f * scale.z;
                if (position.z < minZ)
                {
                    clampedPosition.z = minZ;
                    reflectedPosition.z = 2f * minZ - position.z;
                    result = GameBoundsEdge.Rear;
                }
                else
                {
                    var maxZ = boundsPosition.z + 0.5f * paddedScale.z * boundsScale.z - 0.5f * scale.z;
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

        public bool IsOutOfBounds(Vector3 position, Vector3 scale)
        {
            var t = transform;
            var boundsPosition = t.position;
            var boundsScale = t.localScale;
            var result = false;

            if (constrainX)
            {
                var minX = boundsPosition.x - 0.5f * boundsScale.x - 0.5f * scale.x;
                var maxX = boundsPosition.x + 0.5f * boundsScale.x + 0.5f * scale.x;
                var clampedX = Mathf.Clamp(position.x, minX, maxX);
                if (!clampedX.Approximately(position.x))
                {
                    result = true;
                }
            }

            if (constrainY)
            {
                var minY = boundsPosition.y - 0.5f * boundsScale.y - 0.5f * scale.y;
                var maxY = boundsPosition.y + 0.5f * boundsScale.y + 0.5f * scale.y;
                var clampedY = Mathf.Clamp(position.y, minY, maxY);
                if (!clampedY.Approximately(position.y))
                {
                    result = true;
                }
            }

            if (constrainZ)
            {
                var minZ = boundsPosition.z - 0.5f * boundsScale.z - 0.5f * scale.z;
                var maxZ = boundsPosition.z + 0.5f * boundsScale.z + 0.5f * scale.z;
                var clampedZ = Mathf.Clamp(position.z, minZ, maxZ);
                if (!clampedZ.Approximately(position.z))
                {
                    result = true;
                }
            }

            return result;
        }
    }
}