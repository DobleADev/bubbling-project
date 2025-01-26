using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DoubleADev.Scriptables.Events
{
    public class FloatScriptableEventListener : MonoBehaviour
    {
        [SerializeField] private FloatScriptableEvent Event;
        [SerializeField, Min(0)] private float executionDelay;
        [SerializeField] private FloatEvent actions;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(float action)
        {
            StartCoroutine(ExecuteEvent(action));
        }

        IEnumerator ExecuteEvent(float action)
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
    public class FloatEvent : UnityEvent<float> {}
}