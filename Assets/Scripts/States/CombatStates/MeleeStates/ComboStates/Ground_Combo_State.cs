using UnityEngine;

public class Ground_Combo_State : Melee_Base_State
{
    private const float _attackDuration = 1f;
    public Ground_Combo_State(Player_State_Machine stateMachine) : base(stateMachine)   
    {
       
    }

    public override void Enter()
    {
        base.Enter();
       
        _animator.SetTrigger("Attack2");
        Debug.Log("Player Attack 2 Fired!");
    }

    public override void Update()
    {
        
        base.Update();

        if (_timer >= _attackDuration)
        {
            _state_Machine.ChangeState(_inputQueued
                ? new Ground_Finisher_State(_state_Machine)
                : new Idle_Combat_State(_state_Machine));
        }
    }

    public override void Exit()
    {
        _animator.ResetTrigger("Attack2");
    }
}
