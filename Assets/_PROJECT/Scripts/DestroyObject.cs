using UnityEngine;
using UnityEngine.Events;

namespace APROMASTER
{
    public class DestroyObject : MonoBehaviour
    {
        [SerializeField] private float _destroyDelay = 0.1f;
        [SerializeField] private UnityEvent OnDestroy;
        public void DestroyInmediate()
        {
            OnDestroy?.Invoke();
            Destroy(gameObject);
        }
        public void DestroyWithDelay()
        {
            Invoke("DestroyInmediate", _destroyDelay);
        }
    }
}
