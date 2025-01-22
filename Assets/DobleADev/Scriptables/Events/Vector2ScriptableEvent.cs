using System.Collections.Generic;
using UnityEngine;

namespace DoubleADev.Scriptables.Events
{
    [CreateAssetMenu(fileName = "NewVector2Event", menuName = "Scriptable Event/Vector2")]
    public class Vector2ScriptableEvent : ScriptableObject
    {
        private List<Vector2ScriptableEventListener> listeners = new List<Vector2ScriptableEventListener>();

        public void Raise(Vector2 param1)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(param1);
            }
        }

        public void RegisterListener(Vector2ScriptableEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(Vector2ScriptableEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
