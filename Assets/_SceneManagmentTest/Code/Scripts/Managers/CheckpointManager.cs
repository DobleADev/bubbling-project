using UnityEngine;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance { get; private set; }
    public Checkpoint lastCheckpoint { get; private set; }
    [SerializeField] Checkpoint startCheckpoint;

    void Start()
    {
        if (instance == null) instance = this;
        instance.SaveCheckpoint(startCheckpoint);
    }

    public bool SaveCheckpoint(Checkpoint nextCheckpoint)
    {
        if (instance.lastCheckpoint == nextCheckpoint) return false;
        instance.lastCheckpoint = nextCheckpoint;
        return true;
    }
}
