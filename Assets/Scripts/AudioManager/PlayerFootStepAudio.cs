using UnityEngine;
using FMODUnity;

public class PlayerFootStepAudio : MonoBehaviour
{
    [Header("FMOD")]
    [SerializeField] private EventReference _stepEvent;

    [Header("LayerMask por terreno")]
    [SerializeField] private LayerMask _sandMask;
    [SerializeField] private LayerMask _woodMask;

    [Header("Raycast settings")]
    [SerializeField] private float _rayLength = 2.5f;

    private void Awake()
    {
        //#TEST
        if (!_stepEvent.IsNull)
            Debug.Log("Step event asignado correctamente.");
        else
            Debug.LogWarning("El eventReference de pasos es nulo");
    }

    // This function is called from an Animation Event
    public void PlayFootstep()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
       
        float materialValue = DetectMaterialValue();
        AudioManager.Instance.PlayStepWithMaterial(_stepEvent, materialValue, transform.position);
        ////#TEST Debug.Log($"Llamando a PlayFootstep. Valor de material: {materialValue}");

    }

    private float DetectMaterialValue()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, _rayLength, _sandMask))
            return 0f; // Sand
        if (Physics2D.Raycast(transform.position, Vector2.down, _rayLength, _woodMask))
            return 1f; // Wood

        return 0f; // Default: Sand
        ////#TEST Debug.Log("Raycast hacia abajo: " + Physics2D.Raycast(transform.position, Vector2.down, _rayLength, _sandMask));

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _rayLength);
    }
}
