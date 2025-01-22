using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] Vector3 _rotationVector;
    void Update()
    {
        transform.Rotate(Time.deltaTime * _rotationVector); 
    }
}
