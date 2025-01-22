using System.Collections.Generic;
using UnityEngine;

namespace DoubleADev.Scriptables.Events
{
    [CreateAssetMenu(fileName = "NewIntEvent", menuName = "Scriptable Event/Int")]
    public class IntScriptableEvent : ScriptableObject
    {
        private List<IntScriptableEventListener> listeners = new List<IntScriptableEventListener>();

        public void Raise(int param1)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(param1);
            }
        }

        public void RegisterListener(IntScriptableEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(IntScriptableEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
