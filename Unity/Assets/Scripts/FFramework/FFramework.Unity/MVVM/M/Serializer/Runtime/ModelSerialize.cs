using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;



namespace FFramework.MVVM
{
    public interface ISerializer
    {
        string Serialize(object obj);
    }

    public interface IDeSerializer
    {
        object DeSerialize(string str,Type type=null);
    }

    public interface ISerializeConverter : ISerializer,IDeSerializer
    {

    }

    public class ModelSerialize
    {
        public static string Serialize(object obj)
        {
            if (obj is int intValue)
            {
                return SingletonProperty<IntConverter>.Instance.Serialize(intValue);
            }
            else if (obj is long longValue)
            {
                return SingletonProperty<LongConverter>.Instance.Serialize(longValue);
            }
            else if (obj is uint uintValue)
            {
                return SingletonProperty<UIntConverter>.Instance.Serialize(uintValue);
            }
            else if (obj is ulong ulongValue)
            {
                return SingletonProperty<ULongConverter>.Instance.Serialize(ulongValue);
            }
            else if (obj is byte byteValue)
            {
                return SingletonProperty<ByteConverter>.Instance.Serialize(byteValue);
            }
            else if (obj is sbyte sbyteValue)
            {
                return SingletonProperty<SByteConverter>.Instance.Serialize(sbyteValue);
            }
            else if(obj is BigInteger big)
            {
                return SingletonProperty<BigIntegerConverter>.Instance.Serialize(big);
            }


            if (obj is double)
            {
                return SingletonProperty<DoubleConverter>.Instance.Serialize(obj);
            }
            else if(obj is float)
            {
                return SingletonProperty<FloatConverter>.Instance.Serialize(obj);
            }


            //date time
            if (typeof(DateTime) == obj.GetType())
            {
                return SingletonProperty<DateTimeConverter>.Instance.Serialize(obj);
            }
            //string
            if (typeof(string) == obj.GetType())
            {
                return SingletonProperty<StringConverter>.Instance.Serialize(obj);
            }

            //是BindableProperty
            if (obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition().Name.Split('`')[0] == "BindableProperty")
            {
                return SingletonProperty<BindableConverter>.Instance.Serialize(obj);
            }

            throw new InvalidCastException($"Unsupport type:{obj.GetType().FullName}");
        }

        public static object DeSerialize(string str,Type type)
        {
            if (typeof(int) == type)
            {
                return SingletonProperty<IntConverter>.Instance.DeSerialize(str);
            }
            else if (typeof(long) == type)
            {
                return SingletonProperty<LongConverter>.Instance.DeSerialize(str);
            }
            else if (typeof(uint) == type)
            {
                return SingletonProperty<UIntConverter>.Instance.DeSerialize(str);
            }
            else if (typeof(ulong) == type)
            {
                return SingletonProperty<ULongConverter>.Instance.DeSerialize(str);
            }
            else if (typeof(byte) == type)
            {
                return SingletonProperty<ByteConverter>.Instance.DeSerialize(str);
            }
            else if (typeof(sbyte) == type)
            {
                return SingletonProperty<SByteConverter>.Instance.DeSerialize(str);
            }
            else if (typeof(BigInteger) == type)
            {
                return SingletonProperty<BigIntegerConverter>.Instance.DeSerialize(str);
            }
            else if (typeof(double) == type)
            {
                return SingletonProperty<DoubleConverter>.Instance.DeSerialize(str);
            }
            else if (typeof(float) == type)
            {
                return SingletonProperty<FloatConverter>.Instance.DeSerialize(str);
            }
            else if (typeof(DateTime) == type)
            {
                return SingletonProperty<DateTimeConverter>.Instance.DeSerialize(str);
            }
            else if (typeof(string) == type)
            {
                return SingletonProperty<StringConverter>.Instance.DeSerialize(str);
            }

            //是BindableProperty
            if (type.IsGenericType && type.GetGenericTypeDefinition().Name.Split('`')[0] == "BindableProperty")
            {
                return SingletonProperty<BindableConverter>.Instance.DeSerialize(str,type);
            }

            throw new InvalidCastException($"Unsupport type:{type.FullName}");
        }
    }
}