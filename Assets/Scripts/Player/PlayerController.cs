using UnityEngine;
using System.Collections;



[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Referencia al SO")]

    [SerializeField] private Character_Data_SO _character_Data_SO;

    private Rigidbody2D _rbPlayer;
    private Animator _animator;
    [SerializeField] public PlayerSlashHitbox slashHitbox;
    public Animator animator => _animator;
    public Vector2 FacingDirection => _lastMoveDirection;

    private Vector2 _lastMoveDirection = Vector2.down; // Default starting address
    private Vector2 _currentDirection;

    //State machine
    public Player_State_Machine stateMachine; 
    public PlayerIdleState idleState;
    public PlayerMoveState moveState;
    public PlayerAttackState attackState;


    private void Awake()
    {
        _rbPlayer = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        attackState = new PlayerAttackState(this);
        stateMachine = GetComponent<Player_State_Machine>();
        stateMachine.Initialize(_animator, this);
        
    }

    private void Start()
    {
        stateMachine.ChangeState(idleState);
    }

    // Update is called once per frame
    void Update()
    {
           
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
           stateMachine.FixedUpdate();
    }

    public void UpdateAnimation(Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            // Active movement → we save the last valid address
            _lastMoveDirection = moveInput;

            _animator.SetFloat("Move_X", moveInput.x);
            _animator.SetFloat("Move_Y", moveInput.y);
            _animator.SetFloat("Speed", moveInput.sqrMagnitude);
        }
        else
        {
            // No movement → show idle at the last valid address
            _animator.SetFloat("Move_X", _lastMoveDirection.x);
            _animator.SetFloat("Move_Y", _lastMoveDirection.y);

            // PFor idles use the same values
            _animator.SetFloat("Idle_X", _lastMoveDirection.x);
            _animator.SetFloat("Idle_Y", _lastMoveDirection.y);

            _animator.SetFloat("Speed", 0f);

            //Debug.Log($"IdleDir: {_lastMoveDirection}");
        }
    }
       
    public void ChangeState(IPlayer_State newState)
    {
        stateMachine.ChangeState(newState);
    }

    public Vector2 ReadInput()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsInDialogue)
        return Vector2.zero; // if is in dialogue do not read the inputs
        
        // Read input and assign address
        float _move_X = Input.GetAxisRaw("Horizontal");
        float _move_Y = Input.GetAxisRaw("Vertical");
        _currentDirection = new Vector2(_move_X, _move_Y).normalized;
        return _currentDirection;
    }

    public void ApplyMovement(Vector2 moveInput)
    {

        // Apply motion based on SO velocity
        _rbPlayer.MovePosition(_rbPlayer.position + moveInput * _character_Data_SO.Speed * Time.fixedDeltaTime);
    }

    //AddForce with ForceMode2D.Impulse because it is a quick and immediate push (like a physical blow).
    public void ApplyKnockback(Vector2 direction, float force)
    {
        StopAllCoroutines(); // In case there is already an active knockback
        StartCoroutine(KnockbackCoroutine(direction, force));
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction, float force)
    {
        _rbPlayer.linearVelocity = Vector2.zero; // We stop previous movement
        _rbPlayer.AddForce(direction.normalized * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.15f); 

        _rbPlayer.linearVelocity = Vector2.zero; 
    }

}
