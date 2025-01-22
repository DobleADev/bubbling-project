using UnityEngine;

[CreateAssetMenu(fileName = "NewCheckpoint", menuName = "Scriptable Object/Checkpoint")]
public class CheckpointData : ScriptableObject
{
    [SerializeField] private Vector3 _position;
    public Vector3 position { get { return _position; } set { _position = value; } }
    [SerializeField] private SceneContainer _area;
    public SceneContainer area { get { return _area; } set { _area = value; } }
    [SerializeField] private SceneContainer _room;
    public SceneContainer room { get { return _room; } set { _room = value; } }
    [SerializeField] private string _camera;
    public string camera { get { return _camera; } set { _camera = value; } }
    public void SetCamera(Object cameraObject)
    {
        if (cameraObject == null)
        {
            _camera = "";
            return;
        }
        _camera = cameraObject.name;
    }

    public void SetDataFromOther(CheckpointData other)
    {
        position = other.position;
        area = other.area;
        room = other.room;
        _camera = other.camera;
    }
}
