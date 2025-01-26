using DobleADev.Scriptables.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace DobleADev.Behaviours.Branches
{
    [ExecuteAlways]
    public class FloatBranch : MonoBehaviour
    {
        [SerializeField] FloatScriptableVariable _a;
        public enum Comparison { Equal, GreaterThan, LessThan, NotEqual, GreaterOrEqual, LessOrEqual }
        [SerializeField] Comparison _condition;
        [SerializeField] float _b;
        [SerializeField] UnityEvent _onTrue;
        [SerializeField] UnityEvent _onFalse;

        public void Evaluate()
        {
            if (isConditionMet(_a.GetValueTyped())) _onTrue?.Invoke();
            else _onFalse?.Invoke();
        }

        public bool isConditionMet(float a)
        {
            switch (_condition)
            {
                case Comparison.Equal: return a == _b;
                case Comparison.GreaterThan: return a > _b;
                case Comparison.LessThan: return a < _b;
                case Comparison.NotEqual: return a != _b;
                case Comparison.GreaterOrEqual: return a >= _b;
                case Comparison.LessOrEqual: return a <= _b;
                default: return false;
            }
        }
    }
}