using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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

    // to block the direction of the attack
    private Vector2 _attackDirection;

    public bool isMovementBlocked = false; // block enemy AI behavior momentarily

    private EnemyHealth _enemyHealth; // Reference to EnemyHealth

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _initialPosition = transform.position;
        _enemyHealth = GetComponent<EnemyHealth>(); // Get the EnemyHealth component
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isMovementBlocked || _enemyHealth.IsDead) return; // We block AI while it is active or if the enemy is dead

        HandleEnemyLogic();

        HandleAnimations();
    }

    private void HandleEnemyLogic()
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
                if (_agent != null && _agent.enabled)
                {
                    _agent.SetDestination(player.position);
                }
            }
            else
            {
                if (_agent != null && _agent.enabled)
                {
                    _agent.SetDestination(player.position);
                }
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
                    _returningToInitialPosition = false;
                    _wanderTimer = _wanderCooldown;
                }
            }
            else
            {
                Wander();
            }
        }
    }

    bool CanSeePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleMask);
        if (hit.collider == null)
        {
            return true; // No obstacle in between
        }
        return false; // There is something blocking
    }

    void ChasePlayer()
    {
        _isChasing = true;
        _agent.speed = chaseSpeed;
        if (_agent != null && _agent.enabled)
        {
            _agent.SetDestination(player.position); // Usá esto en lugar de llamadas directas
        }
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
        //This ensures that the enemy will not initiate a new attack if he is already attacking,
        //but he will be able to attack again when he finishes and the cooldown allows it.
        if (Time.time - _lastAttackTime < attackCooldown || _animator.GetBool("isAttacking")) return;

        _agent.enabled = false; // desactivamos el NavMesh


        // to block the direction of the attack
        _attackDirection = (player.position - transform.position).normalized;
        _animator.SetFloat("Move_X", _attackDirection.x);
        _animator.SetFloat("Move_Y", _attackDirection.y);
        _animator.SetBool("isAttacking", true);

        //For attack to trigger the function ApplyAttackDamage() 1 time x hit and take 1HP - YW-ES-006 BUG-ES-005 --> QA testing
        //Activate animation attack
        _animator.SetTrigger("Attack");
        //StartCoroutine(SimulateAttackApproach(_attackDirection, 1.5f, 0.3f)); // Ajustá velocidad/duración si querés


        // We stop any previous coroutine before launching a new one.
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }

        _attackCoroutine = StartCoroutine(StopAttackAfterDelay(1.1f)); // duration of the attack
        _lastAttackTime = Time.time;
    } 

    
    public void ApplyAttackDamage()
    {
        Debug.Log("ApplyAttackDamage ejecutado");
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // This ensures that damage only occurs if the player is still within range at the correct time.
        if (distanceToPlayer <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Daño aplicado al jugador");
            }

            // Knockback
            PlayerController playerMovement = player.GetComponent<PlayerController>();
            if (playerMovement != null)
            {
                Vector2 knockbackDir = (player.position - transform.position).normalized;
                playerMovement.ApplyKnockback(knockbackDir, 5f); 
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        // Color for vision
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        // Color for wander
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_initialPosition != Vector3.zero ? _initialPosition : transform.position, wanderRadius);
    }

    // for animations
    private void HandleAnimations()
    {
        if (_animator.GetBool("isAttacking")) return; // prevent changes if it is attacking

        Vector3 velocity = _agent.velocity; // the agent's speed is taken 
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
        _agent.enabled = true;
        _agent.isStopped = false; //  Resumes movement after the attack 
    }

    public void StopAttack()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
        _animator.SetBool("isAttacking", false);
        _agent.enabled = true;
    }

    public void ResumeAttack()
    {
        if (Time.time - _lastAttackTime >= attackCooldown)
        {
            StartCoroutine(StopAttackAfterDelay(1.1f)); // duration of the attack
        }
    }
}
