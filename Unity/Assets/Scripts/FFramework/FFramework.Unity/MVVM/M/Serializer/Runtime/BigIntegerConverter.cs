using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework.MVVM
{
    using System;
    using System.Numerics;

    public class BigIntegerConverter : ISerializeConverter
    {
        public object DeSerialize(string str, Type t=null)
        {
            if (BigInteger.TryParse(str, out BigInteger result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public string Serialize(object obj)
        {
            if (obj is BigInteger bigIntegerValue)
            {
                return bigIntegerValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }



    public class IntConverter : ISerializeConverter
    {
        public object DeSerialize(string str, Type t = null)
        {
            if (int.TryParse(str, out int result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public string Serialize(object obj)
        {
            if (obj is int intValue)
            {
                return intValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public class LongConverter : ISerializeConverter
    {
        public object DeSerialize(string str, Type t = null)
        {
            if (long.TryParse(str, out long result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public string Serialize(object obj)
        {
            if (obj is long longValue)
            {
                return longValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public class UIntConverter : ISerializeConverter
    {
        public object DeSerialize(string str, Type t = null)
        {
            if (uint.TryParse(str, out uint result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public string Serialize(object obj)
        {
            if (obj is uint uintValue)
            {
                return uintValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public class ULongConverter : ISerializeConverter
    {
        public object DeSerialize(string str, Type t = null)
        {
            if (ulong.TryParse(str, out ulong result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public string Serialize(object obj)
        {
            if (obj is ulong ulongValue)
            {
                return ulongValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public class ByteConverter : ISerializeConverter
    {
        public object DeSerialize(string str, Type t = null)
        {
            if (byte.TryParse(str, out byte result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public string Serialize(object obj)
        {
            if (obj is byte byteValue)
            {
                return byteValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public class SByteConverter : ISerializeConverter
    {
        public object DeSerialize(string str, Type t = null)
        {
            if (sbyte.TryParse(str, out sbyte result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public string Serialize(object obj)
        {
            if (obj is sbyte sbyteValue)
            {
                return sbyteValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}