using TMPro;
using UnityEngine;

public class PointsSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textPointsAmount;
    [SerializeField] private int _pointsAmount;
    [SerializeField] private int _maxPoints;

    void Start()
    {
        TextUpdate();
    }
    void OnEnable()
    {
        Points.PointCollected += SumePoints;
    }
    void OnDisable()
    {
        Points.PointCollected -= SumePoints;
    }

    private void SumePoints(int entradeAmount)
    {
        if (_pointsAmount + entradeAmount > _maxPoints){ return; }
        _pointsAmount += entradeAmount;
        TextUpdate();
    }

    private void TextUpdate()
    {
        _textPointsAmount.text = _pointsAmount.ToString();
    }
}
