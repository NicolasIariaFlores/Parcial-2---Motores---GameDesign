using UnityEngine;

public class NPCStats : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 2f;

    public float BaseSpeed => _baseSpeed;
}
