// using UnityEngine;

// [RequireComponent(typeof(CircleCollider2D))]
// public class Enemy_Vision : MonoBehaviour
// {
//     [SerializeField] private Enemy_Controller _enemyController;
//     private CircleCollider2D _circleCollider2D; 

//     private void Start()
//     {
//         _circleCollider2D = GetComponent<CircleCollider2D>();
//     }
//         public void OnTriggerEnter2D(Collider2D other)
//     {
//         if (!other.CompareTag("Player")) return;
        
//         var playerDirection = other.transform.position - transform.position;
//         var layer = 1 << LayerMask.NameToLayer("Player");
//         var rayCast = Physics2D.Raycast(transform.position, playerDirection, _circleCollider2D.radius, layer);
//         if (rayCast.collider == null) return;

//         if (rayCast.collider.CompareTag("Player"))
//         {
//             _enemyController.SetTarget(other.transform);
//         }                
//     }
// }

using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Enemy_Vision : MonoBehaviour
{
    [SerializeField] private Enemy_Controller _enemyController;
    private CircleCollider2D _circleCollider2D;

    private void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();

        // Si no asignaron _enemyController en el Inspector, buscarlo automáticamente
        if (_enemyController == null)
        {
            _enemyController = GetComponentInParent<Enemy_Controller>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Vector2 directionToPlayer = other.transform.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, _circleCollider2D.radius);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            _enemyController.SetTarget(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _enemyController.ClearTarget();
    }
}
