using UnityEngine;
using UnityEngine.Events;

public class OxygenGain : MonoBehaviour
{
    [SerializeField] string _playerTag = "Player";
    [SerializeField] UnityEvent _onGet;
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag(_playerTag))
        {
            if (other.TryGetComponent(out PlayerFacade player))
            {
                player.RechargeOxygen();
                _onGet?.Invoke();
            }
        }    
    }
}
