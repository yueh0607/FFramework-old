using System.IO;

namespace FFramework
{
    /// <summary>
    /// Object ID
    /// </summary>
    public class OIDAllocator
    {
        private long currentID = long.MinValue;


        /// <summary>
        /// 获取下一个ID
        /// </summary>
        /// <returns>ID值</returns>
        /// <exception cref="InvalidDataException">(假如long.MaxValue为9,223,372,036,854,775,807   如果每天分配大约100亿个ID，大概约200-300年不重启将耗尽)</exception>
        public long NextID()
        {
            if (currentID == long.MaxValue) throw new InvalidDataException("ID MAX");
            return currentID++;
        }


    }
}
