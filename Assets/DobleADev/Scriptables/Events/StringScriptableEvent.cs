using System.Collections.Generic;
using UnityEngine;

namespace DoubleADev.Scriptables.Events
{
    [CreateAssetMenu(fileName = "NewStringEvent", menuName = "Scriptable Event/String")]
    public class StringScriptableEvent : ScriptableObject
    {
        private List<StringScriptableEventListener> listeners = new List<StringScriptableEventListener>();

        public void Raise(string param1)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(param1);
            }
        }

        public void RegisterListener(StringScriptableEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(StringScriptableEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
