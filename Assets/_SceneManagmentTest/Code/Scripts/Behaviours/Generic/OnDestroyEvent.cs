using DobleADev.Core;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class OnDestroyEvent : MonoBehaviour
{
    [SerializeField] ExecuteType _execute;
    [SerializeField] UnityEvent _onDestroy;
    void OnDestroy()
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
        _onDestroy?.Invoke();
    }
}
