using System;

namespace FFramework
{
    public class FloatLerper : Singleton<FloatLerper>, ILerper<float>
    {
        public float Lerp(float start, float end, float t)
        {
            return (end - start) * t + start;
        }

        public float LerpClamp(float start, float end, float t)
        {
            return Math.Clamp(Lerp(start, end, t), start, end);
        }

        public bool ValueEqual(float v1, float v2)
        {
            return Math.Abs(v1 - v2) < float.Epsilon; // Comparing floats with an epsilon for precision
        }
    }
}
