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
       // _player._stateMachine.Animator.SetBool("InCombat", false);
    }

    public void Exit() { }

    public void Update()
    {
        Vector2 input = _player.ReadInput();

        if (Input.GetKeyDown(KeyCode.R))
        {
            _player._stateMachine.ChangeState(new Ground_Entry_State(_player._stateMachine));
        }

        if (input != Vector2.zero)
        {
            _player._stateMachine.ChangeState(_player.moveState);
        }

    }


    public void FixedUpdate() 
    
    { }
}
