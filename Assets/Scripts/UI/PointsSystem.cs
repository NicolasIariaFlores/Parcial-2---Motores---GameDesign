using TMPro;
using UnityEngine;

public class PointsSystem : MonoBehaviour
{
    public static PointsSystem Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI _textPointsAmount;
    [SerializeField] private int _pointsAmount;
    [SerializeField] private int _maxPoints;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

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
        if (_pointsAmount + entradeAmount > _maxPoints) { return; }
        _pointsAmount += entradeAmount;
        TextUpdate();
    }

    private void TextUpdate()
    {
        _textPointsAmount.text = _pointsAmount.ToString();
    }

    public bool TrySpend(int amount)
    {
        if (_pointsAmount >= amount)
        {
            _pointsAmount -= amount;
            TextUpdate();
            return true;
        }
        return false;
    }
    public int GetPoints() => _pointsAmount;
}
