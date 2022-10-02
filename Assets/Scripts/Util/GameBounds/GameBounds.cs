using UnityEngine;

namespace Util.GameBounds
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

        public bool IsOut(Vector3 position)
        {
            if (constrainX)
            {
                if (position.x < MinX) return true;
                if (position.x > MaxX) return true;
            }

            if (constrainY)
            {
                if (position.y < MinY) return true;
                if (position.y > MaxY) return true;
            }

            if (constrainZ)
            {
                if (position.z < MinZ) return true;
                if (position.z > MaxZ) return true;
            }

            return false;
        }

        public GameBoundsEdge GetEdge(Vector3 position)
        {
            if (constrainX)
            {
                if (position.x < MinX) return GameBoundsEdge.Left;
                if (position.x > MaxX) return GameBoundsEdge.Right;
            }

            if (constrainY)
            {
                if (position.y < MinY) return GameBoundsEdge.Bottom;
                if (position.y > MaxY) return GameBoundsEdge.Top;
            }

            if (constrainZ)
            {
                if (position.z < MinZ) return GameBoundsEdge.Rear;
                if (position.z > MaxZ) return GameBoundsEdge.Front;
            }

            return GameBoundsEdge.None;
        }

        public Vector3 Clamp(Vector3 position)
        {
            if (constrainX)
            {
                if (position.x < MinX) position.x = MinX;
                if (position.x > MaxX) position.x = MaxX;
            }

            if (constrainY)
            {
                if (position.y < MinY) position.y = MinY;
                if (position.y > MaxY) position.y = MaxY;
            }

            if (constrainZ)
            {
                if (position.z < MinZ) position.z = MinZ;
                if (position.z > MaxZ) position.z = MaxZ;
            }

            return position;
        }

        public Vector3 Reflect(Vector3 position)
        {
            if (constrainX)
            {
                if (position.x < MinX) position.x = 2f * MinX - position.x;
                if (position.x > MaxX) position.x = 2f * MaxX - position.x;
            }

            if (constrainY)
            {
                if (position.y < MinY) position.y = 2f * MinY - position.y;
                if (position.y > MaxY) position.y = 2f * MaxY - position.y;
            }

            if (constrainZ)
            {
                if (position.z < MinZ) position.z = 2f * MinZ - position.z;
                if (position.z > MaxZ) position.z = 2f * MaxZ - position.z;
            }

            return position;
        }
    }
}