using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    public void EnableComboWindow()
    {
        _player.attackState?.EnableComboWindow();
    }

    public void DisableComboWindow()
    {
        _player.attackState?.DisableComboWindow();
    }
}

