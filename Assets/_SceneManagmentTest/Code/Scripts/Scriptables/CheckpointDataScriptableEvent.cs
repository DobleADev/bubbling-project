
using System.Collections.Generic;
using DoubleADev.ScriptableEvent;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCheckpointDataEvent", menuName = "Scriptable Event/CheckpointData")]
    internal class CheckpointDataScriptableEvent : ScriptableObject
    {
        private List<CheckpointDataScriptableEventListener> listeners = new List<CheckpointDataScriptableEventListener>();

        public void Raise(CheckpointData param1)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(param1);
            }
        }

        public void RegisterListener(CheckpointDataScriptableEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(CheckpointDataScriptableEventListener listener)
        {
            listeners.Remove(listener);
        }
    }