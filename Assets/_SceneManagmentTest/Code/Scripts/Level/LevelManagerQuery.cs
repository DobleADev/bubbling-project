using DoubleADev.ScriptableEvent;
using UnityEngine;

public class LevelManagerQuery : MonoBehaviour
{
    [SerializeField] SceneContainerEvent _onGetCurrentArea;
    [SerializeField] SceneContainerEvent _onGetCurrentRoom;

    public void QueryGetArea()
    {
        SceneContainer area = LevelManager.instance.area;
        // Debug.Log(area);
        _onGetCurrentArea?.Invoke(area);
    }

    public void QueryGetRoom()
    {
        SceneContainer room = LevelManager.instance.room;
        // Debug.Log(room);
        _onGetCurrentRoom?.Invoke(room);
    }
}
