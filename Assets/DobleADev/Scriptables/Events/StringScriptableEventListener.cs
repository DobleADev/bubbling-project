using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DoubleADev.Scriptables.Events
{
    public class StringScriptableEventListener : MonoBehaviour
    {
        [SerializeField] private StringScriptableEvent Event;
        [SerializeField, Min(0)] private float executionDelay;
        [SerializeField] private StringEvent actions;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(string action)
        {
            StartCoroutine(ExecuteEvent(action));
        }

        IEnumerator ExecuteEvent(string action)
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
    public class StringEvent : UnityEvent<string> {}
}