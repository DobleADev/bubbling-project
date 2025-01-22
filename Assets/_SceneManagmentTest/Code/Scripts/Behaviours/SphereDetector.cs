using UnityEngine;
using UnityEngine.Events;

namespace DobleADev
{
    public class SphereDetector : MonoBehaviour
    {
        public LayerMask layerMask;
        public Color activeColor = new Color(1f, 1f, 1f, 1f);
        public Color inactiveColor = new Color(0.4f, 0.4f, 0.4f, 0.5f);
        public float radius = 1;
        [SerializeField] bool checkOnUpdate = true;
        [SerializeField] GameObjectEvent onDetectedCollision;
        [SerializeField] GameObjectsEvent onDetectedCollisions;

        [Header("")]
        public bool debug = false;
        private Collider[] colliders;
        private void Update() {
            if (!checkOnUpdate) return;
            CheckCollision();
        }
        public void _CheckCollision() { CheckCollision(); }

        public bool CheckCollision()
        {
            colliders = Physics.OverlapSphere(
                transform.position,
                radius * transform.lossyScale.magnitude,
                layerMask
            );

            if (colliders.Length > 0)
            {
                GameObject[] checkedGameObjects = ConvertCollidersToGameObjects(colliders);
                // onDetectedCollision.Invoke(GetClosestGameObject(checkedGameObjects));
                if (onDetectedCollision != null) onDetectedCollision.Invoke(GetClosestGameObject(checkedGameObjects));
                if (onDetectedCollisions != null) onDetectedCollisions.Invoke(checkedGameObjects);

                return true;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            if (debug)
            {
                if (colliders == null) Gizmos.color = inactiveColor;
                else Gizmos.color = colliders.Length > 0 ? activeColor : inactiveColor;
                Gizmos.DrawWireSphere(transform.position, radius * transform.lossyScale.magnitude);
            }
        }

        GameObject GetClosestGameObject(GameObject[] gameObjects)
        {
            GameObject bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            for (int i = 0; i < gameObjects.Length; i++)
            {
                Vector3 directionToTarget = gameObjects[i].transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if(dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = gameObjects[i];
                }
            }
    
            return bestTarget;
        }

        GameObject[] ConvertCollidersToGameObjects(Collider[] colliders)
        {
            int colliderCount = colliders.Length;
            GameObject[] gameObjectsList = new GameObject[colliderCount];
            for(int i = 0; i < colliderCount; i++)
            {
                gameObjectsList[i] = colliders[i].transform.gameObject;
            }
            return gameObjectsList;
        }
    }
}
