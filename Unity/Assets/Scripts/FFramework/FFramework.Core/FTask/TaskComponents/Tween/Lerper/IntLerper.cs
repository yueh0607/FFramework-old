using System;

namespace FFramework
{
    public class IntLerper : Singleton<FloatLerper>, ILerper<int>
    {
        public int Lerp(int start, int end, float t)
        {
            return (int)((end - start) * t + start);
        }

        public int LerpClamp(int start, int end, float t)
        {
            int lerpedValue = Lerp(start, end, t);
            return Math.Clamp(lerpedValue, start, end);
        }

        public bool ValueEqual(int v1, int v2)
        {
            return v1 == v2;
        }
    }
}
