using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoubleADev.Scriptables.Events
{
    [CreateAssetMenu(fileName = "NewVector3Event", menuName = "Scriptable Event/Vector3")]
    public class Vector3ScriptableEvent : ScriptableObject
    {
        private List<Vector3ScriptableEventListener> listeners = new List<Vector3ScriptableEventListener>();

        public void Raise(Vector3 param1)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(param1);
            }
        }

        public void RegisterListener(Vector3ScriptableEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(Vector3ScriptableEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
