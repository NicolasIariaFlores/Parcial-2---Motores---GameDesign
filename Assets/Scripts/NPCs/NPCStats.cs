using UnityEngine;

public class NPCStats : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 15f;

    public float BaseSpeed => _baseSpeed;
}
