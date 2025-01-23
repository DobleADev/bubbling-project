using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventQueue : MonoBehaviour
{
    [SerializeField] UnityEvent[] _eventsReference;
    Queue<UnityEvent> _eventsQueue;

    void Start()
    {
        Reset();
    }

    public void Dequeue()
    {
        _eventsQueue.Dequeue()?.Invoke();
    }

    public void Reset()
    {
        _eventsQueue = new Queue<UnityEvent>(_eventsReference);
    }
}
