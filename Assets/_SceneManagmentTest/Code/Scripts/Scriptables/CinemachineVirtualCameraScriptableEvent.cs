using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DoubleADev.ScriptableEvent;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCinemachineVirtualCameraEvent", menuName = "Scriptable Event/Cinemachine Virtual Camera")]
    internal class CinemachineVirtualCameraScriptableEvent : ScriptableObject
    {
        private List<CinemachineVirtualCameraScriptableEventListener> listeners = new List<CinemachineVirtualCameraScriptableEventListener>();

        public void Raise(CinemachineVirtualCamera cinemachineVirtualCamera)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(cinemachineVirtualCamera);
            }
        }

        public void RegisterListener(CinemachineVirtualCameraScriptableEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(CinemachineVirtualCameraScriptableEventListener listener)
        {
            listeners.Remove(listener);
        }
    }