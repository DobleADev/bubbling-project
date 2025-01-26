using System.Collections.Generic;
using UnityEngine;

namespace DoubleADev.Scriptables.Events
{
    [CreateAssetMenu(fileName = "NewGameObjectEvent", menuName = "Scriptable Event/GameObject")]
    public class GameObjectScriptableEvent : ScriptableObject
    {
        private List<GameObjectScriptableEventListener> listeners = new List<GameObjectScriptableEventListener>();

        public void Raise(GameObject param1)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(param1);
            }
        }

        public void RegisterListener(GameObjectScriptableEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(GameObjectScriptableEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
