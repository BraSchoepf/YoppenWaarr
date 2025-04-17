using UnityEngine;


[RequireComponent (typeof(Rigidbody2D))]
public class Player_Movement : MonoBehaviour
{
    [Header("Referencia al SO")]

    [SerializeField] private Character_Data_SO _character_Data_SO;

    private Rigidbody2D _rbPlayer;
    private Animator _animator;

    // Por defecto, mirando hacia abajo
    private Vector2 _lastMoveDirection = Vector2.zero;

    private void Awake()
    {
        _rbPlayer = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        // Leer input y asignar direcci�n
        float _move_X = Input.GetAxisRaw("Horizontal");
        float _move_Y = Input.GetAxisRaw("Vertical");
        Vector2 move_input = new Vector2(_move_X, _move_Y).normalized;
        
        // Actualizar direcci�n en el SO
        _character_Data_SO.Direction = move_input;

        if (move_input != Vector2.zero)
        {
            // Hay movimiento: actualizar valores
            _lastMoveDirection = move_input;

            // Pasar al Animator los valores de direcci�n
            _animator.SetFloat("Move_X", move_input.x);
            _animator.SetFloat("Move_Y", move_input.y);
            _animator.SetFloat("Speed", move_input.sqrMagnitude);

        }
        else
        {
            // No hay movimiento: usar �ltima direcci�n v�lida para Idle correspondiente
            _animator.SetFloat("Move_X", _lastMoveDirection.x);
            _animator.SetFloat("Move_Y", _lastMoveDirection.y);
            _animator.SetFloat("Speed", 0f);
        }
        
    }

    private void FixedUpdate()
    {
        // Aplicar movimiento basado en velocidad del SO
        _rbPlayer.MovePosition(_rbPlayer.position + _character_Data_SO.Direction * _character_Data_SO.Speed * Time.fixedDeltaTime);
    }
}
