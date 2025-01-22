using DobleADev.Scriptables.Variables;
using DoubleADev.Scriptables.Events;
using UnityEngine;

public class IntCounter : MonoBehaviour
{
    [SerializeField] IntScriptableVariable _integer;
    [SerializeField] IntEvent _onChange;
    
    public void Add(int add)
    {
        _integer.SetValueTyped(_integer.GetValueTyped() + add);
        _onChange?.Invoke(_integer.GetValueTyped());
    }

    public void Subtract(int subtract)
    {
        _integer.SetValueTyped(_integer.GetValueTyped() - subtract);
        _onChange?.Invoke(_integer.GetValueTyped());
    }

    public void Multiply(int multiply)
    {
        _integer.SetValueTyped(_integer.GetValueTyped() * multiply);
        _onChange?.Invoke(_integer.GetValueTyped());
    }

    public void Divide(int divide)
    {
        if (divide == 0)
        {
            Debug.LogWarning("Fail - cant divide by zero");
            return;
        }
        _integer.SetValueTyped(_integer.GetValueTyped() / divide);
        _onChange?.Invoke(_integer.GetValueTyped());
    }
}
