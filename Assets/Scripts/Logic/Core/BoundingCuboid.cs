namespace Logic.Core
{
    public struct BoundingCuboid
    {
        public readonly float MinX;
        public readonly float MaxX;
        public readonly float MinY;
        public readonly float MaxY;
        public readonly float MinZ;
        public readonly float MaxZ;

        public BoundingCuboid(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
            MinZ = minZ;
            MaxZ = maxZ;
        }
    }
}