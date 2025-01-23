using System.Collections.Generic;
using System.Text;
using DobleADev.Scriptables.Variables;
using UnityEngine;
using UnityEngine.Events;

public class LevelQueueManager : MonoBehaviour
{
    [SerializeField] Vector3ScriptableVariable _nextRoomStartPoint;
    // public Vector3 nextRoomStartPoint { get { return _nextRoomStartPoint; } set { _nextRoomStartPoint = value; } }
    [SerializeField] UnityEvent _onEnqueue;
    [SerializeField] UnityEvent _onDequeue;
    [SerializeField] UnityEvent _onClear;
    Queue<GameObject> levelQueue = new Queue<GameObject>();
    
    public void Enqueue(GameObject levelPrefab)
    {
        var nextLevel = Instantiate(levelPrefab);
        nextLevel.transform.position = _nextRoomStartPoint.GetValueTyped();
        levelQueue.Enqueue(nextLevel);
        // Debug.Log(nextLevel.name + " enqueued");
        _onEnqueue?.Invoke();
    }

    public void Dequeue()
    {
        var roomToDelete = levelQueue.Dequeue();
        // Debug.Log(roomToDelete.name + " dequeued");
        Destroy(roomToDelete);
        _onDequeue?.Invoke();
    }

    public void Clear()
    {
        int queueCount = levelQueue.Count;
        // Debug.Log("Beginning clearing all " + queueCount + " objects:");
        
        for (int i = 0; i < queueCount; i++)
        {
            var roomToDelete = levelQueue.Dequeue();
            // Debug.Log(roomToDelete.name + " dequeued");
            Destroy(roomToDelete);
        }
        _onClear?.Invoke();
    }
    
    // private void OnGUI() 
    // {
    //     StringBuilder debugText = new StringBuilder();
    //     // debugText.Append("Rooms loaded:");
    //     debugText.AppendLine("Rooms loaded:");
    //     var rooms = levelQueue.ToArray();
    //     for (int i = 0; i < rooms.Length; i++)
    //     {
    //         debugText.AppendLine((i+1) + " - " + rooms[i].name);
    //     }
    //     GUI.Label(new Rect(0, 0, 360,360), debugText.ToString());
    // }
}
