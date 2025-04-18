using UnityEngine;

public class Player_State_Machine
{
    private IPlayer_State _currentState;

    public void ChangeState(IPlayer_State newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Update() => _currentState?.Update();
    public void FixedUpdate() => _currentState?.FixedUpdate();

}
