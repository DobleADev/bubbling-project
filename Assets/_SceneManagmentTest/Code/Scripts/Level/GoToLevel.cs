using System.Collections;
using System.Collections.Generic;
using DoubleADev.ScriptableEvent;
using UnityEngine;

public class GoToLevel : MonoBehaviour
{
    [SerializeField] SceneContainerEvent _onGoToArea;
    [SerializeField] SceneContainerEvent _onGoToRoom;
    public void GoToRoom(SceneContainer room)
    {
        if (room == null) return;
        LevelManager.instance.room = room;
        _onGoToRoom?.Invoke(room);
    }

    public void GoToArea(SceneContainer area)
    {
        if (area == null) return;
        LevelManager.instance.room = area;
        _onGoToArea?.Invoke(area);
    }
}
