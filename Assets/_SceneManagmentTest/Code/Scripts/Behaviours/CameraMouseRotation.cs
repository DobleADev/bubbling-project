using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraMouseRotation : MonoBehaviour
{
    [SerializeField] bool manual = true;
    [Header("-Orientation-", order = 0)]
    [SerializeField] Transform centerTransform; //[RECOMENDED] use a empty or "head" object to lock-rotate to
    public enum CenterOrientation {Forward = 0, Backward = 1}
    public CenterOrientation orientation = CenterOrientation.Forward;
    [SerializeField] Vector3 offset = Vector3.zero;
    [SerializeField] float orbitDistance = 1f;
    [SerializeField] float distanceStep = 1.1f;
    public Vector2 distanceLimit = new Vector2(0, 14f);

    [Header("-Mouse Configs-", order = 1)]
    [SerializeField] private Vector2 rotation = new Vector2(0f, 0f);
    public enum MouseAxis {xAxis = 0, yAxis = 1, xyAxis = 2}
    public MouseAxis axis = MouseAxis.xAxis;
    public Vector2 sensibility = new Vector2(5f, 5f);
    public Vector2 xAxisLimit = new Vector2(-180f, 180f);
    public Vector2 yAxisLimit = new Vector2(-180f, 180f);
    

    // Update is called once per frame
    void Update()
    {
        if (centerTransform == null) return;

        if (Input.mouseScrollDelta.y > 0) orbitDistance -= distanceStep;
        if (Input.mouseScrollDelta.y < 0) orbitDistance += distanceStep;

        orbitDistance = Mathf.Clamp(orbitDistance, distanceLimit.x, distanceLimit.y);

        if (manual != true)
        {
            if (orbitDistance > distanceLimit.x) orientation = CenterOrientation.Backward;
            else orientation = CenterOrientation.Forward;
        } 

        RotationUpdate();
        CameraUpdate();
    }

    void RotationUpdate()
    {
        switch (axis)
        {
            case MouseAxis.xAxis:
            {
                rotation.x += Input.GetAxisRaw("Mouse X") * sensibility.x;
                if (xAxisLimit.x > -180 || xAxisLimit.y < 180) rotation.x = Mathf.Clamp(rotation.x, xAxisLimit.x, xAxisLimit.y);
            } break;

            case MouseAxis.yAxis:
            {
                rotation.y += Input.GetAxisRaw("Mouse Y") * sensibility.y;
                if (yAxisLimit.x > -180 || yAxisLimit.y < 180) rotation.y = Mathf.Clamp(rotation.y, yAxisLimit.x, yAxisLimit.y);
            } break;

            case MouseAxis.xyAxis:
            {
                rotation.x += Input.GetAxisRaw("Mouse X") * sensibility.x;
                rotation.y += Input.GetAxisRaw("Mouse Y") * sensibility.y;
                if (xAxisLimit.x > -180 || xAxisLimit.y < 180) rotation.x = Mathf.Clamp(rotation.x, xAxisLimit.x, xAxisLimit.y);
                if (yAxisLimit.x > -180 || yAxisLimit.y < 180) rotation.y = Mathf.Clamp(rotation.y, yAxisLimit.x, yAxisLimit.y);
                
            } break;
        }
    }

    void CameraUpdate()
    {
        switch (orientation)
        {
            case CenterOrientation.Forward: //First-Person View
            {
                centerTransform.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);
                transform.position = centerTransform.position + offset;
                transform.localRotation = centerTransform.rotation * Quaternion.AngleAxis(rotation.y, Vector3.left);
            } break;
            case CenterOrientation.Backward: //Orbital Third-Person View
            {
                centerTransform.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);
                transform.position = centerTransform.position - (centerTransform.forward * orbitDistance) + offset;
                transform.forward = centerTransform.forward;
                transform.RotateAround(centerTransform.position, -transform.right, rotation.y);
            } break;
        }
    }
}
