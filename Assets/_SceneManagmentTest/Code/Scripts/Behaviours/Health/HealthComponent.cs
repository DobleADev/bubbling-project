using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] HealthData health;
    [SerializeField] float currentHealth;
    [SerializeField] DropdownUnityEventFloat onHeal;
    [SerializeField] DropdownUnityEventFloat onHurt;
    [SerializeField] DropdownUnityEvent onDeath;

    public UnityEvent DeadEvent => onDeath.Action;

    private void Start() 
    {
        if (health == null) return;
        currentHealth = health.MaxHealth;
    }
    
    public void Heal(float healPoints)
    {
        currentHealth += healPoints;
        onHeal.Invoke(healPoints);

        if (health == null) return;
        if (currentHealth > health.MaxHealth)
        {
            currentHealth = health.MaxHealth;
        }
    }

    public void TakeDamage(float damagePoints)
    {
        currentHealth -= damagePoints;
        onHurt.Invoke(damagePoints);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            onDeath.Invoke();
        }
    }
}
