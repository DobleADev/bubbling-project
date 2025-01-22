using Cinemachine;
using DoubleADev.ScriptableEvent;
using DoubleADev.Scriptables.Events;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointFetch : MonoBehaviour
{
    [SerializeField] CheckpointData checkpoint;
    [SerializeField] Vector3Event onGetPosition;
    [SerializeField] SceneContainerEvent onGetArea;
    [SerializeField] SceneContainerEvent onGetRoom;
    [SerializeField] StringEvent onGetCamera;

    public void QueryPosition() { QueryPosition(checkpoint); }
    public void QueryPosition(CheckpointData checkpointData)
    {
        onGetPosition?.Invoke(checkpointData.position);
    }
    
    public void QueryArea() { QueryArea(checkpoint); }
    public void QueryArea(CheckpointData checkpointData)
    {
        onGetArea?.Invoke(checkpointData.area);
    }

    public void QueryRoom() { QueryRoom(checkpoint); }
    public void QueryRoom(CheckpointData checkpointData)
    {
        onGetRoom?.Invoke(checkpointData.room);
    }

    public void QueryCamera() { QueryCamera(checkpoint); }
    public void QueryCamera(CheckpointData checkpointData)
    {
        onGetCamera?.Invoke(checkpointData.camera);
    }
}

