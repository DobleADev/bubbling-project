using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DoubleADev.Scriptables.Events
{
    public class Vector2ScriptableEventListener : MonoBehaviour
    {
        [SerializeField] private Vector2ScriptableEvent Event;
        [SerializeField, Min(0)] private float executionDelay;
        [SerializeField] private Vector2Event actions;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(Vector2 action)
        {
            StartCoroutine(ExecuteEvent(action));
        }

        IEnumerator ExecuteEvent(Vector2 action)
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
    public class Vector2Event : UnityEvent<Vector2> {}
}