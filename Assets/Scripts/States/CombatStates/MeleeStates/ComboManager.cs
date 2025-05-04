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

            // Detectar si está mirando a la derecha o izquierda
            float moveX = _playerMovement.GetComponent<Animator>().GetFloat("Move_X");
            bool isFacingRight = moveX > 0f;

            _animator.SetBool("isFacingRight", isFacingRight);
            _animator.SetInteger("ComboIndex", _comboIndex);
            _animator.SetTrigger("Combo");

            
           

            Debug.Log("Move_X: " + moveX  + " | isFacingRight: " + isFacingRight);

            _canReceiveInput = false;
        }
    }

    // Este método se llama desde eventos en las animaciones
    public void EnableInput()
    {
        _canReceiveInput = true;
    }
}



