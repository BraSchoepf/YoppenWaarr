using UnityEngine;

public class PlayerMoveState : IPlayer_State
{
    private readonly PlayerController _player;
    private Vector2 _currentVelocity = Vector2.zero;
    private float _acceleration = 10f;

    public PlayerMoveState(PlayerController player)
    {
        _player = player;
    }

    public void Enter() { }

    public void Exit() { }

    public void Update()
    {
        Vector2 input = _player.ReadInput();

        if (input == Vector2.zero)
        {
            _player.ChangeState(_player.idleState);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            _player.ChangeState(_player.attackState);
        }

        _player.UpdateAnimation(input);
    }

    public void FixedUpdate()
    {
        Vector2 input = _player.ReadInput().normalized;

        _currentVelocity = Vector2.Lerp(_currentVelocity, input, _acceleration * Time.fixedDeltaTime);
        _player.ApplyMovement(_currentVelocity);
    }
}
