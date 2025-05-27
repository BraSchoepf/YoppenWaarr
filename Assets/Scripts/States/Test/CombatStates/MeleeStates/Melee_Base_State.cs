using UnityEngine;

public class Melee_Base_State : IPlayer_State
{
    protected readonly Player_State_Machine _state_Machine;
    protected readonly Animator _animator;
    protected bool _inputQueued;
    protected float _timer;
    public Melee_Base_State(Player_State_Machine stateMachine)
    {
        _state_Machine = stateMachine;
        _animator = stateMachine.Animator;
       
    }

    public virtual void Enter() 
    {
        _timer = 0f;
        _inputQueued = false;
    }

    public virtual void Update()
    {
        _timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R))
            _inputQueued = true;
    }

    public virtual void FixedUpdate() { }

    public virtual void Exit() { }

}

