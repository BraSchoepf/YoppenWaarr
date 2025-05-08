using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("El player recibe daño, vida restante: " + currentHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {   
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Vida: " + currentHealth;
        }
    }

    void Die()
    {
        Debug.Log("El player murio");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
