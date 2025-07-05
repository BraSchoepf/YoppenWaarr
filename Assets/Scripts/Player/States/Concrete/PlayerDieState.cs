using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerDieState : IPlayer_State
{

    private PlayerController _player;
    private Animator _animator;
    private bool _hasPlayedAnimation = false;
    private float _timer;


    public PlayerDieState(PlayerController player)
    {
        _player = player;
        _animator = player.animator;
    }

    public void Enter()
    {
        _hasPlayedAnimation = false;
        _timer = 0f;

        string animName = _player.FacingDirection.x >= 0 ? "Die_Right" : "Die_Left";
        _animator.Play(animName);

        _player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    public void Update()
    {
        if (!_hasPlayedAnimation)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            string currentAnim = _player.FacingDirection.x >= 0 ? "Die_Right" : "Die_Left";

            if (stateInfo.IsName(currentAnim) && stateInfo.normalizedTime >= 1f)
            {
                _hasPlayedAnimation = true;
            }
        }
        else
        {
            _timer += Time.deltaTime;
            if (_timer >= 1f)
            {
                // Reiniciar escena o llamar al GameManager
                if (GameManager.Instance != null)
                    GameManager.Instance.GameOver();
                else
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    public void FixedUpdate() { }

    public void Exit() { }
}
