using System;

namespace FFramework
{
    public class DoubleLerper : Singleton<DoubleLerper>, ILerper<double>
    {
        public double Lerp(double start, double end, float t)
        {
            return (end - start) * t + start;
        }

        public double LerpClamp(double start, double end, float t)
        {
            double lerpedValue = Lerp(start, end, t);
            return Math.Clamp(lerpedValue, Math.Min(start, end), Math.Max(start, end));
        }

        public bool ValueEqual(double v1, double v2)
        {
            return Math.Abs(v1 - v2) < double.Epsilon; // Comparing doubles with an epsilon for precision
        }
    }
}
