using UnityEngine;
using UnityEngine.Events;

public class DisableObject : MonoBehaviour
{
    [SerializeField] private float _disableDelay = 0.1f;
        [SerializeField] private UnityEvent OnDisable;
        public void DisableInmediate()
        {
            OnDisable?.Invoke();
            gameObject.SetActive(false);
        }
        public void DisableWithDelay()
        {
            Invoke("DisableInmediate", _disableDelay);
        }
}
