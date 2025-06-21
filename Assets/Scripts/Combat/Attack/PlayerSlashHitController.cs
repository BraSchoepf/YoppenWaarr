using UnityEngine;

public class PlayerSlashHitController : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [Header("Hitbox por dirección")]
    [SerializeField] private PlayerSlashHitbox hitboxRight;
    [SerializeField] private PlayerSlashHitbox hitboxLeft;
    [SerializeField] private PlayerSlashHitbox hitboxUp;
    [SerializeField] private PlayerSlashHitbox hitboxDown;

    private PlayerSlashHitbox activeHitbox;

    public void EnableCurrentHitbox()
    {
        Vector2 dir = player.FacingDirection;

        if (dir.x > 0.5f)
            activeHitbox = hitboxRight;
        else if (dir.x < -0.5f)
            activeHitbox = hitboxLeft;
        else if (dir.y > 0.5f)
            activeHitbox = hitboxUp;
        else
            activeHitbox = hitboxDown;

        activeHitbox.EnableHitbox();
    }

    public void DisableCurrentHitbox()
    {
        if (activeHitbox != null)
        {
            activeHitbox.DisableHitbox();
            activeHitbox = null;
        }
    }
}
