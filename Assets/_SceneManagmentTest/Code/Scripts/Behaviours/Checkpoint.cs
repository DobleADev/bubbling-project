using Cinemachine;
using DoubleADev.ScriptableEvent;
using DoubleADev.Scriptables.Events;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    [SerializeField] CheckpointData checkpointReference;
    [SerializeField] Vector3Event onCheckpointReached;

    private void OnTriggerEnter(Collider other) 
    {
        if (checkpointReference == null)
        {
            Debug.Log("Invalid checkpoint reference");
            return;
        }
        if (!other.CompareTag(playerTag) || checkpointReference.position == transform.position) return;
        onCheckpointReached?.Invoke(transform.position);  
    }
}
