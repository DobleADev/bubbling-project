using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DobleADev.Scriptables
{
    [Serializable]
    public abstract class ScriptableVariableBase : ScriptableObject
    {
        public abstract object GetValue();
        public abstract void SetValue(object value);
        public abstract Type GetVariableType();
    }

    public abstract class ScriptableVariable<T> : ScriptableVariableBase
    {
        [SerializeField] private T _value;

        public override object GetValue() => _value;
        public T GetValueTyped()
        {
            return _value;
        }

        public override void SetValue(object value)
        {
            if (value is T typedValue)
                _value = typedValue;
            else
                Debug.LogError($"Tipo incorrecto asignado a {name}. Se esperaba {typeof(T).Name}, se recibió {value?.GetType().Name ?? "null"}");
        }
        public void SetValueTyped(T value)
        {
            SetValue(value);
        }

        public override Type GetVariableType() => typeof(T);

        public static implicit operator T(ScriptableVariable<T> variable) => (T)variable.GetValue();
    }
}
