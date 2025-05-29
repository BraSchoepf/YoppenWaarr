using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Referencia al SO")]

    [SerializeField] private Character_Data_SO _character_Data_SO;

    private int maxHealth;
    private int currentHealth;

    //effect for damage recive - for flashDamage()
    public SpriteRenderer spriteRenderer;
    private Color _originalColor;
    
    [Header("UI")]
    public TextMeshProUGUI healthText;

    void Start()
    {
        maxHealth = _character_Data_SO.Health;
        currentHealth = maxHealth;
        UpdateHealthUI();
        _originalColor  = spriteRenderer.color;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("El player recibe daño, vida restante: " + currentHealth);
        UpdateHealthUI();

        // Coroutina for flashDamage()
        StartCoroutine(FlashDamage());

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

    //Coroutina for effect damage recive

    IEnumerator FlashDamage()
    {
        int flashCount = 2;
        float flashDuration = 0.1f;
        

        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = _originalColor; //  original color
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
