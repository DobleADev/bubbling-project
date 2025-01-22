using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointLoader : MonoBehaviour
{
    [SerializeField] UnityEventVector3 onRequestPosition;
    [SerializeField] UnityEventCinemachineVirtualCamera onRequestCamera;

    public void RequestPosition()
    {
        Checkpoint checkpoint = CheckpointManager.instance.lastCheckpoint;
        if (checkpoint == null) 
        {
            Debug.Log("There's no checkpoint saved");
            return;
        }
        onRequestPosition?.Invoke(checkpoint.transform.position);
    }

    public void RequestCamera()
    {
        Checkpoint checkpoint = CheckpointManager.instance.lastCheckpoint;
        if (checkpoint == null) 
        {
            Debug.Log("There's no checkpoint saved");
            return;
        }
        // onRequestCamera?.Invoke(checkpoint.GetComponent<Camera>());
    }
}
