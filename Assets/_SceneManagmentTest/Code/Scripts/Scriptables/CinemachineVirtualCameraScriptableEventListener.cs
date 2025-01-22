using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace DoubleADev.ScriptableEvent
{
    internal class CinemachineVirtualCameraScriptableEventListener : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCameraScriptableEvent Event;
        [SerializeField, Min(0)] private float executionDelay;
        [SerializeField] private CinemachineVirtualCameraEvent actions;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(CinemachineVirtualCamera action)
        {
            StartCoroutine(ExecuteEvent(action));
        }

        IEnumerator ExecuteEvent(CinemachineVirtualCamera action)
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
    public class CinemachineVirtualCameraEvent : UnityEvent<CinemachineVirtualCamera> {}
}
