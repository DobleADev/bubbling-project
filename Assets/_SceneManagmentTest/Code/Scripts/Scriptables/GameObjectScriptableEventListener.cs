using UnityEngine;
using DoubleADev.Scriptables;

namespace DoubleADev.ScriptableEvent
{
    [CreateAssetMenu(fileName = "NewGameObjectEvent", menuName = "Scriptable Event/GameObject")]
    internal class GameObjectScriptableEvent : ScriptableEvent<GameObject> {}
    internal class GameObjectScriptableEventListener : ScriptableEventListener<GameObject> {}
}
