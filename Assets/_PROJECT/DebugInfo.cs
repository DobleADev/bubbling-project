﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInfo : MonoBehaviour
{
    [SerializeField, Range(0.02f, 2)] private float tickRate = 0.1f;
    [SerializeField] DropdownUnityEventFloat _onGetFPS;

    IEnumerator Start()
    {
        while (true)
        {
            _onGetFPS.Invoke(1 / Time.unscaledDeltaTime);
            yield return new WaitForSecondsRealtime(tickRate);
        }
    }
}
