using UnityEngine;

public class Ground_Finisher_State : Melee_Base_State
{
    private const float _attackDuration = 0.75f;
    public Ground_Finisher_State(Player_State_Machine stateMachine) : base(stateMachine)
    {
       
    }

    public override void Enter()
    {base.Enter();
        _animator.SetTrigger("Attack3");
        Debug.Log($"Attack 3 fired");
    }

    public override void Update()
    {
        base.Update();

        if (_timer >= _attackDuration)
            _state_Machine.ChangeState(new Idle_Combat_State(_state_Machine));
    }
    public override void Exit()
    {
        _animator.ResetTrigger("Attack3");
    }
}