﻿using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace FFramework.MVVM
{

    public interface IBindablePropertyNotify
    {
        void Notify(object oldValue);

        void Notify();
    }

    public class BindableProperty<T>:IBindablePropertyNotify where T : IEquatable<T>
    {
        private readonly object bindObject = null;

        private T _value = default;
        private readonly Func<object ,T> _getter = null;
        private readonly Action<object ,T> _setter = null;

        private readonly bool mode = true;
        public T Value
        {
            get
            {
                if (mode)
                {
                    return _getter(bindObject);
                }
                return _value;
            }
            set
            {
                if (_value.Equals(value)) return;
                T oldValue = _value;
                if (mode)
                {
                    _setter(bindObject,value);
                }
                _value = value;
                onPropertyChanged?.Invoke(oldValue, _value);
            }
        }

        private Action<T, T> onPropertyChanged = null;
        public event Action<T, T> OnPropertyChanged
        {
            add
            {
                value?.Invoke(_value, _value);
                onPropertyChanged += value;
            }
            remove
            {
                onPropertyChanged-=value;
            }
        }

        public BindableProperty(object bindObject ,Func<object,T> getter, Action<object,T> setter)
        {
            mode = true;
            this.bindObject = bindObject;
            _getter = getter;
            _setter = setter;
        }
        public BindableProperty(T initValue)
        {
            mode = false;
            _value = initValue;
        }

        private BindableProperty() { mode = false; }

        T oldValue;
        public void ListenChanged(float deltaTime)
        {
            _value = _getter(bindObject);
            if (!oldValue.Equals(_value))
            {
                onPropertyChanged?.Invoke(oldValue, _value);
            }
            oldValue = _value;
        }


        public void Notify(object oldValue)
        {
            onPropertyChanged?.Invoke((T)oldValue, _value);
        }
        public void Notify()
        {
            onPropertyChanged?.Invoke(_value, _value);
        }

    }
}