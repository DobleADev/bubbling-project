using UnityEngine;

public class MouseDistanceJoystick : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] Transform _center;
    [SerializeField, Range(0, 7)] int _mouseButton;
    [SerializeField, Min(0)] float _minDistance = 0;
    [SerializeField, Min(0.00001f)] float _maxDistance = 1;
    [SerializeField] Vector2Event _onMove;

    void Update()
    {
        Vector2 finalVector = Vector2.zero;
        if (Input.GetMouseButton(_mouseButton) && _camera != null)
        {
            Vector3 mousePosition = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
            Vector3 centerPosition = _center == null? 0.5f * Vector3.one : _camera.WorldToViewportPoint(_center.position);
            Vector2 difference = mousePosition - centerPosition;
            // Debug.Log(mousePosition + " - " + centerPosition + " = " + difference);
            // Debug.Log(difference);
            float finalScale = Mathf.Clamp01((difference.magnitude - _minDistance) / _maxDistance );
            // Debug.Log(finalScale);
            finalVector = finalScale * difference.normalized;
            // finalVector /= _maxDistance;
            // Debug.Log(finalScale + " * " + difference.normalized + " = " + finalVector);
        }
        _onMove?.Invoke(finalVector);
        
    }
}
