using UnityEngine;

public class Player_State_Machine : MonoBehaviour
{
    private IPlayer_State _currentState;

    public Animator Animator { get; private set; }
    public PlayerController PlayerRef { get; private set; }

    public void Initialize(Animator animator, PlayerController player)
    {
        Animator = animator;
        PlayerRef = player;
    }

    public void ChangeState(IPlayer_State newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Update() => _currentState?.Update();
    public void FixedUpdate() => _currentState?.FixedUpdate();
}