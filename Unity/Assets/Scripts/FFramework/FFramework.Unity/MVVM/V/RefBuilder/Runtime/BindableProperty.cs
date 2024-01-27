using System;

namespace FFramework.MVVM
{
    public class BindableProperty<T> where T : IEquatable<T>
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
                OnPropertyChanged?.Invoke(oldValue, _value);
            }
        }

        public event Action<T, T> OnPropertyChanged = null;

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

        T oldValue;
        public void ListenChanged(float deltaTime)
        {
            _value = _getter(bindObject);
            if (!oldValue.Equals(_value))
            {
                OnPropertyChanged?.Invoke(oldValue, _value);
            }
            oldValue = _value;
        }

    }
}
