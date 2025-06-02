using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 6;
    private int _currentHealth;

    //effect for damage recive - for flashDamage()
    public SpriteRenderer spriteRenderer;
    private Color _originalColor;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _originalColor  = spriteRenderer.color;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        Debug.Log("El enemigo recibe daño, vida restante:: " + _currentHealth);

        // Coroutina for flashDamage()
        StartCoroutine(FlashDamage());

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("ENEMY DOWN, COUNTER-TERRORIST WIN.");
        // I leave this space to add animations at a later date
        Destroy(gameObject);
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
            spriteRenderer.color = _originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
