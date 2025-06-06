using UnityEngine;
using UnityEngine.UI;

public class AllyHealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Vector3 _offset;

    private Transform _target;
    private Transform _cameraTransform;

    public void Initialize(Transform target)
    {
        _target = target;
    }

    void Start()
    {
        // Intentar obtener la cámara principal solo una vez
        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró una cámara con el tag 'MainCamera'. Asegúrate de que exista.");
        }
    }

    public void UpdateHealth(float current, float max)
    {
        _fillImage.fillAmount = current / max;
    }

    void LateUpdate()
    {
        if (_target == null || _cameraTransform == null) return;

        transform.position = _target.position + _offset;
        transform.forward = _cameraTransform.forward;
    }
}