using UnityEngine;

public class PlayerIdleState : IPlayer_State
{
    private readonly Player_Movement _player;

    public PlayerIdleState(Player_Movement player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.UpdateAnimation(Vector2.zero);
    }

    public void Exit() { }

    public void Update()
    {
        Vector2 input = _player.ReadInput();

        if (input != Vector2.zero)
        {
            _player.ChangeState(_player.moveState);
        }
    }

    public void FixedUpdate() { }
}
