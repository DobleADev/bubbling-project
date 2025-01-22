using DobleADev.Core;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class OnEnableEvent : MonoBehaviour
{
    [SerializeField] ExecuteType _execute;
    [SerializeField] UnityEvent _onEnable;
    void OnEnable()
    {
        #if UNITY_EDITOR
        if (_execute == (ExecuteType.Editor | ExecuteType.Always)) return;
        #else
        if (_execute == (ExecuteType.Runtime | ExecuteType.Always)) return;
        #endif
        Raise();
    }

    public void Raise()
    {
        _onEnable?.Invoke();
    }
}
