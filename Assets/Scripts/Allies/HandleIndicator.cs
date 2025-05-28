using UnityEngine;

public class HandleIndicator : MonoBehaviour, IProximityResponse
{
    [SerializeField] private GameObject _indicator;
    private Vector3 _originalScale;
    private Coroutine _fadeCoroutine;


    private void Start()
    {
        if (_indicator != null)
        {
            _originalScale = _indicator.transform.localScale;
            _indicator.SetActive(false);
        }
    }

    public void OnPlayerEnter()
    {
        if (_indicator != null)
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _indicator.SetActive(true);
            _indicator.transform.localScale = _originalScale;
        }
    }

    public void OnPlayerExit()
    {
        if (_indicator != null)
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = StartCoroutine(ScaleDownAndDisable());
        }
    }
    public void DisableIndicator()
    {
        if (_indicator != null)
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = StartCoroutine(ScaleDownAndDisable());
        }
    }
     private System.Collections.IEnumerator ScaleDownAndDisable()
    {
        float duration = 0.35f;
        float time = 0f;
        Vector3 startScale = _indicator.transform.localScale;
        Vector3 endScale = Vector3.zero;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            _indicator.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        _indicator.SetActive(false);
    }
}