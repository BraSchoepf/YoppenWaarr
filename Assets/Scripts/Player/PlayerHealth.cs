using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("El player recibe daño, vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {   
            Die();
        }
    }

    void Die()
    {
        Debug.Log("El player murio");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
