using UnityEngine;

public class ComboManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _comboResetDelay = 1.0f;
    [SerializeField] private Player_Movement _playerMovement;

    private int _comboIndex = 0;
    private float _lastClickTime;
    private bool _canReceiveInput = true;

    private void Update()
    {
        if (Time.time - _lastClickTime > _comboResetDelay)
        {
            _comboIndex = 0;
            _canReceiveInput = true;
        }

        if (Input.GetMouseButtonDown(0) && _canReceiveInput)
        {
            _lastClickTime = Time.time;
            _comboIndex++;
            _comboIndex = Mathf.Clamp(_comboIndex, 1, 3);

            // Detect whether you are looking to the right or to the left
            float moveX = _playerMovement.GetComponent<Animator>().GetFloat("Move_X");
            bool isFacingRight = moveX > 0f;

            _animator.SetBool("isFacingRight", isFacingRight);
            _animator.SetInteger("ComboIndex", _comboIndex);
            _animator.SetTrigger("Combo");

            
           

            Debug.Log("Move_X: " + moveX  + " | isFacingRight: " + isFacingRight);

            _canReceiveInput = false;
        }
    }

    // This method is called from events in the animations
    public void EnableInput()
    {
        _canReceiveInput = true;
    }
}



