namespace Logic.Core
{
    public struct InputParams
    {
        public readonly float Horizontal;
        public readonly bool Fire;

        public InputParams(float horizontal, bool fire)
        {
            Horizontal = horizontal;
            Fire = fire;
        }
    }
}