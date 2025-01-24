using System;
using UnityEngine;

public class AddForceToCenter : MonoBehaviour
{
    [SerializeField] float _minimumStrength = 4f;
    [SerializeField] float _maximumStrength = 10f; // Fuerza del vórtice
    [SerializeField] float _blendDistance = 5f; // Distancia a partir de la cual se empieza a mezclar la fuerza
    [SerializeField, Range(0, 1)] float _initialDesviation = 0.4f;
    [SerializeField] bool _inverted;
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
        if (other.CompareTag(_targetTag) && other.TryGetComponent(out Rigidbody body))
        {
            Transform selfTransform = transform;
            Transform otherTransform = body.transform;

            // Vector desde el centro del vórtice al objeto
            Vector3 toTarget = otherTransform.position - selfTransform.position;

            // Calcular la distancia
            float distance = toTarget.magnitude;

            // Blend distance: Valor entre 0 y 1 que indica qué tan cerca está el objeto del centro
            float blend = Mathf.Clamp01(distance / _blendDistance);
            // Debug.Log("Blend: " + blend);

            // Vector tangente al círculo imaginario alrededor del vórtice
            Vector3 tangent = Vector3.Cross(toTarget, (_inverted ? -1 : 1) * Vector3.forward).normalized;

            // Fuerza final: Mezcla entre la fuerza tangencial y la fuerza hacia el centro
            Vector3 finalVelocity = -1 * Vector3.Lerp(toTarget, tangent, Mathf.Clamp01(blend * _initialDesviation)) * Mathf.Lerp(_minimumStrength, _maximumStrength, blend);

            body.AddForce(finalVelocity, _forceMode);
        }
    }
}
