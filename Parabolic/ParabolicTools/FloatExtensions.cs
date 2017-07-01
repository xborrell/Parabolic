using System;

namespace Parabolic
{
    public static class FloatExtensions
    {
        public static bool AreEqualApproximately(this float left, float right, float epsilon)
        {
            return Math.Abs(right - left) <= epsilon;
        }
    }
}
