using System.Collections.Generic;
using DoubleADev.ScriptableEvent;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSceneContainerEvent", menuName = "Scriptable Event/Scene Container")]
internal class SceneContainerScriptableEvent : ScriptableObject
{
    private List<SceneContainerScriptableEventListener> listeners = new List<SceneContainerScriptableEventListener>();

    public void Raise(SceneContainer param1)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(param1);
        }
    }

    public void RegisterListener(SceneContainerScriptableEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(SceneContainerScriptableEventListener listener)
    {
        listeners.Remove(listener);
    }
}
