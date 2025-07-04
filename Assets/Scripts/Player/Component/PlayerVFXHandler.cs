using UnityEngine;

public class PlayerVFXHandler : MonoBehaviour
{
    [Header("Prefabs de ataque 1")]
    [SerializeField] private GameObject _attack1EffectRight;
    [SerializeField] private GameObject _attack1EffectLeft;

    [Header("Part�cula de ataque 2 (embebida)")]
    [SerializeField] private ParticleSystem _attack2Effect;

    [Header("Configuraci�n general")]
    [SerializeField] private Transform _vfxSpawnPoint;
    [SerializeField] private float _spawnDistance = 0.8f;

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

        // Elegimos el prefab seg�n direcci�n
        if (direction.x >= 0 && Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            prefabToUse = _attack1EffectRight;
        }
        else if (direction.x < 0 && Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            prefabToUse = _attack1EffectLeft;
        }
        else
        {
            // No reproducimos nada en vertical
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
            Debug.LogWarning("Falta la part�cula del ataque 2.");
            return;
        }

        _attack2Effect.Play();
    }
}


