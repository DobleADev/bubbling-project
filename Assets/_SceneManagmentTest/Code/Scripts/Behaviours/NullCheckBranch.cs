using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NullCheckBranch : MonoBehaviour
{
    [SerializeField] UnityEvent _onNull;
    [SerializeField] UnityEvent _onNotNull;

    public void Check(Object a)
    {
        // Debug.Log("a is null? " + (a is null));
        if (a is null) _onNull?.Invoke();
        else _onNotNull?.Invoke();
    }
}
