using DobleADev.Core;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class OnStartEvent : MonoBehaviour
{
    [SerializeField] ExecuteType _execute;
    [SerializeField] UnityEvent _onStart;
    void Start()
    {
        #if UNITY_EDITOR
        if (_execute == (ExecuteType.Editor | ExecuteType.Always)) return;
        #else
        if (_execute == (ExecuteType.Runtime | ExecuteType.Always)) return;
        #endif
        _onStart?.Invoke();
    }

    public void Raise()
    {
        _onStart?.Invoke();
    }
}
