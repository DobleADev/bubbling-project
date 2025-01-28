using UnityEngine;

public class LookRotation : MonoBehaviour
{
    [SerializeField] Vector3 _lookVector;
    public Vector3 lookVector { get { return _lookVector; } set { _lookVector = value; } }
    
    void Update()
    {
        if (_lookVector == Vector3.zero) return;
        transform.rotation = Quaternion.LookRotation(lookVector);
    }
}
