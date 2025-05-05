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
        Debug.Log("Enemy took damage. Current health: " + _currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died.");
        // Podés agregar animaciones, efectos, etc.
        Destroy(gameObject);
    }
}
