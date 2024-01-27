using System;

namespace FFramework
{
    public class LongLerper : Singleton<LongLerper>, ILerper<long>
    {
        public long Lerp(long start, long end, float t)
        {
            return (long)((end - start) * t + start);
        }

        public long LerpClamp(long start, long end, float t)
        {
            long lerpedValue = Lerp(start, end, t);
            return Math.Clamp(lerpedValue, Math.Min(start, end), Math.Max(start, end));
        }

        public bool ValueEqual(long v1, long v2)
        {
            return v1 == v2;
        }
    }
}
