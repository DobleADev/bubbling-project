using UnityEngine;

namespace APROMASTER
{
    public class InstantiateObject : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField, Range(0, 1)] private float chance = 1;

        public void Instantiate()
        {
            if (Random.value < chance)
            {
                Instantiate(prefab, transform.position, transform.rotation);
            }
        }

        public void InstantiateOnPosition(Vector3 position)
        {
            if (Random.value < chance)
            {
                Instantiate(prefab, position, transform.rotation);
            }
        }
    }
}
