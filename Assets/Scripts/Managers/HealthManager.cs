using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void ReduceHealth()
    {
        currentHealth--;
    }

    public virtual void IncreaseHealth()
    {
        currentHealth++;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public bool ShouldDie() => currentHealth <= 0;
}
