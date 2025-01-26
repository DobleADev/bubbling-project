using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private DropdownUnityEvent _onStart;
    [SerializeField] private DropdownUnityEvent _onEnd;
    [SerializeField] private DropdownUnityEventFloat _onPing;
    public bool paused { get; set; }
    public float currentTime { get; private set; }
    private Coroutine _pingRoutine;

    public void SetTime(float newTime)
    {
        currentTime = newTime;
        _onPing.Invoke(currentTime);
    }

    public void Run()
    {
        if (_pingRoutine != null)
        {
            Debug.Log("Timer already running");
            return;
        }
        _pingRoutine = StartCoroutine(PingProcess());
    }

    public void Stop()
    {
        if (_pingRoutine == null) 
        {

            return;
        }
        
        StopCoroutine(_pingRoutine);
        _pingRoutine = null;
    }
    
    IEnumerator PingProcess()
    {
        _onStart.Invoke();
        while (currentTime > 0)
        {
            if (!paused) 
            {
                currentTime -= Time.deltaTime;
                _onPing.Invoke(currentTime);
            }
            yield return null;
        }
        currentTime = 0;
        _onEnd.Invoke();
    }
}
