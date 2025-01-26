using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DoubleADev.Scriptables.Events
{
    public class GameObjectScriptableEventListener : MonoBehaviour
    {
        [SerializeField] private GameObjectScriptableEvent Event;
        [SerializeField, Min(0)] private float executionDelay;
        [SerializeField] private GameObjectEvent actions;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(GameObject action)
        {
            StartCoroutine(ExecuteEvent(action));
        }

        IEnumerator ExecuteEvent(GameObject action)
        {
            if (executionDelay > 0) yield return new WaitForSecondsRealtime(executionDelay);
            actions.Invoke(action);
        }

        // public override string ToString()
        // {
        //     return gameObject.name;
        // }
    }

    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> {}
}