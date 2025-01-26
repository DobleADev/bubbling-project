using DoubleADev.Scriptables.Events;
using UnityEngine;

public class SendTransformData : MonoBehaviour
{
    // [SerializeField] TransformEvent _onTransformSent;
    [SerializeField] Vector3Event _onPositionSent;
    [SerializeField] Vector3Event _onLocalPositionSent;
    private Transform _transform;
    private void Awake() 
    {
        _transform = transform;
    }
    // public void SendTransform()
    // {
    //     _onTransformSent?.Invoke(_transform);
    // }

    public void SendPosition()
    {
        _onPositionSent?.Invoke(_transform.position);
    }

    public void SendLocalPosition()
    { 
        _onLocalPositionSent?.Invoke(_transform.localPosition);
    }
}
