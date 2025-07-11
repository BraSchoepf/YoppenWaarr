using UnityEditor.Experimental;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [HideInInspector] public BossManager bossManager;

    public Animator Animator;
    public SpriteRenderer spriteRenderer;
    public Color originalColor;

    public float velocidadMovimiento = 2f;
    public float distanciaMinima = 1.5f;

    private Transform objetivoJugador;
    private BossState currentState;

    [Header("Melee Attack")]
    public float radiusAttack = 1.2f;
    public Transform pointAttack;
    public LayerMask playerLayer;
    public int MeleeDamage = 1;

    private bool _possibleDamage = false;

    void Start()
    {
        Animator = GetComponent<Animator>();
        currentState = new IdleState(this);
        currentState.Enter();
    }

    void Update()
    {
        currentState.Update();
    }

    public void ChangeState(BossState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void MoveTowardsPlayer()
    {
        if (objetivoJugador == null)
            objetivoJugador = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (objetivoJugador != null)
        {
            Vector2 dir = (objetivoJugador.position - transform.position).normalized;
            transform.position += (Vector3)dir * velocidadMovimiento * Time.deltaTime;
        }
    }

    public bool InAttackRange()
    {
        if (objetivoJugador == null)
            return false;

        return Vector2.Distance(transform.position, objetivoJugador.position) <= distanciaMinima;
    }

    public bool ShouldDisappear()
    {
        return objetivoJugador != null;
    }

    public void PerformAttack()
    {
        Collider2D player = Physics2D.OverlapCircle(pointAttack.position, radiusAttack, playerLayer);
        if (player != null)
        {
            Vector2 direccionKnockback = (player.transform.position - transform.position).normalized;

            PlayerKnockback knockback = player.GetComponent<PlayerKnockback>();
            if (knockback != null)
            {
                knockback.RecibirKnockback(direccionKnockback, 8f, MeleeDamage); // fuerza y da�o
            }
            else
            {
                // fallback en caso de no tener knockback
                player.GetComponent<PlayerHealth>()?.TakeDamage(MeleeDamage);
            }
        }


    }
    public void OrientTowardsPlayer()
    {
        if (objetivoJugador == null)
            return;

        float dirX = objetivoJugador.position.x - transform.position.x;

        transform.rotation = (dirX >= 0)
    ? Quaternion.Euler(0, 180, 0)   // Mira a la derecha
    : Quaternion.Euler(0, 0, 0);    // Mira a la izquierda
    }

    private void OnDrawGizmosSelected()
    {
        if (pointAttack != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pointAttack.position, radiusAttack);
        }
    }

    public void AllowDamage()
    {
        _possibleDamage = true;
    }

    public void ActiveDamage()
    {
        if (_possibleDamage && currentState is AttackState)
        {
            PerformAttack();
            _possibleDamage = false;
        }

    }

    public void EnableDamage()
    {
        GetComponent<BossHealth>().EnableDamage();
    }

    public void DisableDamage()
    {
        GetComponent<BossHealth>().DisableDamage();
    }

}

