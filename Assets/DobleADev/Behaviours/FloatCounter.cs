using DobleADev.Scriptables.Variables;
using DoubleADev.Scriptables.Events;
using UnityEngine;

public class FloatCounter : MonoBehaviour
{
    [SerializeField] FloatScriptableVariable _floateger;
    [SerializeField] FloatEvent _onChange;
    
    public void Add(float add)
    {
        _floateger.SetValueTyped(_floateger.GetValueTyped() + add);
        _onChange?.Invoke(_floateger.GetValueTyped());
    }

    public void Subtract(float subtract)
    {
        _floateger.SetValueTyped(_floateger.GetValueTyped() - subtract);
        _onChange?.Invoke(_floateger.GetValueTyped());
    }

    public void Multiply(float multiply)
    {
        _floateger.SetValueTyped(_floateger.GetValueTyped() * multiply);
        _onChange?.Invoke(_floateger.GetValueTyped());
    }

    public void Divide(float divide)
    {
        if (divide == 0)
        {
            Debug.LogWarning("Fail - cant divide by zero");
            return;
        }
        _floateger.SetValueTyped(_floateger.GetValueTyped() / divide);
        _onChange?.Invoke(_floateger.GetValueTyped());
    }
}
