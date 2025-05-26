using UnityEngine;

public class NPCStats : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 4f;

    public float BaseSpeed => _baseSpeed;
}
