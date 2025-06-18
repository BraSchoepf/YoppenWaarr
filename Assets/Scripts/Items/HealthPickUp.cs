using FMODUnity;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int amountHealth = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.sfx_powerUp);
            //AudioManager.Instance.PlaySFX(AudioManager.Instance.sfx_dog);
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Curar(amountHealth);
            }

            Destroy(gameObject);
        }
    }
}
