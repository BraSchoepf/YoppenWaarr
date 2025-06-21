using UnityEngine;

public class PlayerAttackState : IPlayer_State
{
    private readonly PlayerController _player;

    private int _comboIndex = 0;
    private float _attackTimer = 0f;
    private float _attackDuration = 0f;

    // Duraciones por combo, ajustá según tu animación
    private readonly float[] _attackDurations = { 1.6f, 2f };
    private const int _totalCombos = 2;

    private bool _inputBuffered = false;
    private bool _inputWindowOpen = false;

    public PlayerAttackState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        _attackTimer = 0f;
        _inputBuffered = false;
        _inputWindowOpen = false;

        // Mantenemos el combo actual (no reiniciar)
        _attackDuration = _attackDurations[_comboIndex];
        PlayCurrentComboAnimation();
    }

    public void Exit()
    {
        _player.animator.Play("Idle_Tree",0);
    }

    public void Update()
    {
        _attackTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (_inputWindowOpen)
            {
                _inputBuffered = true;
            }
        }

        if (_attackTimer >= _attackDuration)
        {
            if (_inputBuffered && _comboIndex < _totalCombos - 1)
            {
                _comboIndex++;
                _attackTimer = 0f;
                _inputBuffered = false;
                _inputWindowOpen = false;
                _attackDuration = _attackDurations[_comboIndex];
                PlayCurrentComboAnimation();
            }
            else
            {
                _comboIndex = 0; // Reiniciamos combo
                _player.ChangeState(_player.idleState);
            }
        }
    }

    public void FixedUpdate()
    {
        _player.ApplyMovement(Vector2.zero); // Detener al atacar
    }

    private void PlayCurrentComboAnimation()
    {
        string direction = GetFacingDirection();
        string animName = $"Attack{_comboIndex + 1}_{direction}";

        Debug.Log("Reproduciendo animación: " + animName);
        _player.animator.Play(animName);
    }

    private string GetFacingDirection()
    {
        Vector2 dir = _player.FacingDirection;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return dir.x >= 0 ? "Right" : "Left";
        else
            return dir.y >= 0 ? "Up" : "Down";
    }

    // Métodos llamados desde eventos de animación
    public void EnableComboWindow() => _inputWindowOpen = true;
    public void DisableComboWindow() => _inputWindowOpen = false;
}
