using UnityEngine;

public class BossVisuals : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Material defaultMaterial;
    public Material additiveMaterial;

    public void SetAdditiveMaterial()
    {
        spriteRenderer.material = additiveMaterial;
    }

    public void ResetMaterial()
    {
        spriteRenderer.material = defaultMaterial;
    }
}
