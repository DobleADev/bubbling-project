using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthDeadEventHandler : MonoBehaviour
{
    [SerializeField] HealthComponent _healthEntity;
    [SerializeField] UnityEvent onDeath;

    private void OnEnable() 
    {
        _healthEntity.DeadEvent.AddListener(onDeath.Invoke);
    }

    private void OnDisable() 
    {
        _healthEntity.DeadEvent.RemoveListener(onDeath.Invoke);
    }
}
