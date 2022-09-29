using System;

namespace Util.Extensions
{
    public static class FloatExtensions
    {
        public static bool Approximately(this float a, float b)
        {
            return Math.Abs(a - b) <= float.Epsilon;
        }
    }
}