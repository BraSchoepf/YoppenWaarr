using UnityEngine;

public class PlayerVFXHandler : MonoBehaviour
{
    [Header("Prefabs de ataque 1")]
    [SerializeField] private GameObject _attack1Right;
    [SerializeField] private GameObject _attack1Left;
    [SerializeField] private GameObject _attack1Up;
    [SerializeField] private GameObject _attack1Down;

    [Header("Partícula de ataque 2 (embebida)")]
    [SerializeField] private ParticleSystem _attack2Effect;

    [Header("Configuración general")]
    [SerializeField] private Transform _vfxSpawnPoint;
    [SerializeField] private float _spawnDistance = 0.8f;

    [Header("Partícula de caminata (embebida)")]
    [SerializeField] private ParticleSystem _walkPoofEffect;

   

    public void PlayAttack1Effect(Vector2 direction)
    {
        if (_vfxSpawnPoint == null)
        {
            Debug.LogWarning("Falta el punto de spawn.");
            return;
        }

        direction = direction.normalized;
        Vector3 spawnPosition = _vfxSpawnPoint.position + (Vector3)(direction * _spawnDistance);

        GameObject prefabToUse = null;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal
            prefabToUse = direction.x >= 0 ? _attack1Right : _attack1Left;
        }
        else
        {
            // Vertical
            prefabToUse = direction.y >= 0 ? _attack1Up : _attack1Down;
        }

        if (prefabToUse == null)
        {
            Debug.LogWarning("No hay prefab asignado para esa dirección.");
            return;
        }

        GameObject effect = Instantiate(prefabToUse, spawnPosition, Quaternion.identity);

        ParticleSystem ps = effect.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
            ps.Play();

        Destroy(effect, 2f);
    }

    public void PlayAttack2Effect()
    {
        if (_attack2Effect == null)
        {
            Debug.LogWarning("Falta la partícula del ataque 2.");
            return;
        }

        _attack2Effect.Play();
    }
    private int _lastDirectionX = 0;

    public void EnsureWalkPoofActive(Vector2 direction)
    {
        if (_walkPoofEffect == null || direction.magnitude < 0.1f)
            return;

        int currentDirX = (int)Mathf.Sign(direction.x);

        // Si no está reproduciendo, o cambió de dirección
        if (!_walkPoofEffect.isPlaying || currentDirX != _lastDirectionX)
        {
            _walkPoofEffect.Play();
            _lastDirectionX = currentDirX;

            // Si querés cambiar rotación o escala, podés hacerlo acá también
        }
    }

    public void StopWalkPoof()
    {
        if (_walkPoofEffect != null && _walkPoofEffect.isPlaying)
        {
            _walkPoofEffect.Stop();
        }
    }
}
