using System;
using System.Collections.Generic;
using System.Linq;
namespace Core.Utilities
{
    public interface IReadOnlyPropertyMax<T>
    {
        T Value { get; }
        T MaxValue { get; }
        event Action<T, T> OnValueChanged;
        void AddValueChangedListenter(System.Action<T, T> valueChangeListener);
        void RemoveValueCangedListener(System.Action<T, T> valueChangeListener);
    }
    public interface IPropertyMax<T> : IReadOnlyPropertyMax<T>
    {
        void SetMaxValue(T maxValue, bool force, bool notify = true);
        void SetValue(T newValue, bool force, bool notify = true);
    }
    public class CustomPropertyMax<T> : IPropertyMax<T>
    {
        public T Value { get; private set; }

        public T MaxValue { get; private set; }

        public event Action<T, T> OnValueChanged;
        private List<Action<T, T>> _valueChangedListeners;

        private Func<T, T, bool> _gratenThenFunction;
        public CustomPropertyMax(T value, T maxValue, Func<T, T, bool> gratenThen)
        {
            Value = value;
            MaxValue = maxValue;
            _gratenThenFunction = gratenThen;

            if (_gratenThenFunction(Value, MaxValue) == true)
            {
                Value = MaxValue;
            }

            _valueChangedListeners = new List<Action<T, T>>();
        }

        public void SetValue(T newValue, bool force, bool notify = true)
        {
            if (object.Equals(Value, newValue) == false || force == true)
            {
                Value = newValue;
            }

            if (notify == true)
            {
                if (_gratenThenFunction(Value, MaxValue) == true)
                {
                    Value = MaxValue;
                }

                OnValueChanged?.Invoke(Value, MaxValue);
                NotifyListeners();
            }
        }

        public void AddValueChangedListenter(Action<T, T> valueChangeListener)
        {
            _valueChangedListeners.Add(valueChangeListener);
            VaidateListeners();
        }

        public void RemoveValueCangedListener(Action<T, T> valueChangeListener)
        {
            _valueChangedListeners.Remove(valueChangeListener);
            VaidateListeners();
        }

        private void NotifyListeners()
        {
            _valueChangedListeners.ForEach(listener => listener.Invoke(Value, MaxValue));
        }

        private void VaidateListeners()
        {
            var counter = 0;
            while (counter < _valueChangedListeners.Count)
            {
                var listener = _valueChangedListeners[counter];
                if (listener == null)
                {
                    _valueChangedListeners.Remove(listener);
                }
                else
                {
                    counter++;
                }
            }
        }

        public void SetMaxValue(T maxValue, bool force, bool notify = true)
        {
            if (object.Equals(MaxValue, maxValue) == false || force == true)
            {
                MaxValue = maxValue;
            }

            if (notify == true)
            {
                OnValueChanged?.Invoke(Value, MaxValue);
                NotifyListeners();
            }
        }
    }


    public interface IFloatReadOnlyPropertyMax : IReadOnlyPropertyMax<float>
    { }

    public interface IFloatPropertyMax : IFloatReadOnlyPropertyMax, IPropertyMax<float>
    { }


    public class FloatPropertyMax : CustomPropertyMax<float>, IFloatPropertyMax
    {
        public FloatPropertyMax(float value, float maxValue, Func<float, float, bool> gratenThen) : base(value, maxValue, gratenThen)
        {
        }
    }

    public interface IIntReadOnlyPropertyMax : IReadOnlyPropertyMax<int>
    { }

    public interface IIntPropertyMax : IIntReadOnlyPropertyMax, IPropertyMax<int>
    { }

    public class IntPropertyMax : CustomPropertyMax<int>, IIntPropertyMax
    {
        public IntPropertyMax(int value, int maxValue, Func<int, int, bool> gratenThen) : base(value, maxValue, gratenThen)
        {

        }
    }

    public interface IReadOnlyProperty<T>
    {
        T Value { get; }
        event Action<T> OnValueChanged;
        void AddValueChangedListenter(System.Action<T> valueChangeListener, bool notify = false);
        void RemoveValueCangedListener(System.Action<T> valueChangeListener);
    }

    public interface IProperty<T> : IReadOnlyProperty<T>
    {
        void SetValue(T newValue, bool force);
    }

    public interface IIntReadOnlyProperty : IReadOnlyProperty<int>
    {

    }

    public interface IIntProperty : IIntReadOnlyProperty, IProperty<int>
    {
        void Plus(int plusValue);
        void Minus(int minusValue);
    }

    public interface ICustomListProperty<T>
    {
        void Add(T value);
    }

    public class CustomListProperty<T> : CustomProperty<List<T>>, ICustomListProperty<T>
    {
        public CustomListProperty(List<T> value) : base(value) { }

        public void Add(T newElement)
        {
            Value.Add(newElement);
            SetValue(Value);
        }
    }

    public class CustomProperty<T> : IProperty<T>
    {
        public T Value { get; private set; }
        public event Action<T> OnValueChanged;
        private List<Action<T>> _valueChangedListeners;

        public CustomProperty(T value)
        {
            Value = value;
            _valueChangedListeners = new List<Action<T>>();
        }

        public void SetValue(T newValue, bool force = true)
        {
            if (object.Equals(Value, newValue) == false || force == true)
            {
                Value = newValue;
                OnValueChanged?.Invoke(Value);
                NotifyListeners();
            }
        }

        public void AddValueChangedListenter(Action<T> valueChangeListener, bool notify = false)
        {
            _valueChangedListeners.Add(valueChangeListener);
            ValidateListeners();

            if (notify == true)
            {
                OnValueChanged?.Invoke(Value);
                NotifyListeners();
            }
        }

        public void RemoveValueCangedListener(Action<T> valueChangeListener)
        {
            _valueChangedListeners.Remove(valueChangeListener);
            ValidateListeners();
        }

        public void RemoveAllListeners()
        {
            _valueChangedListeners.Clear();
            ValidateListeners();
        }

        private void NotifyListeners()
        {
            _valueChangedListeners.ToList().ForEach(listener => listener?.Invoke(Value));
        }

        private void ValidateListeners()
        {
            var counter = 0;
            while (counter < _valueChangedListeners.Count)
            {
                var listener = _valueChangedListeners[counter];
                if (listener == null)
                {
                    _valueChangedListeners.Remove(listener);
                }
                else
                {
                    counter++;
                }
            }
        }
    }
    public interface IBoolReadOnlyProperty : IReadOnlyProperty<bool>
    { }

    public interface IBoolProperty : IBoolReadOnlyProperty, IProperty<bool>
    { }

    public class BoolProperty : CustomProperty<bool>, IBoolProperty
    {
        public BoolProperty(bool value) : base(value)
        {
        }
    }

    public class IntProperty : CustomProperty<int>, IIntProperty
    {
        public IntProperty(int value) : base(value)
        {
        }

        public void Plus(int plusValue)
        {
            SetValue(Value + plusValue);
        }

        public void Minus(int minusValue)
        {
            SetValue(Value - minusValue);
        }
    }

    public interface IFloatReadOnlyProperty : IReadOnlyProperty<float>
    {

    }

    public interface IFloatProperty : IFloatReadOnlyProperty, IProperty<float>
    {
        void Plus(float plusValue);
        void Minus(float minusValue);
    }

    public class FloatProperty : CustomProperty<float>, IFloatProperty
    {
        public FloatProperty(float value) : base(value)
        {
        }

        public void Plus(float plusValue)
        {
            SetValue(Value + plusValue);
        }

        public void Minus(float minusValue)
        {
            SetValue(Value - minusValue);
        }
    }

    public class ListProperty<T>
    {
        public T Value { get; private set; }
        public List<T> ListValue { get; private set; }
        private List<Action<T>> _valueChangedListeners;

        public ListProperty(List<T> value)
        {
            ListValue = value;
            _valueChangedListeners = new List<Action<T>>();
        }

        public void SetList(List<T> newValue)
        {
            ListValue = newValue;
        }

        public void AddValue(T newValue)
        {
            Value = newValue;
            ListValue.Add(newValue);
            NotifyListeners();
        }

        public void RemoveValue(T newValue)
        {
            Value = newValue;
            ListValue.Remove(newValue);
            NotifyListeners();
        }

        public void AddValueChangedListenter(Action<T> valueChangeListener, bool notify = false)
        {
            _valueChangedListeners.Add(valueChangeListener);
            ValidateListeners();

            if (notify == true)
            {
                NotifyListeners();
            }
        }

        public void RemoveValueCangedListener(Action<T> valueChangeListener)
        {
            _valueChangedListeners.Remove(valueChangeListener);
            ValidateListeners();
        }

        public void RemoveAllListeners()
        {
            _valueChangedListeners.Clear();
            ValidateListeners();
        }

        private void NotifyListeners()
        {
            _valueChangedListeners.ToList().ForEach(listener => listener?.Invoke(Value));
        }

        private void ValidateListeners()
        {
            var counter = 0;
            while (counter < _valueChangedListeners.Count)
            {
                var listener = _valueChangedListeners[counter];
                if (listener == null)
                {
                    _valueChangedListeners.Remove(listener);
                }
                else
                {
                    counter++;
                }
            }
        }
    }

}
