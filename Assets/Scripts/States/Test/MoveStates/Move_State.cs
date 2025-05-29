using UnityEngine;

public class PlayerMoveState : IPlayer_State
{
    private readonly Player_Movement _player;

    public PlayerMoveState(Player_Movement player)
    {
        _player = player;
    }

    public void Enter() { }

    public void Exit() { }

    public void Update()
    {
        Vector2 input = _player.ReadInput();

        if (Input.GetKeyDown(KeyCode.R))
        {
            _player.stateMachine.ChangeState(new Ground_Entry_State(_player.stateMachine));
        }

        if (input == Vector2.zero)
        {
            _player.ChangeState(_player.idleState);
        }

        _player.UpdateAnimation(input);
    }

    public void FixedUpdate()
    {
        Vector2 input = _player.ReadInput();
        _player.ApplyMovement(input);
    }
}
