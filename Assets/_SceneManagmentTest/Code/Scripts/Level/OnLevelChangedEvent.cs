using System.Collections;
using System.Collections.Generic;
using DoubleADev.ScriptableEvent;
using UnityEngine;

public class OnLevelChangedEvent : MonoBehaviour
{
    [SerializeField] SceneContainerEvent _response;
    private void OnEnable() {
        LevelManager.instance.onRoomChanged += ResponseToChange;
    }

    private void OnDisable() {
        LevelManager.instance.onRoomChanged -= ResponseToChange;
    }

    void ResponseToChange(SceneContainer nextLevel)
    {
        _response?.Invoke(nextLevel);
    }
}
