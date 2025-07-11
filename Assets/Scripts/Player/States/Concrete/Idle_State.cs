using UnityEngine;

public class PlayerIdleState : IPlayer_State
{
    private readonly PlayerController _player;

    public PlayerIdleState(PlayerController player)
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
            _player.stateMachine.ChangeState(_player.moveState);
        }

        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown("joystick button 0"))
        {
            _player.ChangeState(_player.attackState);
        }

    }


    public void FixedUpdate() 
    
    { }
}
