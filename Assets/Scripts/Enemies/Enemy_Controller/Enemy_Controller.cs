using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public float visionRange = 5f;
    public LayerMask obstacleMask;
    public Transform player;
    public float wanderRadius = 3f;
    public float chaseSpeed = 3.5f;
    public float wanderSpeed = 2f;

    private NavMeshAgent _agent;
    private Vector3 _initialPosition;
    private bool _isChasing = false;
    private Vector3 _lastSeenPosition;
    private bool _goingToLastSeenPosition = false;
    private bool _returningToInitialPosition = false;

    [SerializeField]private float _wanderCooldown = 0.5f;
    [SerializeField]private float _wanderTimer = 0.5f;

    [Header("Attack Settings")]
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public int attackDamage = 1;

    private float _lastAttackTime;

    //For animations
    private Animator _animator;
    private Coroutine _attackCoroutine;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _initialPosition = transform.position;

        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (CanSeePlayer() && distanceToPlayer <= visionRange)
        {
            _lastSeenPosition = player.position;
            _goingToLastSeenPosition = false;
            _isChasing = true;
            _agent.speed = chaseSpeed;
            

            if (distanceToPlayer <= attackRange)
            {
                 Attack();
                _agent.SetDestination(transform.position);
            }
            else
            {
                _agent.SetDestination(player.position);
            }
        }
        else if (_isChasing)
        {
            if (!_goingToLastSeenPosition)
            {
                _agent.speed = chaseSpeed;
                _agent.SetDestination(_lastSeenPosition);
                _goingToLastSeenPosition = true;
            }
            else
            {
                if (Vector2.Distance(transform.position, _lastSeenPosition) < 0.5f)
                {
                    _isChasing = false;
                    _goingToLastSeenPosition = false;
                    // _wanderTimer = _wanderCooldown; // Forzar a que busque un nuevo punto de wander
                    _returningToInitialPosition = true;
                    ReturnToInitialPosition();                    
                }
            }
        }
        else
        {
            if (_returningToInitialPosition)
            {
                if (Vector2.Distance(transform.position, _initialPosition) < 0.5f)
                {
                    _returningToInitialPosition =false;
                    _wanderTimer = _wanderCooldown;
                }
            }
            else
            {
                Wander();
            }
        }

        HandleAnimations();
    }


    bool CanSeePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleMask);
        if (hit.collider == null)
        {
            return true; // No hay obstáculo en medio
        }
        return false; // Hay algo bloqueando
    }

    void ChasePlayer()
    {
        _isChasing = true;
        _agent.speed = chaseSpeed;
        _agent.SetDestination(player.position);
    }

    void ReturnToInitialPosition()
    {
        _agent.speed = wanderSpeed;
        _agent.SetDestination(_initialPosition);

        if (Vector2.Distance(transform.position, _initialPosition) < 0.5f)
        {
            _isChasing = false;
        }
    }

    void Wander()
    {
        _wanderTimer += Time.deltaTime;

        if (!_agent.hasPath || _agent.remainingDistance < 0.5f || _wanderTimer >= _wanderCooldown)
        {
            Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
            Vector3 wanderTarget = _initialPosition + new Vector3(randomDirection.x, randomDirection.y, 0);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(wanderTarget, out hit, 1.0f, NavMesh.AllAreas))
            {
                _agent.speed = wanderSpeed;
                _agent.SetDestination(hit.position);
            }

            _wanderTimer = 0f;
        }
    }

    void Attack()
    {
        //Esto asegura que el enemigo no inicie un nuevo ataque si ya está atacando, pero sí podrá volver a atacar cuando termine y el cooldown lo permita.
        if (Time.time - _lastAttackTime < attackCooldown || _animator.GetBool("isAttacking")) return;

        //Congelar manualmente la velocidad del NavMeshAgent durante el ataque - YW-ES-005 BUG-ES-004
        _agent.velocity = Vector3.zero;
        _agent.isStopped = true;

        //For attack para que dispare la función ApplyAttackDamage() 1 vez x golpe y 1HP quite - YW-ES-006 BUG-ES-005
        //Activate animation attack
        _animator.SetTrigger("Attack");

        // Paramos cualquier coroutine anterior antes de lanzar una nueva
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }

        _attackCoroutine = StartCoroutine(StopAttackAfterDelay(1.1f)); // duración del ataque
        _lastAttackTime = Time.time;
    } 

    // Acá aplico el daño
    public void ApplyAttackDamage()
    {
        Debug.Log("ApplyAttackDamage ejecutado");
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Esto asegura que el daño solo ocurra si el player sigue dentro del rango en el instante correcto.
        if (distanceToPlayer <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Daño aplicado al jugador");
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Color para la visión
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        // Color para el wander
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_initialPosition != Vector3.zero ? _initialPosition : transform.position, wanderRadius);
    }

    // for animations
    private void HandleAnimations()
    {
        Vector3 velocity = _agent.velocity; // se toma la velocidad del agente 
        bool isWalking = velocity.magnitude > 0.1f;

        _animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            Vector2 direction = velocity.normalized;

            _animator.SetFloat("Move_X", direction.x);
            _animator.SetFloat("Move_Y", direction.y);
        }
    }
    
    IEnumerator StopAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.SetBool("isAttacking", false);
        _agent.isStopped = false; //  Reanuda el movimiento después del ataque 
    }
}
