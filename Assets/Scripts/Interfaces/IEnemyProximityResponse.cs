using UnityEngine;

public interface IEnemyProximityResponse
{
    void OnEnemyEnter(Transform enemy);
    void OnEnemyExit();
}
