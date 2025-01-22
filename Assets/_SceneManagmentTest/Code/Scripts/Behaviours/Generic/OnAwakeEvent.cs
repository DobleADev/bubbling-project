using DobleADev.Core;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class OnAwakeEvent : MonoBehaviour
{
    [SerializeField] ExecuteType _execute;
    [SerializeField] UnityEvent _onAwake;
    void Awake()
    {
        #if UNITY_EDITOR
        if (_execute == (ExecuteType.Editor | ExecuteType.Always)) return;
        #else
        if (_execute == (ExecuteType.Runtime | ExecuteType.Always)) return;
        #endif
        _onAwake?.Invoke();
    }

    public void Raise()
    {
        _onAwake?.Invoke();
    }
}
