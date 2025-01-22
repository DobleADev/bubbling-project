using DobleADev.Scriptables.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace DobleADev.Behaviours.Branches
{
    [ExecuteAlways]
    public class IntBranch : MonoBehaviour
    {
        [SerializeField] IntScriptableVariable _a;
        public enum Comparison { Equal, GreaterThan, LessThan, NotEqual }
        [SerializeField] Comparison _condition;
        [SerializeField] int _b;
        [SerializeField] UnityEvent _onTrue;
        [SerializeField] UnityEvent _onFalse;

        public void Evaluate()
        {
            if (_a.GetValueTyped() == _b) _onTrue?.Invoke();
            else _onFalse?.Invoke();
        }
    }
}