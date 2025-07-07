using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    private PlayerController _player;


    [Header("Referencia al SO")]

    [SerializeField] private Character_Data_SO _character_Data_SO;

    private int _maxHealth;
    private int _currentHealth;

    //effect for damage recive - for flashDamage()
    public SpriteRenderer spriteRenderer;
    private Color _originalColor;

    [Header("UI")]
    public TextMeshProUGUI healthText;
    public HUDVida hudVida; // Asignalo en Inspector

    private FMOD.Studio.EventInstance lowLifeInstance;
    private bool lowLifeSonando = false;

    void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    void Start()
    {
        _maxHealth = _character_Data_SO.Health;
        _currentHealth = _maxHealth;
        UpdateHealthUI();
        _originalColor  = spriteRenderer.color;
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        //Debug.Log("El player recibe daño, vida restante: " + _currentHealth);
        UpdateHealthUI();

        // Coroutina for flashDamage()
        StartCoroutine(FlashDamage());

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Vida: " + _currentHealth;
        }

        if (hudVida != null)
        {
            hudVida.ActualizarVida(_currentHealth);
        }

        // --- SFX de vida baja ---
        if (_currentHealth <= 50 && !lowLifeSonando)
        {
            lowLifeInstance = FMODUnity.RuntimeManager.CreateInstance(AudioManager.Instance.sfx_lowLife);
            lowLifeInstance.start();
            lowLifeSonando = true;
        }
        else if (_currentHealth > 50 && lowLifeSonando)
        {
            lowLifeInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            lowLifeInstance.release();
            lowLifeSonando = false;
        }
    }

    void Die()
    {
        DetenerLowLifeSFX();
        _player.ChangeState(_player.dieState);

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

    public void Curar(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
        Debug.Log("Curado. Vida ACTUAL: " + _currentHealth);
        UpdateHealthUI();
    }

    public void PausarLowLifeSFX()
    {
        if (lowLifeSonando && lowLifeInstance.isValid())
            lowLifeInstance.setPaused(true);
    }

    public void ReanudarLowLifeSFX()
    {
        if (lowLifeSonando && lowLifeInstance.isValid())
            lowLifeInstance.setPaused(false);
    }

    public void DetenerLowLifeSFX()
    {
        if (lowLifeSonando && lowLifeInstance.isValid())
        {
            lowLifeInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            lowLifeInstance.release();
            lowLifeSonando = false;
        }
    }
}
