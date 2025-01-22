using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DoubleADev.ScriptableEvent
{
    internal class SceneContainerScriptableEventListener : MonoBehaviour
    {
        [SerializeField] private SceneContainerScriptableEvent Event;
        [SerializeField, Min(0)] private float executionDelay;
        [SerializeField] private SceneContainerEvent actions;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(SceneContainer action)
        {
            StartCoroutine(ExecuteEvent(action));
        }

        IEnumerator ExecuteEvent(SceneContainer action)
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
    public class SceneContainerEvent : UnityEvent<SceneContainer> {}
}