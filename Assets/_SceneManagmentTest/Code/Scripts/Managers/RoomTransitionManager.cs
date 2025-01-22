using System;
using System.Collections;
using System.Collections.Generic;
using DoubleADev.Scriptables;
using UnityEngine;
using UnityEngine.Events;

public class RoomTransitionManager : MonoBehaviour
{
    public static RoomTransitionManager instance { get; private set; }
    public bool isPlaying { get; set; }
    [SerializeField] ScriptableEvent onStartTransitionEvent;
    [SerializeField] ScriptableEvent onEndTransitionEvent;
    
    void Start()
    {
        if (instance == null) instance = this;
    }

    public void StartTransition()
    {
        onStartTransitionEvent.Raise();
    }

    public void EndTransition()
    {
        onEndTransitionEvent.Raise();
    }
}
