using UnityEngine;

public class PlayerMoveState : IPlayer_State
{
    private readonly PlayerController _player;

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
        Vector2 input = _player.ReadInput();
        _player.ApplyMovement(input);
    }
}
