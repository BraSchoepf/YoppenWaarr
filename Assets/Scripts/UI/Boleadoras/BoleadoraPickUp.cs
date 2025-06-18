using UnityEngine;

public class BoleadoraPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.sfx_powerUp);
            GameManager.Instance.AddBoleadora();
            Destroy(gameObject);
        }
    }
}
