using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DoubleADev.Scriptables.Events
{
    public class Vector3ScriptableEventListener : MonoBehaviour
    {
        [SerializeField] private Vector3ScriptableEvent Event;
        [SerializeField, Min(0)] private float executionDelay;
        [SerializeField] private Vector3Event actions;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(Vector3 action)
        {
            StartCoroutine(ExecuteEvent(action));
        }

        IEnumerator ExecuteEvent(Vector3 action)
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
    public class Vector3Event : UnityEvent<Vector3> {}
}