using System.Collections.Generic;
using UnityEngine;

namespace DoubleADev.Scriptables.Events
{
    [CreateAssetMenu(fileName = "NewFloatEvent", menuName = "Scriptable Event/Float")]
    public class FloatScriptableEvent : ScriptableObject
    {
        private List<FloatScriptableEventListener> listeners = new List<FloatScriptableEventListener>();

        public void Raise(float param1)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(param1);
            }
        }

        public void RegisterListener(FloatScriptableEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(FloatScriptableEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
