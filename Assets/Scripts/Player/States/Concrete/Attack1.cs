/*using UnityEngine;

public class Attack1 : Melee_Base_State
{
    private const float _attackDuration = 1.2f;

    public Attack1(Player_State_Machine stateMachine) : base(stateMachine) { }
    

    public override void Enter()
    {
        base.Enter();
        _animator.SetTrigger("Attack1");
        Debug.Log($"Attack 1 fired");
    }

    public override void Update()
    {

        base.Update();

        

        if (_timer >= _attackDuration)
        {
            _state_Machine.ChangeState(_inputQueued
                ? new Ground_Combo_State(_state_Machine)
                : new Idle_Combat_State(_state_Machine));
        }
    }

    public override void Exit()
    {
        _animator.ResetTrigger("Attack1");
    }
    
}*/