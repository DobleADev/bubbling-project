using DobleADev.Core;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionEnter2DEvent : MonoBehaviour
{
    [SerializeField] string targetTag = "Untagged";
    [SerializeField] ExecuteType _execute;
    [SerializeField] UnityEvent _onCollisionEnter2D;
    private void OnCollisionEnter2D(Collision2D other) 
    {
        #if UNITY_EDITOR
        if (_execute == (ExecuteType.Editor | ExecuteType.Always)) return;
        #else
        if (_execute == (ExecuteType.Runtime | ExecuteType.Always)) return;
        #endif
        if (!other.gameObject.CompareTag(targetTag)) return;
        Raise();
    }

    public void Raise()
    {
        _onCollisionEnter2D?.Invoke();
    }
}
