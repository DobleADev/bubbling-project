using DobleADev.Scriptables.Variables;
using UnityEngine;

public class QuotaRequirement : MonoBehaviour
{
    [SerializeField] IntScriptableVariable _requirementVariable;
    [SerializeField] AnimationCurve _quotaRequirementScale = new AnimationCurve(new Keyframe[2]{new Keyframe(1,3), new Keyframe(3,7)});
    [SerializeField] AnimationCurve _timeRequirementScale = new AnimationCurve(new Keyframe[2]{new Keyframe(1,25), new Keyframe(3,35)});
    [SerializeField] DropdownUnityEventFloat _onTimeRequirementUpdate;
    [SerializeField] DropdownUnityEvent _onQuotaMeet;
    [SerializeField] DropdownUnityEvent _onQuotaFail;

    public void UpdateRequirement(IntScriptableVariable level)
    {
        _requirementVariable.SetValueTyped(Mathf.FloorToInt(_quotaRequirementScale.Evaluate(level)));
        _onTimeRequirementUpdate.Invoke(_timeRequirementScale.Evaluate(level));
    }

    public void Check(IntScriptableVariable currentPoints)
    {
        if (currentPoints.GetValueTyped() >= _requirementVariable.GetValueTyped())
        {
            _onQuotaMeet.Invoke();
        }
        else
        {
            _onQuotaFail.Invoke();
        }
    }
}
