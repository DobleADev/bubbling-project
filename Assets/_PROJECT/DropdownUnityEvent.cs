// using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DropdownUnityEvent
{
    public bool IsEmpty() { return actions.GetPersistentEventCount() == 0; }
    [SerializeField] private UnityEvent actions;
    public UnityEvent Action { get { return actions;}}
    public void Invoke() { if (actions != null) actions.Invoke(); }
}

[System.Serializable]
public class UnityEventFloat : UnityEvent<float> {}

[System.Serializable]
public class DropdownUnityEventFloat
{
    public bool IsEmpty() { return actions.GetPersistentEventCount() == 0; }
    [SerializeField] private UnityEventFloat actions;
    public void Invoke(float arg0) { if (actions != null) actions.Invoke(arg0); }
}

[System.Serializable]
public class UnityEventGameObject : UnityEvent<GameObject> {}

[System.Serializable]
public class DropdownUnityEventGameObject
{
    public bool IsEmpty() { return actions.GetPersistentEventCount() == 0; }
    [SerializeField] private UnityEventGameObject actions;
    public void Invoke(GameObject arg0) { if (actions != null) actions.Invoke(arg0); }
}

[System.Serializable]
public class UnityEventTransform : UnityEvent<Transform> {}

[System.Serializable]
public class DropdownUnityEventTransform
{
    public bool IsEmpty() { return actions.GetPersistentEventCount() == 0; }
    [SerializeField] private UnityEventTransform actions;
    public void Invoke(Transform arg0) { if (actions != null) actions.Invoke(arg0); }
}

[System.Serializable]
public class UnityEventVector3 : UnityEvent<Vector3> {}

[System.Serializable]
public class DropdownUnityEventVector3
{
    public bool IsEmpty() { return actions.GetPersistentEventCount() == 0; }
    [SerializeField] private UnityEventVector3 actions;
    public void Invoke(Vector3 arg0) { if (actions != null) actions.Invoke(arg0); }
}

// [System.Serializable]
// public class UnityEventCinemachineVirtualCamera : UnityEvent<CinemachineVirtualCamera> {}

// [System.Serializable]
// public class DropdownUnityEventCinemachineVirtualCamera
// {
//     public bool IsEmpty() { return actions.GetPersistentEventCount() == 0; }
//     [SerializeField] private UnityEventCinemachineVirtualCamera actions;
//     public void Invoke(CinemachineVirtualCamera arg0) { if (actions != null) actions.Invoke(arg0); }
// }

[System.Serializable]
public class DropdownUnityEvent<T>
{
    [SerializeField] private UnityEvent<T> actions;
    public void Invoke(T arg0) { if (actions != null) actions.Invoke(arg0); }
}

[System.Serializable]
public class DropdownUnityEvent<T0, T1>
{
    [SerializeField] private UnityEvent<T0, T1> actions;
    public void Invoke(T0 arg0, T1 arg1) { if (actions != null) actions.Invoke(arg0, arg1); }
}
