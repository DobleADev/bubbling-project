using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterEvent : MonoBehaviour
{
    [SerializeField] string targetTag = "Untagged";
    [SerializeField] UnityEvent onTriggerEnter;
    private void OnTriggerEnter(Collider other) 
    {
        if (!other.CompareTag(targetTag)) return;
        onTriggerEnter?.Invoke();
    }
}
