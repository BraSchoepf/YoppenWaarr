using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAttackState : IPlayer_State
{
    private readonly PlayerController _player;
    private int _comboIndex = 0;
    protected float _attackTimer = 0f;
    private float _attackDuration = 1.2f;
    protected bool _inputQueued = false;
    private bool _inputRegistered = false;
    private readonly int _totalCombos = 2;
    private float _comboWindow = 0.6f;

    private bool isInAnimation => _player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;

    public PlayerAttackState(PlayerController player)
    {
        this._player = player;

    }

    public virtual void Enter() 
    {
        _comboIndex = 0;
        PlayCurrentComboAnimator();
        _inputQueued = false;
        _inputRegistered = false;
        _attackTimer = 0f;
    }

    public virtual void Exit()
    {
        _player.animator.SetFloat("AttackIndex", -1f);
        _player.animator.SetBool("IsAttacking", false);
    }

    public virtual void Update()
    {
        _attackTimer += Time.deltaTime;

        if (!_inputRegistered && Input.GetKeyDown(KeyCode.J) && _attackTimer > _comboWindow)
        {
            _inputQueued = true;
            _inputRegistered = true;
        }

        if (_attackTimer >= _attackDuration)
        {
            if (_inputQueued && _comboIndex < _totalCombos)
            {
                _comboIndex++;
                PlayCurrentComboAnimator();
                _attackTimer = 0f;
                _inputQueued = false;
                _inputRegistered = false;
                
            }

            else
            {
                _player.ChangeState(_player.idleState);
            }
        }      
    }

    public virtual void FixedUpdate() 
    {
        _player.ApplyMovement(Vector2.zero);
    }

    private void PlayCurrentComboAnimator()
    {
        _player.animator.SetFloat("AttackIndex", _comboIndex);
        _player.animator.SetFloat("FacingX", _player.FacingDirection.x);
        _player.animator.SetBool("IsAttacking", true);
    }
}