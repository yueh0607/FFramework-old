using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace FFramework
{


    public static class SHA64
    {
        private readonly static SHA256 sha256 = SHA256.Create();
        private static Dictionary<Type, long> hashCache = new Dictionary<Type, long>();

        public static long GetHash<T>()
        {
            return GetHash(typeof(T));
        }
        public static long GetHash(Type type)
        {
            if (!hashCache.ContainsKey(type))
            {
                hashCache.Add(type, BitConverter.ToInt64(
                    sha256.ComputeHash(
                        Encoding.UTF8.GetBytes(type.FullName)), 0));
            }
            return hashCache[type];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long CalStringHash(string str)
        {
            return BitConverter.ToInt64(sha256.ComputeHash(Encoding.UTF8.GetBytes(str)), 0);
        }



    }
}