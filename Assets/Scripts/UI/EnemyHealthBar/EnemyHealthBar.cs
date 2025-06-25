using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Transform _target; // El enemigo
    [SerializeField] private Vector3 _offset = new Vector3(0, 1f, 0.1f);

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetHealth(float current, float max)
    {
        _fillImage.fillAmount = current / max;
    }

    private void LateUpdate()
    {
        if (_target != null)
        {
            transform.position = _target.position + _offset;
            transform.rotation = Quaternion.identity; // Para que no rote con la cámara
        }
    }
}
