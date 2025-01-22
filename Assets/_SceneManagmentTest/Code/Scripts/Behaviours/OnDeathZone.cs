using UnityEngine;
using UnityEngine.Events;

public class OnDeathZone : MonoBehaviour
{
    [SerializeField] string deathZoneTag = "DeathZone";
    [SerializeField] UnityEvent onEnterDeathZone;
    
    private void OnTriggerEnter(Collider other) 
    {
        if (!other.CompareTag(deathZoneTag)) return;

        onEnterDeathZone?.Invoke();
    }
}
