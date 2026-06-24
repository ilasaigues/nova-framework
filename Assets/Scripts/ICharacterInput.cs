using System;
using UnityEngine;

public interface ICharacterInput
{
    public class InputWrapper<T>
    {
        private T _value;

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value.Equals(_value)) return;
                var oldValue = _value;
                _value = value;
                OnValueChangedNoParams();
                OnValueChanged(value);
                OnValueChangedHistory(value, oldValue);
            }
        }

        public float TimeLastChanged;

        public event Action<T> OnValueChanged = delegate { };
        public event Action<T, T> OnValueChangedHistory = delegate { };
        public event Action OnValueChangedNoParams = delegate { };


    }

    public InputWrapper<bool> JumpWrapper { get; }
    public InputWrapper<bool> DashWrapper { get; }
    public InputWrapper<Vector2> MovementWrapper { get; }


}


