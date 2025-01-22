using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DoubleADev.ScriptableEvent
{
    internal class CheckpointDataScriptableEventListener : MonoBehaviour
    {
        [SerializeField] private CheckpointDataScriptableEvent Event;
        [SerializeField, Min(0)] private float executionDelay;
        [SerializeField] private CheckpointDataEvent actions;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(CheckpointData action)
        {
            StartCoroutine(ExecuteEvent(action));
        }

        IEnumerator ExecuteEvent(CheckpointData action)
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
    public class CheckpointDataEvent : UnityEvent<CheckpointData> {}
}