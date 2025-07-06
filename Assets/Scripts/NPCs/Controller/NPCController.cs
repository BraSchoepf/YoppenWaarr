using System.Collections.Generic;
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

        // Obtener los clips actuales que se pueden overridear
        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrideController.GetOverrides(overrides);

        // Debug: Ver qué clips están disponibles para override
        foreach (var clipPair in overrides)
        {
            Debug.Log($"[DEBUG] Clip original en baseController: {clipPair.Key.name}, actual override: {clipPair.Value?.name ?? "null"}");
        }

        // Reemplazar el clip (asegurate de que el nombre original sea correcto)
        for (int i = 0; i < overrides.Count; i++)
        {
            if (overrides[i].Key.name == "Idle_Base") // <-- ajustá esto si tu clip original se llama distinto
            {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, npcData.idleClip);
            }
        }

        // Aplicar los cambios al override controller
        overrideController.ApplyOverrides(overrides);

        // Asignar el controller al Animator
        _animator.runtimeAnimatorController = overrideController;

        Debug.Log($"NPC '{npcData.npcName}' usa animación: {npcData.idleClip.name}");
    }
}

