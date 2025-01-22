using DobleADev.Core;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class DoorController : MonoBehaviour
{
    [SerializeField] private bool _isOpened;
    [SerializeField] InspectorButton openDoor;
    [SerializeField] InspectorButton closeDoor;
    [SerializeField] private DropdownUnityEvent _onStartOpened;
    [SerializeField] private DropdownUnityEvent _onOpen;
    [SerializeField] private DropdownUnityEvent _onClose;

    public bool OPENED 
    {
        get { return _isOpened; }
        set { _isOpened = value; }
    }
    public UnityEvent openEvent => _onOpen.Action;
    public UnityEvent closeEvent => _onClose.Action;
    
    void Start() // ASSUMING CLOSED AS DEFAULT STATE
    {
        if (_isOpened) _onStartOpened?.Invoke();
    }

    void OnEnable()
    {
       openDoor = new InspectorButton("Open", Open);
       closeDoor = new InspectorButton("Close", Close);
    }

    public void Open()
    {
        if (_isOpened) return;
        _isOpened = true;
        _onOpen?.Invoke();
    }

    public void Close()
    {
        if (!_isOpened) return;
        _isOpened = false;
        _onClose?.Invoke();
    }
}
