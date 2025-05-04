using UnityEngine;

public class Idle_Combat_State : IPlayer_State
{
    private readonly Player_State_Machine _stateMachine;
    private readonly Animator _animator;
    private float _timer;

    public Idle_Combat_State(Player_State_Machine stateMachine)
    {
        _stateMachine = stateMachine;
        _animator = stateMachine.Animator;
    }

    public void Enter()
    {
        _timer = 0f;
        _animator.SetTrigger("IdleCombat");
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            _stateMachine.ChangeState(new Ground_Entry_State(_stateMachine));
        }

         else if (_timer >= 1f)
        {
            _animator.SetBool("InCombat", false);
            _stateMachine.ChangeState(new PlayerIdleState(_stateMachine.PlayerRef));
        }
    }

    public void FixedUpdate() { }

    public void Exit()
    {
        _animator.ResetTrigger("IdleCombat");
    }
}


