using Unity.Cinemachine;
using UnityEngine;

public class Sword_Collider : MonoBehaviour
{
    [Header("Attack Hitboxes")]
    public GameObject attackPoint;  // Un GameObject con collider, como la espada
    [SerializeField] private int attackDamage = 1;

    [Header("Damage Settings")]
    public float attackRange = 0.5f;
    
    public LayerMask enemyLayers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
           
           

        }
    }

    //public void PerformAttack()
    //{
    //  Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, enemyLayers);

    // foreach (Collider2D enemy in hitEnemies)
    // {
    // Debug.Log("Golpeado: " + enemy.name);
    //enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
    //}
    // }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
