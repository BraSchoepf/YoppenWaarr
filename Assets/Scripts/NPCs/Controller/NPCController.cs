using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private NPCData npcData;

    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        ApplyOverride();
    }

    private void ApplyOverride()
    {
        if (npcData == null || npcData.baseController == null || npcData.idleClip == null)
        {
            Debug.LogError($"{gameObject.name}: Falta información en el NPCData.");
            return;
        }

        // Crear un nuevo override controller basado en el baseController
        AnimatorOverrideController overrideController = new AnimatorOverrideController(npcData.baseController);

        // Debug: Ver qué clips están disponibles para override
        foreach (var clipPair in overrideController.clips)
        {
            Debug.Log($"[DEBUG] Clip original en baseController: {clipPair.originalClip}");
        }

        // Reemplazar el clip (ajustá el nombre si el original no es exactamente "Idle")
        overrideController["Idle_Base"] = npcData.idleClip;

        // Aplicar el override al Animator
        _animator.runtimeAnimatorController = overrideController;

        Debug.Log($"NPC '{npcData.npcName}' usa animación: {npcData.idleClip.name}");
    }


}

