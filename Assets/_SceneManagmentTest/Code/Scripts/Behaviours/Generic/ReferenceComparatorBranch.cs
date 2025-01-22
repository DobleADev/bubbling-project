using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReferenceComparatorBranch : MonoBehaviour
{
    [SerializeField] List<ReferenceEventBranch> _branches = new List<ReferenceEventBranch>();
    public void Comparate(Object reference)
    {
        foreach (var branch in _branches)
        {
            if (branch.reference == null || branch.reference != reference) continue;
            branch.response?.Invoke();
        }
    }

    [System.Serializable]
    struct ReferenceEventBranch
    {
        public Object reference;
        public UnityEvent response;
    }
}
