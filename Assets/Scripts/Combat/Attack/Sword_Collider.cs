
using UnityEngine;

public class Sword_Collider : MonoBehaviour
{
    [Header("Attack Hitboxes")]
    public GameObject attackPoint;  // Object containing the collider
    [SerializeField] private int _attackDamage = 1;

    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = transform.root; // Asume que la espada es hija del jugador
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
            if (enemy != null && !enemy.IsDead) // Evita dañar enemigos muertos
            {
                Vector2 knockbackDirection = (collision.transform.position - _playerTransform.position).normalized;
                enemy.TakeDamage(_attackDamage, knockbackDirection);
            }
        }
    }
    public void EnableCollider()
    {
        attackPoint.GetComponent<Collider2D>().enabled = true;
    }

    public void DisableCollider()
    {
        attackPoint.GetComponent<Collider2D>().enabled = false;
    }
}

