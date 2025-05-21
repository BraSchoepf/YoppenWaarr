using UnityEngine;
using System.Collections;



[RequireComponent (typeof(Rigidbody2D))]
public class Player_Movement : MonoBehaviour
{
    [Header("Referencia al SO")]

    [SerializeField] private Character_Data_SO _character_Data_SO;

    private Rigidbody2D _rbPlayer;
    private Animator _animator;

    private Vector2 _lastMoveDirection = Vector2.down; // Dirección inicial por defecto
    private Vector2 _currentDirection;

    //Maquina de estados
    public Player_State_Machine _stateMachine; 
    public PlayerIdleState idleState;
    public PlayerMoveState moveState;

    private void Awake()
    {
        _rbPlayer = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        _stateMachine = GetComponent<Player_State_Machine>();
        _stateMachine.Initialize(_animator, this);
    }

    private void Start()
    {
        _stateMachine.ChangeState(idleState);
    }

    // Update is called once per frame
    void Update()
    {
           
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
           _stateMachine.FixedUpdate();
    }

    public void UpdateAnimation(Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            // Movimiento activo → guardamos la última dirección válida
            _lastMoveDirection = moveInput;

            _animator.SetFloat("Move_X", moveInput.x);
            _animator.SetFloat("Move_Y", moveInput.y);
            _animator.SetFloat("Speed", moveInput.sqrMagnitude);
        }
        else
        {
            // No hay movimiento → mostrar idle en la última dirección válida
            _animator.SetFloat("Move_X", _lastMoveDirection.x);
            _animator.SetFloat("Move_Y", _lastMoveDirection.y);

            // Para idles, usás los mismos valores
            _animator.SetFloat("Idle_X", _lastMoveDirection.x);
            _animator.SetFloat("Idle_Y", _lastMoveDirection.y);

            _animator.SetFloat("Speed", 0f);

            //Debug.Log($"IdleDir: {_lastMoveDirection}");
        }
    }
       
    public void ChangeState(IPlayer_State newState)
    {
        _stateMachine.ChangeState(newState);
    }

    public Vector2 ReadInput()
    {
        // Leer input y asignar dirección
        float _move_X = Input.GetAxisRaw("Horizontal");
        float _move_Y = Input.GetAxisRaw("Vertical");
        _currentDirection = new Vector2(_move_X, _move_Y).normalized;
        return _currentDirection;
    }

    public void ApplyMovement(Vector2 moveInput)
    {

        // Aplicar movimiento basado en velocidad del SO
        _rbPlayer.MovePosition(_rbPlayer.position + moveInput * _character_Data_SO.Speed * Time.fixedDeltaTime);
    }

    //AddForce con ForceMode2D.Impulse porque es un empujón rápido e inmediato (como un golpe físico).
    public void ApplyKnockback(Vector2 direction, float force)
    {
        StopAllCoroutines(); // Por si ya hay un knockback activo
        StartCoroutine(KnockbackCoroutine(direction, force));
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction, float force)
    {
        _rbPlayer.linearVelocity = Vector2.zero; // Paramos movimiento previo
        _rbPlayer.AddForce(direction.normalized * force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.15f); // Tiempo de deslizamiento

        _rbPlayer.linearVelocity = Vector2.zero; // Detenemos completamente
    }
}
