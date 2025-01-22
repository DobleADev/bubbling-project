using DobleADev.Scriptables.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace DobleADev.Behaviours.Branches
{
    [ExecuteAlways]
    public class BoolBranch : MonoBehaviour
    {
        [SerializeField] BoolScriptableVariable _value;
        [SerializeField] UnityEvent _onTrue;
        [SerializeField] UnityEvent _onFalse;

        public void Evaluate()
        {
            if (_value.GetValueTyped() == true) _onTrue?.Invoke();
            else _onFalse?.Invoke();
        }
    }
}