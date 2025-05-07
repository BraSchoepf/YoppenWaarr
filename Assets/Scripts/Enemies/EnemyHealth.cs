using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 6;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        Debug.Log("El enemigo recibe daño, vida restante:: " + _currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("ENEMY DOWN, COUNTER-TERRORIST WIN.");
        // dejo este espacio para agregar animaciaones mas adelante
        Destroy(gameObject);
    }
}
