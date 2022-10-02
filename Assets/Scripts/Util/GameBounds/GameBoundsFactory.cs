using UnityEngine;

namespace Util.GameBounds
{
    public class GameBoundsFactory
    {
        private readonly Vector3 gameBoundsPosition;

        private readonly Vector3 gameBoundsScale;

        private readonly Vector3 paddedBoundsScale;

        private readonly bool constrainX;

        private readonly bool constrainY;

        private readonly bool constrainZ;

        public GameBoundsFactory(Vector3 gameBoundsPosition, Vector3 gameBoundsScale,
            Vector3 paddedBoundsScale, bool constrainX, bool constrainY, bool constrainZ)
        {
            this.gameBoundsPosition = gameBoundsPosition;
            this.gameBoundsScale = gameBoundsScale;
            this.paddedBoundsScale = paddedBoundsScale;
            this.constrainX = constrainX;
            this.constrainY = constrainY;
            this.constrainZ = constrainZ;
        }

        public GameBounds Create(bool usePadding, bool checkOutOfBounds, Vector3 scale)
        {
            return new GameBounds(gameBoundsPosition, usePadding ? paddedBoundsScale : gameBoundsScale,
                checkOutOfBounds ? -scale : scale, constrainX, constrainY, constrainZ);
        }
    }
}