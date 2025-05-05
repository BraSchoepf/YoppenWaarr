
using UnityEngine;

public class Sword_Collider : MonoBehaviour
{
    [Header("Attack Hitboxes")]
    public GameObject attackPoint;  // Objeto que contiene el collider
    [SerializeField] private int _attackDamage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(_attackDamage);
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

