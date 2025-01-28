using UnityEngine;

public class DebugLogMessage : MonoBehaviour
{
    [SerializeField] string _message = "Log message";
    
    public void Log() => Debug.Log(_message);
    public void Log(string message) => Debug.Log(message);
}
