using UnityEngine;

public class AddForceOnTrigger : MonoBehaviour
{
    [SerializeField] Vector3 _forceVelocity = Vector3.zero;
    [SerializeField] ForceMode _forceMode = ForceMode.Force;
    [SerializeField] string _targetTag = "Untagged";

    private void OnTriggerEnter(Collider other) 
    {
        Response(other);
    }

    private void OnTriggerStay(Collider other) 
    {
        Response(other);
    }

    void Response(Collider other)
    {
        if (other.CompareTag(_targetTag))
        {
            if (other.TryGetComponent(out Rigidbody body))
            {
                body.AddForce(transform.TransformVector(_forceVelocity), _forceMode);
            }
        }  
    }
}
